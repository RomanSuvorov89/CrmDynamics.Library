using CrmDynamics.Library.Workers.Cache.Models.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CrmDynamics.Library.Workers.Cache.Models
{
    public class EntityAttribute
    {
        [DataMember(Name = "@odata.context")]
        public string Context { get; set; }
        [DataMember(Name = "@odata.type")]
        public string Type { get; set; }
        public IList<string> Targets { get; set; }
        public object AttributeOf { get; set; }
        public string AttributeType { get; set; }
        public PropertyValue AttributeTypeName { get; set; }
        public int ColumnNumber { get; set; }
        public LocalizedLabelsProperty Description { get; set; }
        public LocalizedLabelsProperty DisplayName { get; set; }
        public object DeprecatedVersion { get; set; }
        public string IntroducedVersion { get; set; }
        public string EntityLogicalName { get; set; }
        public CanBeChangedProperty IsAuditEnabled { get; set; }
        public bool IsCustomAttribute { get; set; }
        public bool IsPrimaryId { get; set; }
        public bool IsPrimaryName { get; set; }
        public bool IsValidForCreate { get; set; }
        public bool IsValidForRead { get; set; }
        public bool IsValidForUpdate { get; set; }
        public bool CanBeSecuredForRead { get; set; }
        public bool CanBeSecuredForCreate { get; set; }
        public bool CanBeSecuredForUpdate { get; set; }
        public bool IsSecured { get; set; }
        public bool IsRetrievable { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsSearchable { get; set; }
        public bool IsManaged { get; set; }
        public CanBeChangedProperty IsGlobalFilterEnabled { get; set; }
        public CanBeChangedProperty IsSortableEnabled { get; set; }
        public object LinkedAttributeId { get; set; }
        public string LogicalName { get; set; }
        public CanBeChangedProperty IsCustomizable { get; set; }
        public CanBeChangedProperty IsRenameable { get; set; }
        public CanBeChangedProperty IsValidForAdvancedFind { get; set; }
        public RequiredLevel RequiredLevel { get; set; }
        public CanBeChangedProperty CanModifyAdditionalSettings { get; set; }
        public string SchemaName { get; set; }
        public bool IsLogical { get; set; }
        public object InheritsFrom { get; set; }
        public object SourceType { get; set; }
        public string MetadataId { get; set; }
        public object HasChanged { get; set; }
    }
}
