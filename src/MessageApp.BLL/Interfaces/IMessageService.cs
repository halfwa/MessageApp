using MessageApp.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.BLL.Interfaces
{
    public interface IMessageService
    {
        Task<Message?> GetMessageAsync(Guid id);
        Task<List<Message>> GetAllMessagesAsync(DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null);
        Task<int> AddMessageAsync(Message message);
        Task<long?> MessagesCountAsync();
    }
}
