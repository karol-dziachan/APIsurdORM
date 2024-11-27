using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Update__Entity__
{
    public class Update__Entity__Command : IRequest<CommandResult>
    {
        public Guid Id {  get; set; }
        public Domain.Entities.__Entity__ Entity { get; set; }

        public Update__Entity__Command(Domain.Entities.__Entity__ entity, Guid id)
        {
            Entity = entity;
            Id = id;
        }
    }
}
