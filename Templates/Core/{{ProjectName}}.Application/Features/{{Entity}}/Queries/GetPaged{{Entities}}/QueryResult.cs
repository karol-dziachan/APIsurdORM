using __ProjectName__.Application.Common.Abstractions;

namespace __ProjectName__.Application.Features.__Entity__.Queries.GetPaged__Entities__
{
    public sealed class QueryResult : BaseResult
    {
        public IEnumerable<Domain.Entities.__Entity__> Data { get; set; }

        public QueryResult(bool success, string message, IEnumerable<Domain.Entities.__Entity__> data) : base(success, message)
        {
            Data = data;
        }
    }
}
