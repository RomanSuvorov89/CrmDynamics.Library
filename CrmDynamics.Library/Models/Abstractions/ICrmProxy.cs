using CrmDynamics.Library.Models.Crm;
using CrmDynamics.Library.Models.Extensions;
using CrmDynamics.Library.Models.Query.Requests;
using CrmDynamics.Library.Models.Query.Responses;
using System;
using System.Collections.Generic;

namespace CrmDynamics.Library.Models.Abstractions
{
    public interface ICrmProxy
    {
        Guid Create(Entity entity);
        void Update(Entity entity);
        void Delete(string entityName, Guid id);
        ExecuteTransactionResponse ExecuteTransaction(ExecuteTransactionRequest request);
        List<Entity> Fetch(string fetchXml);
        WhoAmI WhoAmI();
        Dictionary<int, string> OptionsMetadata(OptionSetMetadataRequest request);
        void ExecuteWorkflow(Guid workflowId, Guid entityId);
        Entity Retrieve(string entityName, Guid entityId, params string[] columns);
        void SetState(string entityName, Guid id, int statuscode, int statecode);
    }
}
