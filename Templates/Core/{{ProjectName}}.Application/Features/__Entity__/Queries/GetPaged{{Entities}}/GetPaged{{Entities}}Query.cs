using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Queries.GetPaged__Entities__
{
    public class GetPaged__Entities__Query : IRequest<QueryResult>
    {
        public int PageIndex { get; }
        public int PageSize { get; }

        public GetPaged__Entities__Query(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}

