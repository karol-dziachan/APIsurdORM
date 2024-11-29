namespace Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class EntityAttribute : Attribute
    {
        public string EntityName { get; }

        public EntityAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }

    /*
     * 
     * var assembly = Assembly.GetExecutingAssembly();
var types = assembly.GetTypes();

foreach (var type in types)
{
    var attribute = type.GetCustomAttribute<EntityAttribute>();
    if (attribute != null)
    {
        Console.WriteLine($"Class {type.Name} is marked as entity with name: {attribute.EntityName}");
    }
}
     * 
     * 
     */
}
