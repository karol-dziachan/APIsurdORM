using MediatR;
using System.Linq.Expressions;

namespace __ProjectName__.Application.Features.__Entity__.Queries.Count__Entities__
{
    public class Count__Entities__Query : IRequest<QueryResult>
    {
        public Expression<Func<Domain.Entities.__Entity__, bool>> Predicate { get; }

        public Count__Entities__Query(Expression<Func<Domain.Entities.__Entity__, bool>> predicate)
        {
            Predicate = predicate;
        }
    }
}
