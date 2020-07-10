using CrmDynamics.Library.Workers.Cache.Models.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CrmDynamics.Library.Workers.Cache.Models
{
    public class EntityDefinitionsAttributes
    {
        [DataMember(Name = "@odata.context")]
        public string Context { get; set; }
        [DataMember(Name = "value")]
        public IList<Attribute> Attributes { get; set; }
    }

    public class Attribute
    {
        [DataMember(Name = "@odata.type")]
        public string Type { get; set; }
        public string Format { get; set; }
        public PropertyValue FormatName { get; set; }
        public string ImeMode { get; set; }
        public int MaxLength { get; set; }
        public string YomiOf { get; set; }
        public bool IsLocalizable { get; set; }
        public int DatabaseLength { get; set; }
        public string FormulaDefinition { get; set; }
        public int SourceTypeMask { get; set; }
        public string AttributeOf { get; set; }
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
        public int? SourceType { get; set; }
        public string MetadataId { get; set; }
        public object HasChanged { get; set; }
        public IList<string> Targets { get; set; }
        public bool? DefaultValue { get; set; }
        public int? DefaultFormValue { get; set; }
        public double? MaxValue { get; set; }
        public double? MinValue { get; set; }
        public int? Precision { get; set; }
        public DateTime? MinSupportedValue { get; set; }
        public DateTime? MaxSupportedValue { get; set; }
        public PropertyValue DateTimeBehavior { get; set; }
        public CanBeChangedProperty CanChangeDateTimeBehavior { get; set; }
        public int? PrecisionSource { get; set; }
        public string CalculationOf { get; set; }
        public bool? IsBaseCurrency { get; set; }
        public bool? IsPrimaryImage { get; set; }
        public int? MaxHeight { get; set; }
        public int? MaxWidth { get; set; }
    }
}
