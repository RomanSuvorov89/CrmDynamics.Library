using CrmDynamics.Library.Extensions;
using CrmDynamics.Library.Models.Crm;
using CrmDynamics.Library.Models.Query.Requests.Abstractions;

namespace CrmDynamics.Library.Models.Query.Requests
{
    public class DeleteRequest : OrganizationRequest
    {
        public EntityReference Target { get; set; }

        public DeleteRequest()
        {
            RequestName = Constants.DELETE;
        }
    }
}
