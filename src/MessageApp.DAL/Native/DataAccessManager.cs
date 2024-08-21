using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using System.Data.Common;

namespace MessageApp.DAL.Native
{
    public class DataAccessManager : IDisposable
    {
        private readonly string? _connectionString;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly ILogger<DataAccessManager> _logger;

        private readonly NpgsqlConnection _npgsqlConnection;

        public DataAccessManager(
            string connectionString,
            IHttpContextAccessor httpAccessor,
            ILogger<DataAccessManager> logger
        )
        {
            _connectionString = connectionString;
            _httpAccessor = httpAccessor;
            _logger = logger;

            _npgsqlConnection = new NpgsqlConnection(_connectionString);
        }

        /// <summary>
        /// > Execute a query and return the results as a data reader (READ)
        /// </summary>
        public async Task<DbDataReader> ExecuteReaderAsync(string query, IEnumerable<NpgsqlParameter>? parameters = null)
        {
            var correlationId = _httpAccessor.HttpContext?.Items["CorrelationId"]?.ToString();

            try
            {
                await using var command = new NpgsqlCommand(query, _npgsqlConnection);

                if (parameters?.Count() > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                await _npgsqlConnection.OpenAsync();
                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            catch (PostgresException e)
            {
                _logger.LogError("PostgreSQL error occurred: {Message}", e.Message);
                throw;
            }
            catch (NpgsqlException e)
            {
                _logger.LogCritical("Npgsql provider critical error occurred: {Message}", e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogCritical("DataAccesses an unexpected critical error occurred: {Message}", e.Message);
                throw;
            }
        }

        /// <summary>
        /// > Execute a SQL commands and return the number of rows affected (WRITE)
        /// </summary>
        public async Task<int> ExecuteNonQueryAsync(string query, IEnumerable<NpgsqlParameter>? parameters = null)
        {
            try
            {
                await using var command = new NpgsqlCommand(query, _npgsqlConnection);

                if (parameters?.Count() > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                await _npgsqlConnection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
            catch (PostgresException e)
            {
                _logger.LogError(e, "PostgreSQL error occurred: {Message}", e.Message);
                throw;
            }
            catch (NpgsqlException e)
            {
                _logger.LogCritical(e, "Npgsql provider critical error occurred: {Message}", e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "DataAccesses an unexpected critical error occurred: {Message}", e.Message);
                throw;
            }
        }

        /// <summary>
        /// > Execute a query and return the result of aggregate func 
        /// </summary>
        public async Task<object?> ExecuteScalarAsync(string query, IEnumerable<NpgsqlParameter>? parameters = null)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await using var command = new NpgsqlCommand(query, connection);

                if (parameters?.Count() > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                await connection.OpenAsync();
                return await command.ExecuteScalarAsync();
            }
            catch (PostgresException e)
            {
                _logger.LogError(e, "PostgreSQL error occurred: {Message}", e.Message);
                throw;
            }
            catch (NpgsqlException e)
            {
                _logger.LogCritical(e, "Npgsql provider critical error occurred: {Message}", e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "DataAccesses an unexpected critical error occurred: {Message}", e.Message);
                throw;
            }
        }

        public void Dispose()
        {
            _npgsqlConnection.Dispose();
        }
    }
}
