using CrmDynamics.Library.Workers.Cache.Models.Common;
using System.Runtime.Serialization;

namespace CrmDynamics.Library.Workers.Cache.Models
{
    public class RelationshipDefinitionsMetadata
    {
        [DataMember(Name = "@odata.context")]
        public string Context { get; set; }
        [DataMember(Name = "@odata.type")]
        public string Type { get; set; }
        public AssociatedMenuConfiguration AssociatedMenuConfiguration { get; set; }
        public CascadeConfiguration CascadeConfiguration { get; set; }
        public string ReferencedAttribute { get; set; }
        public string ReferencedEntity { get; set; }
        public string ReferencingAttribute { get; set; }
        public string ReferencingEntity { get; set; }
        public bool IsHierarchical { get; set; }
        public string ReferencedEntityNavigationPropertyName { get; set; }
        public string ReferencingEntityNavigationPropertyName { get; set; }
        public bool IsCustomRelationship { get; set; }
        public CanBeChangedProperty IsCustomizable { get; set; }
        public bool IsValidForAdvancedFind { get; set; }
        public string SchemaName { get; set; }
        public string SecurityTypes { get; set; }
        public bool IsManaged { get; set; }
        public string RelationshipType { get; set; }
        public string IntroducedVersion { get; set; }
        public string MetadataId { get; set; }
        public object HasChanged { get; set; }
    }
}
