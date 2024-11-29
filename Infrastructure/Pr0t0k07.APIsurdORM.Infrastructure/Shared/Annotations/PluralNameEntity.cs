namespace Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class PluralNameEntity : Attribute
    {
        public string Name { get; }

        public PluralNameEntity(string name)
        {
            Name = name;
        }
    }
}
