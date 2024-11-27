using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Queries.Get__Entities__ByParameter
{
    public class Get__Entities__ByParameterQuery : IRequest<QueryResult>
    {
        public Dictionary<string, string> Parameters { get; set; }

        public Get__Entities__ByParameterQuery(Dictionary<string, string> parameters)
        {
            Parameters = parameters;
        }
    }
}
