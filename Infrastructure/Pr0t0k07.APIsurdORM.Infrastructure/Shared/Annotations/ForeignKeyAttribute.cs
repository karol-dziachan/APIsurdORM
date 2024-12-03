namespace Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ForeignKeyAttribute : Attribute
    {
        public string RelatedEntityType { get; }
        public string ForeignKey { get; }

        public ForeignKeyAttribute(string relatedEntityType, string foreignKey)
        {
            RelatedEntityType = relatedEntityType;
            ForeignKey = foreignKey;
        }
    }
}
