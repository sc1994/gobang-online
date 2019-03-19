using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SocketAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SendController : ControllerBase
    {
        private readonly IHubContext<Hub.Hub> _hubContext;

        public SendController(IHubContext<Hub.Hub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get(string token, string msg)
        {
            await _hubContext.Clients.Group(token).SendAsync("Send", msg);
            return "OK";
        }
    }
}
