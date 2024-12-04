using Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations; 

namespace Pr0t0k07.APIsurdORM.Examples.Entities
{
    [Entity(nameof(TestEntity))]
    [PluralNameEntity("TestEntities")]
    public class TestEntity
    {
        [PrimaryKey]
        [Unique]
        [RequiredProperty]
        [DefaultValue("default NEWID()")]
        [SqlType("UNIQUEIDENTIFIER")]
        public Guid Id {  get; set; }

        [MaxLength(50)]
        [RequiredProperty]
        [Unique]
        [SqlType("NVARCHAR(50)")]
        public string FirstName { get; set; }

        [SqlType("NVARCHAR(50)")]
        [RequiredProperty]
        public string LastName { get; set; }

        [ForeignKey("RelatedEntity", "Id")]
        [RequiredProperty]
        [SqlType("UNIQUEIDENTIFIER")]
        public RelatedEntity RelatedEntityId { get; set; }

    }
}
