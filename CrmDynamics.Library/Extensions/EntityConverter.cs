using CrmDynamics.Library.Models.Abstractions;
using CrmDynamics.Library.Models.Crm;
using CrmDynamics.Library.Workers.Cache.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CrmDynamics.Library.Extensions
{
    public static class EntityConverter
    {
        public static EntityReference ToEntityReference(this Entity entity) => new EntityReference(entity.LogicalName, entity.Id);

        internal static ExpandoObject ConvertToExpandoObject(this Entity entity, ICrmCache crmCache)
        {
            IDictionary<string, object> expandoObject = new ExpandoObject();

            foreach (var pair in entity.Attributes)
            {
                if (pair.Value != null)
                {
                    if (pair.Value is EntityReference entityReference)
                    {
                        var realationshipNavigationProperty = crmCache.GetRelationshipNavigationPropertyName(pair.Key, entity.LogicalName);
                        if (string.IsNullOrEmpty(realationshipNavigationProperty)) expandoObject.Add($"{pair.Key}@odata.bind", $"/{crmCache.GetEntityDefinitionSchemaName(entityReference.LogicalName)}({entityReference.Id})");
                        else expandoObject.Add($"{realationshipNavigationProperty}@odata.bind", $"/{crmCache.GetEntityDefinitionSchemaName(entityReference.LogicalName)}({entityReference.Id})");
                        continue;
                    }

                    if (pair.Value is OptionSetValue optionSetValue)
                    {
                        expandoObject.Add(pair.Key, optionSetValue.Value);
                        continue;
                    }

                    if (pair.Value is Money money)
                    {
                        expandoObject.Add(pair.Key, money.Value);
                        continue;
                    }

                    expandoObject.Add(pair.Key, pair.Value);
                    continue;
                }
            }

            return (ExpandoObject)expandoObject;
        }

        internal static List<Entity> ConvertToEntities(this IEnumerable<JToken> jTokens, string entityName, ICrmCache crmCache)
        {
            var list = new List<Entity>();
            foreach (var token in jTokens)
            {
                IDictionary<string, object> expandoObject = token.ToObject<ExpandoObject>();

                var entity = new Entity(entityName)
                {
                    Id = Guid.Parse(expandoObject.First(obj => obj.Key == $"{entityName}id").Value.ToString())
                };

                foreach (var pair in expandoObject)
                {
                    if (pair.Key.Contains("@odata.etag")) continue;

                    if (pair.Key.Contains("_value")) // lookup
                    {
                        var match = Regex.Match(pair.Key, @"_([A-za-z0-9]*)_value");

                        if (match.Success)
                        {
                            var entityId = Guid.Parse(pair.Value.ToString());
                            var attributes = crmCache.GetValueUpdateCache<EntityAttribute>($"EntityDefinitions(LogicalName='{entityName}')/Attributes(LogicalName='{match.Groups[1].Value}')", "Get", DateTimeOffset.Now.AddDays(1));

                            if (attributes.Targets.Count == 1)
                            {
                                entity[match.Groups[1].Value] = new EntityReference(attributes.Targets.First(), entityId);
                                continue;
                            }

                            var correctEntityName = crmCache.GetCorrectTargetEntityName(attributes.Targets, entityId);
                            if (!string.IsNullOrWhiteSpace(correctEntityName)) entity[match.Groups[1].Value] = new EntityReference(correctEntityName, entityId);

                            continue;
                        }
                    }
                    else
                    {
                        var attributesDefinitions = crmCache.GetValueUpdateCache<EntityAttribute>($"EntityDefinitions(LogicalName='{entityName}')/Attributes(LogicalName='{pair.Key}')", "Get", DateTimeOffset.Now.AddDays(1));
                        if (attributesDefinitions == null) continue;

                        var entityDefinitions = crmCache.GetValueUpdateCache<EntityDefinitions>($"EntityDefinitions(LogicalName='{entityName}')", "Get", DateTimeOffset.Now.AddDays(1));

                        if (attributesDefinitions.AttributeTypeName.Value == "PicklistType")
                        {
                            var options = crmCache.GetPicklistOptions(entityName, pair.Key);
                            var optionSetValue = Convert.ToInt32(pair.Value);
                            entity[pair.Key] = new OptionSetValue(optionSetValue, options.FirstOrDefault(val => val.Key == optionSetValue).Value);
                            continue;
                        }
                    }

                    entity[pair.Key] = pair.Value;
                }

                list.Add(entity);
            }

            return list;
        }
    }
}
