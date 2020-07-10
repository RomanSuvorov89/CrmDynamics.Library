using CrmDynamics.Library.Extensions;
using CrmDynamics.Library.Models.Abstractions;
using CrmDynamics.Library.Workers.Cache.Models;
using CrmDynamics.Library.Workers.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebProxy = CrmDynamics.Library.Workers.Web.WebProxy;

namespace CrmDynamics.Library.Workers.Cache
{
    public class ApplicationCache : ICrmCache
    {
        private readonly WebProxy _webProxy;
        private RelationshipDefinitions RelationshipDefinitions => GetValueUpdateCache<RelationshipDefinitions>("RelationshipDefinitions/Microsoft.Dynamics.CRM.OneToManyRelationshipMetadata?$select=SchemaName,ReferencedAttribute,ReferencedEntity,ReferencingAttribute,ReferencingEntity,ReferencedEntityNavigationPropertyName", "GET", DateTimeOffset.Now.AddDays(1));

        public ApplicationCache(string apiUrl, NetworkCredential credential)
        {
            _webProxy = new WebProxy(apiUrl, credential, this);
        }

        public ApplicationCache(WebProxy webProxy)
        {
            _webProxy = webProxy;
        }

        public object GetValueUpdateCache(string url, string method, DateTimeOffset? timeOffset = null) => GetValueUpdateCache<object>(url, method, timeOffset);

        public T GetValueUpdateCache<T>(string url, string method, DateTimeOffset? timeOffset = null)
        {
            var value = MemoryCacheHelper.GetValue<T>(url + method);

            if (value == null)
            {
                try
                {
                    var response = _webProxy.GetResponse<T>(url, method);
                    MemoryCacheHelper.AddValue(url + method, response, timeOffset ?? DateTimeOffset.Now.AddMinutes(5));
                    return response;
                }
                catch
                {
                    return default;
                }
            }

            return value;
        }

        public string GetCorrectTargetEntityName(IList<string> targets, Guid id)
        {
            var correctEntityName = MemoryCacheHelper.GetValue<string>(string.Join("", targets) + id.ToString());

            if (string.IsNullOrWhiteSpace(correctEntityName))
            {
                foreach (var entityNameTarget in targets)
                {
                    var targetEntity = GetValueUpdateCache($"{GetEntityDefinitionSchemaName(entityNameTarget)}({id})?$select={entityNameTarget}id", "Get", DateTimeOffset.Now.AddDays(1));
                    if (targetEntity == null) continue;

                    MemoryCacheHelper.AddValue(string.Join("", targets) + id.ToString(), entityNameTarget, DateTimeOffset.Now.AddDays(1));
                    return entityNameTarget;
                }
            }

            return correctEntityName;
        }

        public string GetRelationshipNavigationPropertyName(string relationshipSchemaName, string entityName)
        {
            var relationshipMetadataId = RelationshipDefinitions.Realationships.SingleOrDefault(val => val.SchemaName == relationshipSchemaName)?.MetadataId;
            if (!relationshipMetadataId.HasValue || relationshipMetadataId.Value == Guid.Empty) return null;
            return GetRelationshipNavigationPropertyName(relationshipMetadataId.Value, entityName);
        }

        public string GetReferencedEntityNavigationPropertyName(string entityName, string referencingAttributeKey)
        {
            var relationship = RelationshipDefinitions.Realationships.SingleOrDefault(val => val.ReferencingAttribute == referencingAttributeKey && val.ReferencingEntity == entityName);
            return relationship.ReferencedEntityNavigationPropertyName;
        }

        public string GetRelationshipNavigationPropertyName(Guid metadataId, string entityName)
        {
            var relationship = GetValueUpdateCache<RelationshipDefinitionsMetadata>($"RelationshipDefinitions({metadataId})", "GET", DateTimeOffset.Now.AddDays(1));
            return relationship.ReferencingEntity.ToLower() == entityName.ToLower() ? relationship.ReferencingEntityNavigationPropertyName : string.Empty;
        }

        public string GetEntityDefinitionSchemaName(string entityLogicalName)
        {
            var definitions = GetValueUpdateCache<EntityDefinitions>($"EntityDefinitions(LogicalName='{entityLogicalName.ToLower()}')", "GET");
            return definitions.CollectionSchemaName.ToLower();
        }

        public Dictionary<int, string> GetPicklistOptions(string entityName, string attributeKey)
        {
            var pickListOptionsMetadata = MemoryCacheHelper.GetValue<Dictionary<int, string>>(entityName + attributeKey + "metadata");

            if (pickListOptionsMetadata == null)
            {
                dynamic definitions = GetValueUpdateCache<dynamic>($"EntityDefinitions(LogicalName='{entityName}')/Attributes(LogicalName='{attributeKey}')?$select=MetadataId", "Get", DateTimeOffset.Now.AddMinutes(60));
                var metadata = GetValueUpdateCache<PicklistAttributeMetadata>($"EntityDefinitions(LogicalName='{entityName}')/Attributes({definitions["MetadataId"]})/Microsoft.Dynamics.CRM.PicklistAttributeMetadata?$select=LogicalName&$expand=OptionSet($select=Options),GlobalOptionSet($select=Options)", "Get", DateTimeOffset.Now.AddMinutes(60));

                pickListOptionsMetadata = new Dictionary<int, string>();

                foreach (var option in metadata.GlobalOptionSet?.Options ?? metadata.OptionSet.Options)
                {
                    pickListOptionsMetadata.Add(option.Value, option.Label.UserLocalizedLabel.Label);
                }

                MemoryCacheHelper.AddValue(entityName + attributeKey + "metadata", pickListOptionsMetadata, DateTimeOffset.Now.AddMinutes(60));
            }


            return pickListOptionsMetadata;
        }
    }
}
