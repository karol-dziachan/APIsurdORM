namespace __ProjectName__.Application.Features.__Entity__.Queries.Get__Entity__ById
{
    public class QueryResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Domain.Entities.__Entity__ Data { get; set; }

        public QueryResult(bool success, string message, Domain.Entities.__Entity__ data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
