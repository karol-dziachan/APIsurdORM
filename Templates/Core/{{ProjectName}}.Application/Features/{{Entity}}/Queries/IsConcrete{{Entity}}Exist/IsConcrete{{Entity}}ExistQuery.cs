using MediatR;
using System.Linq.Expressions;

namespace __ProjectName__.Application.Features.__Entity__.Queries.IsConcrete__Entity__Exist
{
    public class IsConcrete__Entity__ExistQuery : IRequest<QueryResult>
    {
        public Expression<Func<Domain.Entities.__Entity__, bool>> Predicate { get; set; }

        public IsConcrete__Entity__ExistQuery(Expression<Func<Domain.Entities.__Entity__, bool>> predicate)
        {
            Predicate = predicate;
        }
    }
}
