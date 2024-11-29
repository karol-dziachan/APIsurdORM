using __ProjectName__.Persistence.Abstractions;
using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Queries.Count__Entities__
{
    public class Count__Entities__QueryHandler : IRequestHandler<Count__Entities__Query, QueryResult>
    {
        private readonly I__Entity__Repository _repository;

        public Count__Entities__QueryHandler(I__Entity__Repository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult> Handle(Count__Entities__Query request, CancellationToken cancellationToken)
        {
            var entities = await _repository.CountAsync(request.Predicate);



            return new QueryResult(true, "Entities retrieved successfully.", entities);
        }
    }
}
