using __ProjectName__.Persistence.Abstractions;
using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Queries.IsConcrete__Entity__Exist
{
    public class IsConcrete__Entity__ExistQueryHandler : IRequestHandler<IsConcrete__Entity__ExistQuery, QueryResult>
    {
        private readonly I__Entity__Repository _repository;

        public IsConcrete__Entity__ExistQueryHandler(I__Entity__Repository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult> Handle(IsConcrete__Entity__ExistQuery request, CancellationToken cancellationToken)
        {
            var isExist = await _repository.Exists(request.Predicate);

            return new QueryResult(true, "Entities retrieved successfully.", isExist);
        }
    }
}
