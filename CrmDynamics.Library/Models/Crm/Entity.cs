using System;
using System.Collections.Generic;
using System.Linq;

namespace CrmDynamics.Library.Models.Crm
{
    public class Entity
    {
        public string LogicalName { get; set; }

        public Guid Id { get; set; }

        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        public object this[string attributeName]
        {
            get => Attributes[attributeName];
            set => Attributes[attributeName] = value;
        }

        public Entity() { }

        public Entity(string entityName)
        {
            LogicalName = entityName;
        }

        public Entity(string entityName, Guid id)
        {
            LogicalName = entityName;
            Id = id;
        }

        public bool Contains(string attributeName)
        {
            return Attributes.ContainsKey(attributeName);
        }

        public T GetAttributeValue<T>(string attributeLogicalName)
        {
            if (!Contains(attributeLogicalName))
                return default;

            var value = Attributes.FirstOrDefault(attribute => attribute.Key == attributeLogicalName).Value;

            if (typeof(T) == value.GetType() || typeof(T) == typeof(object))
                return (T)value;

            throw new Exception("Unknown type of attribute");
        }
    }
}
