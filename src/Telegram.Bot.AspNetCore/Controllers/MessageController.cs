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
        private readonly IRollbar _rollbar;

        public MessageController(IUpdatesHandler updatesHandler, IRollbar rollbar)
        {
            _updatesHandler = updatesHandler;
            _rollbar = rollbar;
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
                await _rollbar.Critical(exception, new Dictionary<string, object> { { "update", update } });
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
