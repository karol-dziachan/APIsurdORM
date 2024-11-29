using __ProjectName__.Persistence.Abstractions;
using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Remove__Entity__
{
    public class Remove__Entity__CommandHandler : IRequestHandler<Remove__Entity__Command, CommandResult>
    {
        private readonly I__Entity__Repository _repository;

        public Remove__Entity__CommandHandler(I__Entity__Repository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResult> Handle(Remove__Entity__Command request, CancellationToken cancellationToken)
        {
            var result = await _repository.RemoveAsync(request.Id);
            if (result != Guid.Empty)
            {
                return new CommandResult(true, "Entity removed successfully.", result);
            }

            return new CommandResult(false, "Entity removal failed.", result);
        }
    }
}
