using CrmDynamics.Library.Models.Query.Requests.Abstractions;

namespace CrmDynamics.Library.Models.Query.Requests
{
    public class OptionSetMetadataRequest : MetadataRequest
    {
        public OptionSetMetadataRequest(string entityName, string attributeKey) : base(entityName, attributeKey) { }
    }
}
