using System.ComponentModel.DataAnnotations;

namespace MessageApp.API.Dtos
{
    public record MessageDto(
        Guid Id,
        long OrderNumber,
        string Text, 
        DateTimeOffset CreatedAt
    );
    public record CreateMessageDto(
        [Required] long OrderNumber,
        [Length(1, 128)] string Text
    );

}
