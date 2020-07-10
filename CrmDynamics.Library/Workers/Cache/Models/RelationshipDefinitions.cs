using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CrmDynamics.Library.Workers.Cache.Models
{
    public class RelationshipDefinitions
    {
        [DataMember(Name = "@odata.context")]
        public string Context { get; set; }
        [DataMember(Name = "value")]
        public IList<Realationship> Realationships { get; set; }
    }

    public class Realationship
    {
        [DataMember(Name = "@odata.type")]
        public string Type { get; set; }
        public string SchemaName { get; set; }
        public Guid MetadataId { get; set; }
        public string ReferencedAttribute { get; set; }
        public string ReferencedEntity { get; set; }
        public string ReferencingAttribute { get; set; }
        public string ReferencingEntity { get; set; }
        public string ReferencedEntityNavigationPropertyName { get; set; }
    }
}
