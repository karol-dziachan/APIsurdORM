using __ProjectName__.Persistence.Abstractions;
using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Queries.Get__Entity__ById
{
    public class Get__Entity__ByIdQueryHandler : IRequestHandler<Get__Entity__ByIdQuery, QueryResult>
    {
        private readonly I__Entity__Repository _repository;

        public Get__Entity__ByIdQueryHandler(I__Entity__Repository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult> Handle(Get__Entity__ByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity == null)
            {
                return new QueryResult(false, "Entity not found.", null);
            }

            return new QueryResult(true, "Entity retrieved successfully.", entity);
        }
    }
}
