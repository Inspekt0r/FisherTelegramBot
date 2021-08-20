using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models.Commands
{
    public class SetupNicknameCommand : ICommand
    {
        public List<string> Name { get; }
        private readonly ILogger<SetupNicknameCommand> _logger;

        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, SetupNicknameCommand>();
        }

        public SetupNicknameCommand(ILogger<SetupNicknameCommand> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            var user = message.From;
            var userName = message.Text;
            await using var dbContext = new ApplicationContext();

            var character = dbContext.Characters.First(p => p.TelegramId == user.Id);

            if (userName.Length > 16 || userName.Length < 3)
            {
                await telegramBot.SendTextMessageAsync(user.Id,
                    $"Длина никнейма слишком большая или короткая, хер его знает попробуй еще раз");
            }
            else
            {
                character.Name = userName;

                await telegramBot.SendTextMessageAsync(user.Id, $"Welcome to the club buddy!\n" +
                                                                $"Для вызова справки щёлкни /help\n" +
                                                                $"Твой профиль здесь: /profile");

                _logger.LogInformation($"Создан персонаж с именем {userName}");

                //todo сделать задачу стартовой локации дефолтной наверное а как а хуй его знает
                var spotStart = dbContext.Spots.First(p => p.Name == "Стартовое озеро");
                character.Spot = spotStart;

                var fishingRod = new BackpackItem()
                {
                    ItemType = ItemType.FishingRod,
                    ItemName = "Почти целая удочка",
                    Rarity = Rarity.Common,
                    Weight = 1.0,
                    Height = 5.0,
                    CatchBonus = 0.15,
                    HookBonus = 0.15,
                    Count = 1,
                    IsEquipped = false
                };

                character.Backpack.BackpackItems.Add(fishingRod);

                await telegramBot.SendTextMessageAsync(user.Id, $"А чтобы освоиться здесь вот тебе подарок: \n" +
                                                                $"<b>Получено {fishingRod.ItemName}</b>", ParseMode.Html);

                character.IsSetupNickname = true;
                await dbContext.SaveChangesAsync();
            }
        }

        public bool Contains(Message message)
        {
            var userId = message.From.Id;
            if (message.Chat.Id != userId)
            {
                return false;
            }
            using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == userId);

            if (message.Type != MessageType.Text)
            {
                return false;
            }

            if (character == null || character.IsSetupNickname)
            {
                return false;
            }

            return true;
        }
    }
}