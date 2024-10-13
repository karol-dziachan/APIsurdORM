using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Pr0t0k07.APIsurdORM.Templates.Interfaces;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace Pr0t0k07.APIsurdORM.Templates.Logic
{
    internal class GenericCrudRepository<T> : IGenericCrudRepository<T> where T : class
    {
        private readonly ILogger<IGenericCrudRepository<T>> _logger;
        private readonly string _connectionString;
        private readonly string _schemaName; 

        public GenericCrudRepository(ILogger<IGenericCrudRepository<T>> logger, string connectionString, string schemaName)
        {
            _logger = logger;
            _connectionString = connectionString;
            _schemaName = schemaName;
        }

        public async Task<int> Add(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = GenerateInsertCommand(entity);
                var cratedId = await command.ExecuteNonQueryAsync();
                return cratedId;
            }
        }

        public async Task<int> Count(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = $"DELETE FROM {_schemaName}.{typeof(T).Name} WHERE [Id] = @Id";
                command.Parameters.AddWithValue("@Id", id);
                await command.ExecuteNonQueryAsync();
                return id;
            }
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<T>> GetPaged(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(T entity, int id)
        {
            throw new NotImplementedException();
        }

        private string GenerateInsertCommand(T entity)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var tableName = typeof(T).Name;

            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var parameterNames = string.Join(", ", properties.Select(p => "@" + p.Name));

            var commandText = $"INSERT OUTPUT INSERTED.ID INTO {_schemaName}.{tableName} ({columnNames}) VALUES ({parameterNames})";

            return commandText;
        }

        private void AddParameters(SqlCommand command, T entity)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var parameterName = "@" + property.Name;
                var parameterValue = property.GetValue(entity) ?? DBNull.Value;

                command.Parameters.AddWithValue(parameterName, parameterValue);
            }
        }
    }
}
