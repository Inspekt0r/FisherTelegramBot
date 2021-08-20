using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using TelegramAspBot.Job;
using TelegramAspBot.Models;
using TelegramAspBot.Models.Commands;
using TelegramAspBot.Models.CreatingScript;
using TelegramAspBot.Models.Interfaces;

namespace TelegramAspBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>();

            services.AddControllers().AddNewtonsoftJson();

            services.AddSingleton<IBotService, Bot>();

            services.AddSingleton<JobManager>();

            services.AddSingleton<EventSystem>();

            var commandType = typeof(ICommand);
            var commandImpl = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(p => commandType.IsAssignableFrom(p) && p.IsClass);

            foreach (var impl in commandImpl)
            {
                services.AddTransient(commandType, impl);
            }
            
            var commandTypeCallback = typeof(ICallback);
            var commandImplCallback = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(p => commandTypeCallback.IsAssignableFrom(p) && p.IsClass);

            foreach (var impl in commandImplCallback)
            {
                services.AddTransient(commandTypeCallback, impl);
            }
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateDatabase(app);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseTelegramBot();

            app.UseJobManager();

            app.UseEventSystem();
        }
        
        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationContext>();
            context.Database.Migrate();
            //CopyName.MoveItemToBackpackItem(context);
            //ReworkOfItems.LetsRockNRoll(context);
        }
    }
}
