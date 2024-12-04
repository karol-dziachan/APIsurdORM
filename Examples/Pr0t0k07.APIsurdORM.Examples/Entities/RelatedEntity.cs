using Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations;

namespace Pr0t0k07.APIsurdORM.Examples.Entities
{
    [Entity("RelatedEntity")]
    [PluralNameEntity("RelatedEntities")]
    public class RelatedEntity
    {
        [PrimaryKey]
        [DefaultValue("default NEWID()")]

        [SqlType("UNIQUEIDENTIFIER")]
        public Guid Id { get; set; }

        [SqlType("NVARCHAR(50)")]
        public string VeryImportantProperty { get; set; }
    }
}
