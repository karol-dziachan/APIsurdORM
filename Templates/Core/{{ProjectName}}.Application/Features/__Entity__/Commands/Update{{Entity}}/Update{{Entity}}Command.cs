using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Update__Entity__
{
    internal class Update__Entity__Command : IRequest<CommandResult>
    {
        public Domain.Entities.__Entity__ Entity { get; set; }

        public Update__Entity__Command(Domain.Entities.__Entity__ entity)
        {
            Entity = entity;
        }
    }
}
