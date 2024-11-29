using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Remove__Entities__ByParameters
{
    public class Remove__Entities__ByParametersCommand : IRequest<CommandResult>
    {
        public Dictionary<string, string> Parameters { get; set; }

        public Remove__Entities__ByParametersCommand(Dictionary<string, string> parameters)
        {
            Parameters = parameters;
        }
    }
}
