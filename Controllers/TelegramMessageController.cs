using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Models;
using TelegramAspBot.Models.Commands;
using TelegramAspBot.Models.Interfaces;

namespace TelegramAspBot.Controllers
{
    [Route(@"api/message/update")]
    [ApiController]
    public class TelegramMessageController : ControllerBase
    {
        private readonly ILogger<TelegramMessageController> _logger;

        private readonly IEnumerable<ICommand> _allCommands;
        private readonly IEnumerable<ICallback> _allCallbacks;
        private readonly IBotService _telegramBot;

        public TelegramMessageController(ILogger<TelegramMessageController> logger, IEnumerable<ICommand> allCommands,
            IBotService telegramBot, IEnumerable<ICallback> allCallbacks)
        {
            _logger = logger;
            _allCommands = allCommands;
            _telegramBot = telegramBot;
            _allCallbacks = allCallbacks;
        }

        [HttpGet]
        public string Get()
        {
            _logger.LogInformation($"Вроде стартанули, кек");

            return "Awesome BullShit";
        }

        [HttpPost]
        public async Task<OkResult> Update([FromBody] Update update)
        {
            //todo разобраться с варнингом:
            //TelegramMessageController.cs(41, 37): [CS1998] В данном асинхронном методе отсутствуют операторы await,
            //поэтому метод будет выполняться синхронно. Воспользуйтесь оператором await для ожидания неблокирующих
            //вызовов API или оператором await Task.Run(...) для выполнения связанных с ЦП заданий в фоновом потоке.
            
            if (update == null)
            {
                return Ok();
            }

            var callback = update.CallbackQuery;
            if (callback != null && callback.Message.Type == MessageType.Text)
            {
                var callBackMessage = callback.Data;
                _logger.LogInformation(
                    $"TelegramMessageController: пришел колбэк от {callback.From.Id}\nСодержание {callBackMessage}");
                //old variant commands changed to _allCommands
                foreach (var command in _allCallbacks)
                {
                    if (command.Contains(callback))
                    {
                        _logger.LogTrace(
                            $"TelegramMessageController: проверяю соответствие команды для {callBackMessage}");
                        Task.Run(async () =>
                            await command.ExecuteCommand(callback, _telegramBot.GetBotClient()));
                        break;
                    }
                }

                return Ok();
            }

            var message = update.Message;
            if (message == null || message.Type != MessageType.Text)
            {
                return Ok();
            }

            _logger.LogInformation(
                $"TelegramMessageController: пришло сообщение от {message.From.Id}\nСодержание {message.Text} chatId: {message.Chat.Id}");
            //old variant commands changed to _allCommands
            foreach (var command in _allCommands)
            {
                if (command.Contains(message))
                {
                    _logger.LogTrace($"TelegramMessageController: проверяю соответствие команды для {message.Text}");
                    Task.Run(async () => await command.ExecuteCommand(message, _telegramBot.GetBotClient()));
                    break;
                }
            }

            return Ok();
        }
    }
}