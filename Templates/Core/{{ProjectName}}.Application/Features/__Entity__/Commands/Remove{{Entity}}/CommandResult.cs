using __ProjectName__.Application.Common.Abstractions;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Remove__Entity__
{
    internal sealed class CommandResult : BaseResult
    {
        public Guid DeletedId {  get; set; }

        public CommandResult(bool success, string message, Guid deletedId) : base(success, message)
        {
            DeletedId = deletedId;
        }
    }
}
