using __ProjectName__.Persistence.Abstractions;
using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Queries.Find__Entities__
{
    public class Find__Entities__QueryHandler : IRequestHandler<Find__Entities__Query, QueryResult>
    {
        private readonly I__Entity__Repository _repository;

        public Find__Entities__QueryHandler(I__Entity__Repository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult> Handle(Find__Entities__Query request, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAsync(request.Predicate);

            if (entities == null || !entities.Any())
            {
                return new QueryResult(false, "No entities found.", null);
            }

            return new QueryResult(true, "Entities retrieved successfully.", entities);
        }
    }
}
