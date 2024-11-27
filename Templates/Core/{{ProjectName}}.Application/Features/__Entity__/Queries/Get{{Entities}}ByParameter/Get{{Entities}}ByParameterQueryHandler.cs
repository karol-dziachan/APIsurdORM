using __ProjectName__.Persistence.Abstractions;
using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Queries.Get__Entities__ByParameter
{
    public class Get__Entities__ByParameterQueryHandler : IRequestHandler<Get__Entities__ByParameterQuery, QueryResult>
    {
        private readonly I__Entity__Repository _repository;

        public Get__Entities__ByParameterQueryHandler(I__Entity__Repository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult> Handle(Get__Entities__ByParameterQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetByParametersAsync(request.Parameters);

            if (entities == null || !entities.Any())
            {
                return new QueryResult(false, "No entities found.", null);
            }

            return new QueryResult(true, "Entities retrieved successfully.", entities);
        }
    }
}
