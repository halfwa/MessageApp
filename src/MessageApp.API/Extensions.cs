using MessageApp.API.Dtos;
using MessageApp.BLL.Models;
using MessageApp.DAL.Native;

namespace MessageApp.API
{
    public static class Extensions
    {
        public static WebApplicationBuilder AddDataLayerAccess(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                throw new InvalidOperationException("The GetConnectionString operation returned null"); 

            builder.Services
                .AddTransient(provider =>
                {
                    var logger = provider.GetRequiredService<ILogger<DataAccessManager>>();
                    var httpAccessor = provider.GetRequiredService<IHttpContextAccessor>();

                    return new DataAccessManager(connectionString, httpAccessor, logger);
                });

            return builder;
        }

        #region Map DTO

        public static MessageDto AsDto(this Message message)
        {
            return new MessageDto(message.Id, message.OrderNumber, message.Text, message.CreatedAt);
        }
        #endregion
    }
}
