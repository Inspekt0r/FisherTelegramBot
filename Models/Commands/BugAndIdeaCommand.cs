using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramAspBot.Models.Commands
{
    public class BugAndIdeaCommand : ICommand
    {
        private readonly ChatId _chatReportId = new ChatId(-575742119);
        private readonly ILogger<BugAndIdeaCommand> _logger;

        public BugAndIdeaCommand(ILogger<BugAndIdeaCommand> logger)
        {
            _logger = logger;
        }

        public List<string> Name { get; } = new List<string>() { "#идея", "#баг" };
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, BugAndIdeaCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            var chat = message.Chat;

            await telegramBot.SendTextMessageAsync(chat.Id, $"Спасибо за фидбек!", replyToMessageId: message.MessageId);
            _logger.LogInformation($"Получил сообщение о баге/фиче от {message.From.Id}\nТекст фичи: \"{message.Text}\"");
            
            await telegramBot.ForwardMessageAsync(_chatReportId, chat, message.MessageId);
            _logger.LogInformation($"Переслал сообщение о баге/фиче от {message.From.Id}\nТекст фичи: \"{message.Text}\"");

        }

        public bool Contains(Message message)
        {
            return Name.Any(str => message.Text.ToLower().Contains(str));
        }
    }
}