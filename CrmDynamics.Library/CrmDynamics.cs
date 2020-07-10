using CrmDynamics.Library.Extensions;
using CrmDynamics.Library.Models.Abstractions;
using CrmDynamics.Library.Models.Crm;
using CrmDynamics.Library.Models.Extensions;
using CrmDynamics.Library.Models.Query.Requests;
using CrmDynamics.Library.Models.Query.Responses;
using CrmDynamics.Library.Workers.Cache;
using CrmDynamics.Library.Workers.Cache.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebProxy = CrmDynamics.Library.Workers.Web.WebProxy;

namespace CrmDynamics.Library
{
    public class CrmDynamics : ICrmProxy
    {
        private readonly WebProxy _webProxy;
        protected readonly ICrmCache _crmCache;

        public CrmDynamics(string apiUrl, NetworkCredential credential, ICrmCache crmCache = null)
        {
            _crmCache = crmCache ?? new ApplicationCache(apiUrl, credential);
            _webProxy = new WebProxy(apiUrl, credential, _crmCache);
        }

        public Guid Create(Entity entity)
        {
            var response = _webProxy.GetResponse(_crmCache.GetEntityDefinitionSchemaName(entity.LogicalName), "Post", entity.ConvertToExpandoObject(_crmCache));

            var idString = response?.Headers?.GetValues("OData-EntityId").FirstOrDefault();

            if (string.IsNullOrEmpty(idString))
                return Guid.Empty;

            var entityIdParts = idString.Split(new[] { "(", ")" }, StringSplitOptions.None);

            return Guid.TryParse(entityIdParts[1], out var idGuid) ? idGuid : Guid.Empty;

        }

        public void Update(Entity entity)
        {
            var transaction = new ExecuteTransactionRequest();
            foreach (var attribute in entity.Attributes.ToList())
            {
                var attributes = _crmCache.GetValueUpdateCache<EntityAttribute>($"EntityDefinitions(LogicalName='{entity.LogicalName}')/Attributes(LogicalName='{attribute.Key}')", "Get", DateTimeOffset.Now.AddDays(1));

                if (attributes.AttributeTypeName.Value == "LookupType" && attribute.Value == null)
                {
                    var diasociateReq = new DisassociateRequest(entity.ToEntityReference(), attribute.Key);
                    transaction.AddRequest(diasociateReq);
                    entity.Attributes.Remove(attribute.Key);
                }
            }

            var updReq = new UpdateRequest();
            updReq.Target = entity;

            transaction.AddRequest(updReq);

            var response = _webProxy.GetTransactionResponse(transaction);
        }

        public void Delete(string entityName, Guid id)
        {
            _webProxy.GetResponse(_crmCache.GetEntityDefinitionSchemaName(entityName) + $"({id})", "Delete");
        }

        public ExecuteTransactionResponse ExecuteTransaction(ExecuteTransactionRequest request)
        {
            var response = _webProxy.GetTransactionResponse(request);
            return response;
        }

        public List<Entity> Fetch(string fetchXml)
        {
            var entityName = fetchXml;
            var startIndex = entityName.IndexOf("<entity name='", StringComparison.Ordinal);
            var substring = entityName.Substring(startIndex, fetchXml.Length - startIndex);
            var parts = substring.Split(new[] { "'" }, StringSplitOptions.None);
            entityName = parts.First(x => !x.Contains('<') && !x.Contains('>') && !x.Contains('='));

            var response = _webProxy.GetResponse(_crmCache.GetEntityDefinitionSchemaName(entityName) + $"?fetchXml={Uri.EscapeUriString(fetchXml)}", "Get");

            var jTokens = JObject.Parse(response.Content.ReadAsStringAsync().Result)["value"].ToList();
            return jTokens.ConvertToEntities(entityName, _crmCache);
        }

        public WhoAmI WhoAmI()
        {
            return _webProxy.GetResponse<WhoAmI>("WhoAmI", "Get");
        }

        public Dictionary<int, string> OptionsMetadata(OptionSetMetadataRequest request)
        {
            return _crmCache.GetPicklistOptions(request.EntityName, request.AttributeKey);
        }

        public void ExecuteWorkflow(Guid workflowId, Guid entityId)
        {
            _webProxy.GetResponse($"workflows(" + workflowId + ")/Microsoft.Dynamics.CRM.ExecuteWorkflow", "Post", new { EntityId = entityId });
        }

        public Entity Retrieve(string entityName, Guid entityId, params string[] columns)
        {
            var fetchXml = $@"<fetch top='50'>
                                <entity name='{entityName}'>
                                {string.Join(string.Empty, columns.Select(x => "<attribute name='" + x + "'/>"))}
                                    <filter type='and'>
                                        <condition attribute='{entityName}id' operator='eq' value='{entityId}'/>
                                    </filter>
                                </entity>
                            </fetch>";

            var entity = Fetch(fetchXml).FirstOrDefault();
            if (entity == null) throw new NullReferenceException($"Сущности {entityName} c id {entityId} не существует");

            return entity;
        }

        public void SetState(string entityName, Guid id, int statuscode, int statecode)
        {
            _webProxy.GetResponse($"{_crmCache.GetEntityDefinitionSchemaName(entityName)}({id})", "Patch", new { statuscode, statecode });
        }
    }
}
