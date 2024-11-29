namespace Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations.Deprecated
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ManyToOneAttribute : Attribute
    {
        public Type RelatedEntityType { get; }

        public ManyToOneAttribute(Type relatedEntityType)
        {
            RelatedEntityType = relatedEntityType;
        }
    }
}
