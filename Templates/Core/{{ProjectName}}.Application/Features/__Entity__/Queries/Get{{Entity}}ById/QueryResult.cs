using __ProjectName__.Application.Common.Abstractions;

namespace __ProjectName__.Application.Features.__Entity__.Queries.Get__Entity__ById
{
    public sealed class QueryResult : BaseResult
    {
        public Domain.Entities.__Entity__ Data { get; set; }

        public QueryResult(bool success, string message, Domain.Entities.__Entity__ data) : base(success, message)
        { 
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
