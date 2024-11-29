namespace Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class PrimaryKeyAttribute : Attribute
    {
    }

    /*   public static string GetPrimaryKeyName(Type entityType)
    {
        // Sprawdzamy wszystkie właściwości w typie encji
        var property = entityType.GetProperties()
            .FirstOrDefault(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null);

        // Jeśli znalazło właściwość z atrybutem PrimaryKey, zwrócimy jej nazwę
        return property?.Name ?? "No Primary Key Defined";
    }*/
}
