using CrmDynamics.Library.Extensions;
using CrmDynamics.Library.Models.Crm;
using CrmDynamics.Library.Models.Query.Requests.Abstractions;

namespace CrmDynamics.Library.Models.Query.Requests
{
    public class CreateRequest : OrganizationRequest
    {
        public Entity Target { get; set; }

        public CreateRequest()
        {
            RequestName = Constants.CREATE;
        }
    }
}
