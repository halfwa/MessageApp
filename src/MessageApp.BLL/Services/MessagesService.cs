using MessageApp.BLL.Interfaces;
using MessageApp.BLL.Models;
using MessageApp.DAL.Native;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace MessageApp.BLL.Services
{
    public class MessagesService: IMessageService
    {
        private readonly DataAccessManager _dataAccessManager;
        private readonly ILogger<DataAccessManager> _logger;

        public MessagesService(
            DataAccessManager dataAccessManager,
            ILogger<DataAccessManager> logger
        )
        {
            _dataAccessManager = dataAccessManager;
            _logger = logger;
        }
        public async Task<List<Message>> GetAllMessagesAsync(
            DateTimeOffset? fromDate = null, 
            DateTimeOffset? toDate = null)
        {
            fromDate ??= DateTimeOffset.MinValue;
            toDate ??= DateTimeOffset.UtcNow;

            string query = "SELECT * FROM messages WHERE created_at > @FromDate AND created_at < @ToDate";
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@FromDate", fromDate),
                new NpgsqlParameter("@ToDate", toDate)
            };

            var messages = new List<Message>();

            try
            {
                await using (var dataReader = await _dataAccessManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await dataReader.ReadAsync())
                    {
                        Message message = new Message();

                        message.Id = dataReader.GetGuid(0);
                        message.OrderNumber = dataReader.GetInt64(1);
                        message.Text = dataReader.GetString(2);
                        message.CreatedAt = (DateTimeOffset)dataReader.GetDateTime(3);

                        messages.Add(message);
                    }

                    _logger.LogInformation("Successfully retrieved {Count} messages in {Method} method.",
                        messages.Count, 
                        nameof(GetAllMessagesAsync)
                    );

                    return messages;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error excuting of {Method}.", nameof(GetAllMessagesAsync));
                throw;
            }
        }

        public async Task<Message?> GetMessageAsync(Guid id)
        {
            var query = "SELECT * FROM messages WHERE Id = @Id ";
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@Id", id)
            };

            try
            {
                var dataReader = await _dataAccessManager.ExecuteReaderAsync(query, parameters);

                if (dataReader.Read())
                {
                    Message message = new Message();

                    message.Id = dataReader.GetGuid(0);
                    message.OrderNumber = dataReader.GetInt64(1);
                    message.Text = dataReader.GetString(2);
                    message.CreatedAt = (DateTimeOffset)dataReader.GetDateTime(3);

                    return message;
                }

                _logger.LogWarning("Message with id = '{id}' is not found in {Method}.",
                    id,
                    nameof(GetMessageAsync)
                );

                return null;
            }
            catch (Exception)
            {
                _logger.LogError("Error excuting of {Method} with id = '{id}'.", nameof(GetMessageAsync), id);
                throw;
            }
        }

        public async Task<int> AddMessageAsync(Message message)
        {
            var command = "INSERT INTO messages (id, order_number, text, created_at) " +
                          "VALUES (@Id, @OrderNumber, @Text, @CreatedAt)";
            var parameters = new NpgsqlParameter[]
            {
                new  NpgsqlParameter("@Id", message.Id),
                new  NpgsqlParameter("@OrderNumber", message.OrderNumber),
                new  NpgsqlParameter("@Text", message.Text),
                new  NpgsqlParameter("@CreatedAt", message.CreatedAt)
            };

            try
            {
                var rowCount = await _dataAccessManager.ExecuteNonQueryAsync(command, parameters);

                _logger.LogInformation("Message with id = '{id}' was processed in {Method} with result {rowCount}.",
                    message.Id,
                    nameof(AddMessageAsync),
                    rowCount
                );

                return rowCount;
            }
            catch (Exception)
            {
                _logger.LogError("Error excuting of {Method}.", nameof(AddMessageAsync));
                throw;
            }
        }

        public async Task<long?> MessagesCountAsync()
        {
            var query = "SELECT COUNT(*) FROM messages";

            try
            {
                var messageCount = await _dataAccessManager.ExecuteScalarAsync(query);

                if (messageCount is long number)
                {
                    return number;
                }
                else
                {
                   return null;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error excuting of {Method}.", nameof(MessagesCountAsync));
                throw;
            }
        }
    }
}
