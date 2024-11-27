using __ProjectName__.Persistence.Abstractions;
using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Remove__Entities__ByParameters
{
    public class Remove__Entities__ByParametersCommandHandler : IRequestHandler<Remove__Entities__ByParametersCommand, CommandResult>
    {
        private readonly I__Entity__Repository _repository;

        public Remove__Entities__ByParametersCommandHandler(I__Entity__Repository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResult> Handle(Remove__Entities__ByParametersCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.RemoveByParametersAsync(request.Parameters);
            if (result > 0)
            {
                return new CommandResult(true, "Entities removed successfully.", result);
            }

            return new CommandResult(false, "Entities removal failed.", result);
        }
    }
}
