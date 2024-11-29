using __ProjectName__.Application.Common.Abstractions;

namespace __ProjectName__.Application.Features.__Entity__.Queries.IsConcrete__Entity__Exist
{
    public sealed class QueryResult : BaseResult
    {
        public string Message { get; set; }
        public bool Data { get; set; }

        public QueryResult(bool success, string message, bool data) : base(success, message)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
