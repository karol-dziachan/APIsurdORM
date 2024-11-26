using Dapper;
using System.Linq.Expressions;

namespace __ProjectName__.Infrastructure.Extensions
{
    public static class PredicateToWhereClause
    {
        public static string BuildWhereClause<T>(Expression<Func<T, bool>> predicate, DynamicParameters parameters)
        {
            var lambda = predicate.Body as BinaryExpression;

            if (lambda == null)
            {
                throw new InvalidOperationException("The predicate must be a binary expression.");
            }

            var left = lambda.Left as MemberExpression;
            var right = lambda.Right as ConstantExpression;

            if (left == null || right == null)
            {
                throw new InvalidOperationException("Unsupported expression in predicate.");
            }

            var columnName = left.Member.Name;

            var value = right.Value;

            parameters.Add($"@{columnName}", value);

            return $"{columnName} = @{columnName}";
        }
    }
}
