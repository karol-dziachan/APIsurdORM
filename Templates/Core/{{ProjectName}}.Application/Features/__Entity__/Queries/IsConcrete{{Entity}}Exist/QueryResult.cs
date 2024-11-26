namespace __ProjectName__.Application.Features.__Entity__.Queries.IsConcrete__Entity__Exist
{
    public class QueryResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool Data { get; set; }

        public QueryResult(bool success, string message, bool data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
