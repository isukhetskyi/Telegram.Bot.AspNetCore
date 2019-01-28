using Microsoft.AspNetCore.Mvc;
using Rollbar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Common;
using Telegram.Bot.Types;

namespace Telegram.Bot.AspNetCore.Controllers
{
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUpdatesHandler _updatesHandler;

        public IRollbar Rollbar { get; set; }

        public MessageController(IUpdatesHandler updatesHandler)
        {
            _updatesHandler = updatesHandler;
        }

        [HttpPost("")]
        public async Task<OkResult> HandleUpdateAsync(Update update)
        {
            try
            {
                await _updatesHandler.HandleUpdateAsync(update);
            }
            catch (Exception exception)
            {
                await Rollbar.Critical(exception, new Dictionary<string, object> { { "update", update } });
            }

            return Ok();
        }

        [HttpGet("")]
        public string Test()
        {
            return "It works";
        }
    }
}
