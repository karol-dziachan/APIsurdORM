using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Queries.Get__Entity__ById
{
    public class Get__Entity__ByIdQuery : IRequest<QueryResult>
    {
        public Guid Id { get; }

        public Get__Entity__ByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
