using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Common;

namespace Telegram.Bot.AspNetCore
{
    public class HandlersProvider : IHandlersProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public HandlersProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<CommandHandler> GetHandlers()
        {
            return _serviceProvider.GetServices<CommandHandler>();
        }
    }
}
