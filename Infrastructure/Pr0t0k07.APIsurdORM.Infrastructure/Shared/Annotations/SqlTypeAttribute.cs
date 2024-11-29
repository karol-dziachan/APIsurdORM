namespace Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class SqlTypeAttribute : Attribute
    {
        public string TypeName { get; }

        public SqlTypeAttribute(string type)
        {
            TypeName = type;
        }
    }
}
