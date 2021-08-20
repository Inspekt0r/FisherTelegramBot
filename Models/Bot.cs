using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramAspBot.Models.Interfaces;

namespace TelegramAspBot.Models
{
    public class Bot : IBotService
    {
        private TelegramBotClient _botClient;

        public Bot()
        {
            //init webhook
            _botClient = new TelegramBotClient(AppSettings.ApiToken);
        }
        public async Task Initialize()
        {
            var hook = string.Format(AppSettings.Url, "/api/message/update");
            await _botClient.SetWebhookAsync(hook);
        }

        public TelegramBotClient GetBotClient() => _botClient;
    }

    public static class BotExtensions
    {
        public static IApplicationBuilder UseTelegramBot(this IApplicationBuilder app)
        {
            var bot = app.ApplicationServices.GetRequiredService<IBotService>();
            bot.Initialize().Wait();
            return app;
        }
    }
}
