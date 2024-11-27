using __ProjectName__.Application.Common.Abstractions;

namespace __ProjectName__.Application.Features.__Entity__.Queries.Count__Entities__
{
    internal sealed class QueryResult : BaseResult
    {
        public int Data { get; set; }

        public QueryResult(bool success, string message, int data) : base(success, message)
        {
            Data = data;
        }
    }
}
