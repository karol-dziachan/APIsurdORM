using __ProjectName__.Application.Common.Abstractions;

namespace __ProjectName__.Application.Features.__Entity__.Queries.Get__Entities__ByParameter
{
    public sealed class QueryResult : BaseResult
    {
        public IEnumerable<Domain.Entities.__Entity__> Data { get; set; }

        public QueryResult(bool success, string message, IEnumerable<Domain.Entities.__Entity__> data) : base(success, message)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
