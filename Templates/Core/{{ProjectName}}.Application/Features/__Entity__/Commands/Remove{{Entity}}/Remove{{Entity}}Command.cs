using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Remove__Entity__
{
    public class Remove__Entity__Command : IRequest<CommandResult>
    {
        public Guid Id { get; set; }

        public Remove__Entity__Command(Guid id)
        {
            Id = id;
        }
    }
}
