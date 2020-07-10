using System;
using System.Collections.Generic;

namespace CrmDynamics.Library.Models.Abstractions
{
    public interface ICrmCache
    {
        T GetValueUpdateCache<T>(string url, string method, DateTimeOffset? timeOffset = null);
        object GetValueUpdateCache(string url, string method, DateTimeOffset? timeOffset = null);
        string GetCorrectTargetEntityName(IList<string> targets, Guid id);
        string GetRelationshipNavigationPropertyName(string relationshipSchemaNameб, string entityName);
        string GetRelationshipNavigationPropertyName(Guid metadataIdб, string entityName);
        string GetEntityDefinitionSchemaName(string entityLogicalName);
        string GetReferencedEntityNavigationPropertyName(string entityName, string referencingAttributeKey);
        Dictionary<int, string> GetPicklistOptions(string entityName, string attributeKey);
    }
}
