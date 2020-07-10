using System;

namespace CrmDynamics.Library.Models.Crm
{
    public class EntityReference
    {
        public Guid Id { get; set; }

        public string LogicalName { get; set; }

        public EntityReference(string logicalName, Guid id)
        {
            LogicalName = logicalName;
            Id = id;
        }
    }
}
