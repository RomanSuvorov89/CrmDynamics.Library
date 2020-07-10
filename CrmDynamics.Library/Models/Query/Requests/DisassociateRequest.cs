using CrmDynamics.Library.Extensions;
using CrmDynamics.Library.Models.Crm;
using CrmDynamics.Library.Models.Query.Requests.Abstractions;
using System;

namespace CrmDynamics.Library.Models.Query.Requests
{
    public class DisassociateRequest : OrganizationRequest
    {
        public EntityReference Target { get; }
        public string NavigationProperty { get; }
        public Guid OtherEntityId { get; set; }
        public bool IsCollectValueReference => OtherEntityId != Guid.Empty;

        /// <summary>
        /// Diassociate N-N entities by <see cref="relationshipName"/> class.
        /// </summary>
        public DisassociateRequest(EntityReference target, string relationshipName, Guid otherEntityId)
        {
            RequestName = Constants.DISASSOCIATE;
            Target = target;
            NavigationProperty = relationshipName;
            OtherEntityId = otherEntityId;
        }

        /// <summary>
        /// Diassociate 1-N entities by <see cref="attributeKey"/> attribute.
        /// </summary>
        public DisassociateRequest(EntityReference target, string attributeKey)
        {
            RequestName = Constants.DISASSOCIATE;
            Target = target;
            NavigationProperty = attributeKey;
        }
    }
}
