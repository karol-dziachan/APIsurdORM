using __ProjectName__.Persistence.Abstractions;
using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Add__Entity__
{
    public class Add__Entity__CommandHandler : IRequestHandler<Add__Entity__Command, CommandResult>
    {
        private readonly I__Entity__Repository _repository;

        public Add__Entity__CommandHandler(I__Entity__Repository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResult> Handle(Add__Entity__Command request, CancellationToken cancellationToken)
        {
            var newEntityId = await _repository.AddAsync(request.Entity);

            if (newEntityId == Guid.Empty)
            {
                return new CommandResult(false, "Failed to add entity.", null);
            }

            return new CommandResult(true, "Entity added successfully.", newEntityId);
        }
    }
}
