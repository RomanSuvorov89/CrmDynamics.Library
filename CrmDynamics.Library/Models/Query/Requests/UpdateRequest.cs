using CrmDynamics.Library.Extensions;
using CrmDynamics.Library.Models.Crm;
using CrmDynamics.Library.Models.Query.Requests.Abstractions;

namespace CrmDynamics.Library.Models.Query.Requests
{
    public class UpdateRequest : OrganizationRequest
    {
        public Entity Target { get; set; }

        public UpdateRequest()
        {
            RequestName = Constants.UPDATE;
        }
    }
}
