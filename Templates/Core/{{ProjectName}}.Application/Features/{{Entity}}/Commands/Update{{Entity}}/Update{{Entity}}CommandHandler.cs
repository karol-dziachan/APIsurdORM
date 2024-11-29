using __ProjectName__.Persistence.Abstractions;
using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Update__Entity__
{
    public class Update__Entity__CommandHandler : IRequestHandler<Update__Entity__Command, CommandResult>
    {
        private readonly I__Entity__Repository _repository;

        public Update__Entity__CommandHandler(I__Entity__Repository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResult> Handle(Update__Entity__Command request, CancellationToken cancellationToken)
        {
            var result = await _repository.UpdateAsync(request.Entity, request.Id);
            if (result > 0)
            {
                return new CommandResult(true, "Entity updated successfully.", null);
            }

            return new CommandResult(false, "Entity update failed.", result);
        }
    }
}
