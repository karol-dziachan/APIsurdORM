using MediatR;
using System.Linq.Expressions;

namespace __ProjectName__.Application.Features.__Entity__.Queries.Find__Entites__
{
    public class Find__Entities__Query : IRequest<QueryResult>
    {
        public Expression<Func<Domain.Entities.__Entity__, bool>> Predicate { get; }

        public Find__Entities__Query(Expression<Func<Domain.Entities.__Entity__, bool>> predicate)
        {
            Predicate = predicate;
        }

        /*
                 // Pomocnicza metoda do konwersji filtra na wyrażenie predykatu
        private Expression<Func<Domain.Entities.__Entity__, bool>> BuildPredicate(string filter)
        {
            // Przykład: filtr w postaci "Name=Test" konwertowany na Expression
            var parts = filter.Split('=');
            var property = parts[0];
            var value = parts[1];

            var parameter = Expression.Parameter(typeof(Domain.Entities.__Entity__), "e");
            var member = Expression.Property(parameter, property);
            var constant = Expression.Constant(value);
            var equals = Expression.Equal(member, constant);

            return Expression.Lambda<Func<Domain.Entities.__Entity__, bool>>(equals, parameter);
        } 
         */
    }
}
