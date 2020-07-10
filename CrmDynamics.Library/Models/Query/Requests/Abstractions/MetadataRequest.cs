namespace CrmDynamics.Library.Models.Query.Requests.Abstractions
{
    public abstract class MetadataRequest
    {
        public string EntityName { get; }
        public string AttributeKey { get; }
        protected MetadataRequest(string entityName, string attributeKey)
        {
            EntityName = entityName;
            AttributeKey = attributeKey;
        }
    }
}
