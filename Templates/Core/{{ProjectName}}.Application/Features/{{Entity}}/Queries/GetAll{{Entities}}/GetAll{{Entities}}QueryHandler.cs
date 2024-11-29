using __ProjectName__.Application.Features.__Entity__.Queries.GetAll__Entity__;
using __ProjectName__.Persistence.Abstractions;
using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Queries.GetAll__Entities__
{
    public class GetAll__Entities__QueryHandler : IRequestHandler<GetAll__Entities__Query, QueryResult>
    {
        private readonly I__Entity__Repository _repository;

        public GetAll__Entities__QueryHandler(I__Entity__Repository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult> Handle(GetAll__Entities__Query request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();

            if (entities == null || !entities.Any())
            {
                return new QueryResult(false, "No entities found.", null);
            }

            return new QueryResult(true, "Entities retrieved successfully.", entities);

        }
    }
}
