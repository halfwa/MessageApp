using MessageApp.API.Dtos;
using MessageApp.API.Hubs;
using MessageApp.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MessageApp.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(
            IHubContext<MessageHub> hubContext,
            ILogger<MessagesController> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        // GET /api/messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetAsync(DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null)
        {
            return Ok("messages");
        }

        // GET /api/messages/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MessageDto>> GetByIdAsync(Guid id)
        {
            return Ok("message");
        }

        // GET api/messages/count
        [HttpGet("count")]
        public async Task<ActionResult<long>> GetStudentsCount()
        {
            return Ok(0);
        }

        // POST /api/messages
        [HttpPost]
        public async Task<ActionResult<MessageDto>> PostAsync(CreateMessageDto createMessageDto)
        {

            return Ok();
        }
    }
}
