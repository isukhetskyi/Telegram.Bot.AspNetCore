using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Common;

namespace Telegram.Bot.AspNetCore
{
    public class Startup
    {
        private IConfigurationSection AppSettings { get; }
        private IConfigurationSection BaseAppSettings { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true);

            var configuration = builder.Build();
            AppSettings = configuration.GetSection("AppSettings");
            BaseAppSettings = configuration.GetSection("BaseSettings");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.RegisterAppSettings(AppSettings);
            services.RegisterCommandHandlers();
            services.RegisterRollbar(BaseAppSettings);
            services.RegisterTelegramClient(BaseAppSettings);

            services.AddScoped<IUpdatesHandler, UpdatesHandler>();
            services.AddScoped<IHandlersProvider, HandlersProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
        }
    }
}
