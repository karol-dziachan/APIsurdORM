using __ProjectName__.Application.Common.Abstractions;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Add__Entity__
{
    public sealed class CommandResult : BaseResult
    {
        public Guid? NewEntityId { get; set; }

        public CommandResult(bool success, string message, Guid? newEntityId) : base(success, message)
        {
            NewEntityId = newEntityId;
        }
    }
}
