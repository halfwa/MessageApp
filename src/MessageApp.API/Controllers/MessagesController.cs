using MessageApp.API.Dtos;
using MessageApp.API.Hubs;
using MessageApp.BLL.Interfaces;
using MessageApp.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MessageApp.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(
            IMessageService messageService,
            IHubContext<MessageHub> hubContext,
            ILogger<MessagesController> logger)
        {
            _messageService = messageService;
            _hubContext = hubContext;
            _logger = logger;
        }

        // GET /api/messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetAsync(DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null)
        {
            var messages = (await _messageService.GetAllMessagesAsync(fromDate, toDate))
                           .Select(message => message.AsDto());

            return Ok(messages);
        }

        // GET /api/messages/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MessageDto>> GetByIdAsync(Guid id)
        {
            var message = await _messageService.GetMessageAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message.AsDto();
        }

        // GET api/messages/count
        [HttpGet("count")]
        public async Task<ActionResult<long>> GetStudentsCount()
        {
            return Ok(await _messageService.MessagesCountAsync());
        }

        // POST /api/messages
        [HttpPost]
        public async Task<ActionResult<MessageDto>> PostAsync(CreateMessageDto createMessageDto)
        {
            var message = new Message
            {
               Id = Guid.NewGuid(),
               OrderNumber = createMessageDto.OrderNumber,
               Text = createMessageDto.Text,
               CreatedAt = DateTimeOffset.UtcNow
            };

            var count = await _messageService.AddMessageAsync(message);

            if (count <= 0) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                          "An error occurred while creating the message.");
            }

            await _hubContext.Clients.All.SendAsync("receiveMessageUpdates", message);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = message.Id }, message);
        }
    }
}
