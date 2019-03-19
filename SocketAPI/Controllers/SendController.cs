using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SocketAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SendController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(string token, string msg)
        {
            return new[] { token, msg };
        }
    }
}
