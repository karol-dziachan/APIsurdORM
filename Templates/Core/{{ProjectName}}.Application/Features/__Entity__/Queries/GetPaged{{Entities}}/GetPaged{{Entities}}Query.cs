using MediatR;

namespace __ProjectName__.Application.Features.__Entity__.Queries.GetPaged__Entities__
{
    public class GetPaged__Entities__Query : IRequest<QueryResult>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public GetPaged__Entities__Query(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}

