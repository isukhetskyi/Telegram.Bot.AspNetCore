using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rollbar;
using Telegram.Bot.Common;

namespace Telegram.Bot.AspNetCore
{
    public static class ServicesExtensions
    {
        private static List<Type> AllUserTypes()
        {
            var assemblies = new List<Assembly>
            {
                Assembly.GetEntryAssembly(),
                Assembly.GetExecutingAssembly()
            };

            return assemblies.SelectMany(a => a.GetTypes()).ToList();
        }
        public static void RegisterAppSettings(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            var appSettings = AllUserTypes().FirstOrDefault(t => t.Name == "AppSettings");
            if (appSettings == null)
            {
                return;
            }
            var appSettingsInterface = AllUserTypes().First(t => t.IsInterface && t.IsAssignableFrom(appSettings));
            if(appSettingsInterface == null)
            {
                return;
            }
            var appSettingsInstance = Activator.CreateInstance(appSettings);
            configurationSection.Bind(appSettingsInstance);
            services.AddSingleton(appSettingsInterface, appSettingsInstance);
        }

        public static void RegisterCommandHandlers(this IServiceCollection services)
        {
            var handlers = AllUserTypes().Where(t => t != typeof(CommandHandler) && typeof(CommandHandler).IsAssignableFrom(t));

            foreach (var handler in handlers)
            {
                services.AddScoped(typeof(CommandHandler), handler);
            }
        }

        public static void RegisterRollbar(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            var baseAppSettings = configurationSection.Get<BaseAppSettings>();

            var config = new RollbarConfig(baseAppSettings.RollbarToken)
            {
                Environment = baseAppSettings.RollbarEnvironment,
                LogLevel = ErrorLevel.Info
            };

            RollbarLocator.RollbarInstance.Configure(config);
            services.AddSingleton<IRollbar>(RollbarLocator.RollbarInstance);
        }

        public static void RegisterTelegramClient(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            var baseAppSettings = configurationSection.Get<BaseAppSettings>();

            var telegramBotClient = new TelegramBotClient(baseAppSettings.Key);
            telegramBotClient.SetWebhookAsync(baseAppSettings.Url).Wait();

            services.AddSingleton<ITelegramBotClient>(telegramBotClient);
        }
    }
}
