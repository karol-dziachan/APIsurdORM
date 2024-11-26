namespace __ProjectName__.Application.Features.__Entity__.Queries.GetAll__Entities__
{
    public class QueryResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<Domain.Entities.__Entity__> Data { get; set; }

        public QueryResult(bool success, string message, IEnumerable<Domain.Entities.__Entity__> data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
