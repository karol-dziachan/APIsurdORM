namespace Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations.Deprecated
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class OneToManyAttribute : Attribute
    {
        public Type RelatedEntityType { get; }
        public string ForeignKeyProperty { get; }

        public OneToManyAttribute(Type relatedEntityType, string foreignKeyProperty)
        {
            RelatedEntityType = relatedEntityType;
            ForeignKeyProperty = foreignKeyProperty;
        }
    }
}
