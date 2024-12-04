using Pr0t0k07.APIsurdORM.Infrastructure.Shared.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr0t0k07.APIsurdORM.Examples.Entities
{
    [Entity(nameof(TestCompositeEntity))]
    [PluralNameEntity("CompositeEntities")]
    [ManyToMany]
    public class TestCompositeEntity
    {
        [RequiredProperty]
        [SqlType("UNIQUEIDENTIFIER")]
        [PrimaryKey]
        public Guid TestEntity_Id { get; set; }

        [RequiredProperty]
        [SqlType("UNIQUEIDENTIFIER")]
        [PrimaryKey]
        public Guid RelatedEntity_Id { get; set; }
    }
}
