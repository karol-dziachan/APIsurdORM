using __ProjectName__.Application.Common.Interfaces;

namespace __ProjectName__.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
