using MessageApp.API.Dtos;
using MessageApp.BLL.Models;

namespace MessageApp.API
{
    public static class Extensions
    {
        #region Map DTO

        public static MessageDto AsDto(this Message message)
        {
            return new MessageDto(message.Id, message.OrderNumber, message.Text, message.CreatedAt);
        }
        #endregion
    }
}
