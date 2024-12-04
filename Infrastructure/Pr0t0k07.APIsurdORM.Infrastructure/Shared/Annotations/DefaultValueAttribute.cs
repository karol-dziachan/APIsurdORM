namespace Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class DefaultValueAttribute : Attribute
    {
        public string DefaultValue { get; set; }

        public DefaultValueAttribute(string defaultValue)
        {
            DefaultValue = defaultValue;   
        }
    }
}
