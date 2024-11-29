using System;
using System.Reflection;
using Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations.Deprecated;

namespace __ProjectName__.Common.Extensions
{
    public static class EntityExtensions
    {
        public static Dictionary<string, object> ToDictionary(this object entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var propertiesDict = new Dictionary<string, object>();

            var properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var attributeOTM = property.GetCustomAttribute<OneToManyAttribute>();
                
                if (attributeOTM is not null)
                {
                    continue;
                }

                var attributeOTO = property.GetCustomAttribute<OneToOneAttribute>();

                var value = property.GetValue(entity);

                if (attributeOTO is not null)
                {
                    propertiesDict.Add(attributeOTO.ForeignKeyProperty, value);
                    continue;
                }

                propertiesDict.Add(property.Name, value);
            }

            return propertiesDict;
        }
    }
}
