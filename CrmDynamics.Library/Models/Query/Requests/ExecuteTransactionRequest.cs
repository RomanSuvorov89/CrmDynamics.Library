using CrmDynamics.Library.Extensions;
using CrmDynamics.Library.Models.Query.Requests.Abstractions;
using System.Collections.Generic;

namespace CrmDynamics.Library.Models.Query.Requests
{
    public sealed class ExecuteTransactionRequest : OrganizationRequest
    {
        public List<object> Requests { get; } = new List<object>();

        public ExecuteTransactionRequest()
        {
            RequestName = Constants.EXECUTETRANSACTION;
        }

        public void AddRequest(OrganizationRequest request)
        {
            Requests.Add(request);
        }
    }
}
