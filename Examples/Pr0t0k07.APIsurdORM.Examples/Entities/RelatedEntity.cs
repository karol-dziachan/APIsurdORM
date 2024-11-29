using Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations;

namespace Pr0t0k07.APIsurdORM.Examples.Entities
{
    [Entity("RelatedEntity")]
    public class RelatedEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string VeryImportantProperty { get; set; }
    }
}
