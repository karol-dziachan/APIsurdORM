using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Add__Entity__
{
    internal class Add__Entity__Command : IRequest<CommandResult>
    {
        public Domain.Entities.__Entity__ Entity { get; set; }

        public Add__Entity__Command(Domain.Entities.__Entity__ entity)
        {
            Entity = entity;
        }
    }
}
