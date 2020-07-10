using CrmDynamics.Library.Extensions;
using System.Collections.Generic;

namespace CrmDynamics.Library.Models.Query.Responses
{
    public class ExecuteTransactionResponse : OrganizationResponse
    {
        public IList<OrganizationResponse> Responses { get; } = new List<OrganizationResponse>();

        public ExecuteTransactionResponse()
        {
            ResponseName = Constants.EXECUTETRANSACTION;
        }
    }
}
