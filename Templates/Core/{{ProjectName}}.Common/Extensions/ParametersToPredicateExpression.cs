using System.Linq.Expressions;

namespace __ProjectName__.Common.Extensions
{
    public static class ParametersToPredicateExpression
    {
        public static Expression<Func<T, bool>> BuildPredicate<T>(this string filter)
        {
            var parts = filter.Split('=');
            var property = parts[0];
            var value = parts[1];

            var parameter = Expression.Parameter(typeof(T), "e");
            var member = Expression.Property(parameter, property);
            var constant = Expression.Constant(value);
            var equals = Expression.Equal(member, constant);

            return Expression.Lambda<Func<T, bool>>(equals, parameter);
        }
    }
}
