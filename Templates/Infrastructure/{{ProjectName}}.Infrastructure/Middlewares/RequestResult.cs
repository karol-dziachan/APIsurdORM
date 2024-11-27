using __ProjectName__.Application.Common.Abstractions;

namespace __ProjectName__.Infrastructure.Middlewares
{
    public class RequestResult : BaseResult
    {
        public RequestResult(bool success, string message) : base(success, message) { }
    }
}
