using System.Collections.Generic;

namespace CrmDynamics.Library.Workers.Cache.Models.Common
{
    public class LocalizedLabelsProperty
    {
        public IList<LocalizedLabel> LocalizedLabels { get; set; }
        public UserLocalizedLabel UserLocalizedLabel { get; set; }
    }

    public class LocalizedLabel
    {
        public string Label { get; set; }
        public int LanguageCode { get; set; }
        public bool IsManaged { get; set; }
        public string MetadataId { get; set; }
        public object HasChanged { get; set; }
    }

    public class UserLocalizedLabel
    {
        public string Label { get; set; }
        public int LanguageCode { get; set; }
        public bool IsManaged { get; set; }
        public string MetadataId { get; set; }
        public object HasChanged { get; set; }
    }
}
