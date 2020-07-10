using CrmDynamics.Library.Extensions;
using CrmDynamics.Library.Models.Crm;
using CrmDynamics.Library.Models.Query.Requests.Abstractions;

namespace CrmDynamics.Library.Models.Query.Requests
{
    public class AssociateRequest : OrganizationRequest
    {
        public EntityReference Target { get; }
        public string NavigationProperty { get; }
        public EntityReference OtherEntity { get; set; }

        /// <summary>
        /// Associate N-N entities by <see cref="navigationProperty"/> relationship navigation property.
        /// </summary>
        public AssociateRequest(EntityReference target, string navigationProperty, EntityReference otherEntity)
        {
            RequestName = Constants.ASSOCIATE;
            Target = target;
            OtherEntity = otherEntity;
            NavigationProperty = navigationProperty;
        }
    }
}
