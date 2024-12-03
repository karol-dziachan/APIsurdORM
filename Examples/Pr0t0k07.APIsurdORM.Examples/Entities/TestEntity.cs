using Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations; 

namespace Pr0t0k07.APIsurdORM.Examples.Entities
{
    [Entity(nameof(TestEntity))]
    [PluralNameEntity("TestEntities")]
    public class TestEntity
    {
        [PrimaryKey]
        [Unique]
        [ColumnName("Identifier")]
        [RequiredProperty]
        [AutoNumerated]
        public Guid Id {  get; set; }

        [MaxLength(50)]
        [RequiredProperty]
        public string FirstName { get; set; }

        [SqlType("NVARCHAR")]
        [RequiredProperty]
        public string LastName { get; set; }

        [ForeignKey("RelatedEntity", "Id")]
        [RequiredProperty]
        public RelatedEntity RelatedEntityId { get; set; }

    }
}
