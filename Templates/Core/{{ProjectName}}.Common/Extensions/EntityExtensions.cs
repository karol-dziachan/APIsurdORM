using System.Reflection;

namespace __ProjectName__.Common.Extensions
{
    public static class EntityExtensions
    {
        public static Dictionary<string, object> ToDictionary(this object entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var dictionary = new Dictionary<string, object>();

            var properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                dictionary.Add(property.Name, value);
            }

            return dictionary;
        }
    }
}
