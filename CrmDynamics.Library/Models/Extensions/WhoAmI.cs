using System;

namespace CrmDynamics.Library.Models.Extensions
{
    public class WhoAmI
    {
        public Guid BusinessUnitId { get; set; }
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
