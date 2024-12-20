﻿using __ProjectName__.Domain.Entities;
using __ProjectName__.Common.Extensions;
using __ProjectName__.Persistence.Abstractions;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using __ProjectName__.Common.Exceptions;

namespace __ProjectName__.Persistence.Repositories
{
    internal class __Entity__Repository : I__Entity__Repository
    {
        private readonly string _connectionString;

        public __Entity__Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Guid> AddAsync(__Entity__ entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @$"
                INSERT INTO [dbo].__Entity__ (__ENTITY_PARAMETERS__)
                OUTPUT INSERTED.ID 
                VALUES (__ENTITY_VALUES__);";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var parameters = new
                        {
                            //__ENTITY_PARAMETERS_MAPPING__
                        };
                        var insertedId = await connection.ExecuteScalarAsync<Guid>(sql, parameters, transaction: transaction);
                        transaction.Commit(); 
                        return insertedId;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback(); 
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<__Entity__>> GetAllAsync(int pageIndex, int pageSize)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"
                    SELECT * 
                    FROM [dbo].__Entity__
                    __LEFT_JOIN__
                    ORDER BY Id 
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;";

                var offset = (pageIndex - 1) * pageSize;
                var parameters = new { Offset = offset, PageSize = pageSize };
                var entities = await connection.QueryAsync<__Entity__>(sql, parameters);
                return entities;
            }
        }

        public async Task<int> CountAsync(Expression<Func<__Entity__, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var parameters = new DynamicParameters();

            var whereClause = PredicateToWhereClause.BuildWhereClause(predicate, parameters);

            var sql = $@"
                        SELECT COUNT(1)
                        FROM [dbo].__Entity__
                        WHERE {whereClause}";

            using (var connection = new SqlConnection(_connectionString))
            {
                var count = await connection.ExecuteScalarAsync<int>(sql, parameters);
                return count;
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<__Entity__, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var parameters = new DynamicParameters();

            var whereClause = PredicateToWhereClause.BuildWhereClause(predicate, parameters);

            var sql = $@"
                        SELECT CASE WHEN EXISTS (SELECT 1 FROM [dbo].__Entity__ WHERE {whereClause}) THEN 1 ELSE 0 END";

            using (var connection = new SqlConnection(_connectionString))
            {
                var exists = await connection.ExecuteScalarAsync<int>(sql, parameters);
                return exists == 1;
            }
        }

        public async Task<IEnumerable<__Entity__>> FindAsync(Expression<Func<__Entity__, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var parameters = new DynamicParameters();

            var whereClause = PredicateToWhereClause.BuildWhereClause(predicate, parameters);

            var sql = $@"
                        SELECT * FROM [dbo].__Entity__
                        WHERE {whereClause}";

            using (var connection = new SqlConnection(_connectionString))
            {
                var entities = await connection.QueryAsync<__Entity__>(sql, parameters);
                return entities;
            }
        }

        public async Task<IEnumerable<__Entity__>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = "SELECT * FROM [dbo].__Entity__ __LEFT_JOIN__";

                var entities = await connection.QueryAsync<__Entity__>(sql);

                return entities;
            }
        }

        public async Task<__Entity__> GetByIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT * FROM [dbo].__Entity__ WHERE Id = @Id";
                var parameters = new { Id = id };

                var entity = await connection.QueryFirstOrDefaultAsync<__Entity__>(sql, parameters);

                if(entity is null)
                {
                    throw new ItemNotFoundException(id.ToString(), nameof(__Entity__));
                }

                return entity; 
            }
        }

        public async Task<IEnumerable<__Entity__>> GetByParametersAsync(Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                throw new ArgumentException("Parameters must not be null or empty", nameof(parameters));
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var whereClause = new StringBuilder("WHERE ");
                var dynamicParameters = new DynamicParameters();
                bool isFirstCondition = true;

                foreach (var param in parameters)
                {
                    if (!isFirstCondition)
                    {
                        whereClause.Append(" AND ");
                    }

                    whereClause.Append($"{param.Key} = @{param.Key}");
                    dynamicParameters.Add($"@{param.Key}", param.Value);
                    isFirstCondition = false;
                }

                var sql = $"SELECT * FROM [dbo].__Entity__ {whereClause}";

                var entities = await connection.QueryAsync<__Entity__>(sql, dynamicParameters);

                return entities;
            }
        }

        public async Task<Guid> RemoveAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = "DELETE FROM [dbo].__Entity__ WHERE Id = @Id";

                var parameters = new { Id = id };
                var rowsAffected = await connection.ExecuteAsync(sql, parameters);

                if (rowsAffected > 0)
                {
                    return id;
                }

                return Guid.Empty;

            }
        }

        public async Task<int> RemoveByParametersAsync(Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                throw new ArgumentException("Parameters cannot be null or empty.", nameof(parameters));
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var whereClauses = new List<string>();
                var sqlParameters = new DynamicParameters();

                foreach (var param in parameters)
                {
                    whereClauses.Add($"{param.Key} = @{param.Key}");
                    sqlParameters.Add($"@{param.Key}", param.Value);
                }

                var whereClause = string.Join(" AND ", whereClauses);
                var sql = $"DELETE FROM [dbo].__Entity__ WHERE {whereClause}";

                var rowsAffected = await connection.ExecuteAsync(sql, sqlParameters);

                return rowsAffected;
            }
        }

        public async Task<int> UpdateAsync(__Entity__ entity, Guid id)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var sql = $@"
                    UPDATE [dbo].__Entity__
                    SET __SET_PARAMETERS__
                    WHERE __PRIMARY_KEY__ = @Id;";


            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    Id = id, 
                    //__ENTITY_PARAMETERS_MAPPING__
                };

                var rowsAffected = await connection.ExecuteAsync(sql, parameters);

                return rowsAffected;
            }
        }
    }
}
