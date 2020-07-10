using CrmDynamics.Library.Workers.Cache.Models.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CrmDynamics.Library.Workers.Cache.Models
{
    public class PicklistAttributeMetadata
    {
        [DataMember(Name = "@odata.context")]
        public string Context { get; set; }
        [DataMember(Name = "OptionSet@odata.context")]
        public string OptionSetContext { get; set; }
        public string LogicalName { get; set; }
        public string MetadataId { get; set; }
        public OptionSet OptionSet { get; set; }
        public GlobalOptionSet GlobalOptionSet { get; set; }
    }

    public class Option
    {
        public int Value { get; set; }
        public LocalizedLabelsProperty Label { get; set; }
        public LocalizedLabelsProperty Description { get; set; }
        public object Color { get; set; }
        public bool IsManaged { get; set; }
        public Guid? MetadataId { get; set; }
        public object HasChanged { get; set; }
    }

    public class OptionSet
    {
        public IList<Option> Options { get; set; }
        public string MetadataId { get; set; }
    }

    public class GlobalOptionSet
    {
        [DataMember(Name = "@odata.type")]
        public string Type { get; set; }
        public IList<Option> Options { get; set; }
        public string MetadataId { get; set; }
    }
}
