using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.Enum;

namespace TelegramAspBot.Models.LocationModule
{
    public class WalkingModule
    {
        private readonly ITelegramBotClient _telegramBot;
        
        private readonly Character _character;

        public WalkingModule(ITelegramBotClient telegramBot, Character character)
        {
            _telegramBot = telegramBot;
            _character = character;
        }

        public async Task TryGoToSpotAsync(Spot spot)
        {
            if (_character.CharState == State.Fishing)
            {
                await _telegramBot.SendTextMessageAsync(_character.TelegramId, $"Ты еще в процессе ловли");
                return;
            }

            if (_character.CharState == State.Walk)
            {
                await _telegramBot.SendTextMessageAsync(_character.TelegramId, $"Ты куда-то идёшь");
                return;
            }

            if (_character.CharState == State.Idle)
            {
                await _telegramBot.SendTextMessageAsync(_character.TelegramId, $"<b>Ты пришёл к {spot.Name}</b>" +
                                                                               $"\n{GetSpotFishesList(spot)}", ParseMode.Html);
                _character.Spot = spot;
            }
        }

        public static string GetSpotFishesList(Spot spot)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Посмотрев по сторонам был обнаружена табличка со списком рыбы, которая здесь водится:\n");
            sb.AppendLine($"{GetTextFishes(spot.FishReferenceSpots)}");
            sb.AppendLine($"Другие локации: /spots");

            return sb.ToString();
        }

        private static string GetTextFishes(List<FishReferenceSpot> fishRef)
        {
            var sb = new StringBuilder();
            
            var orderedFish = fishRef.OrderBy(p => p.FishReference.Rarity).ToList();
            foreach (var fish in orderedFish)
            {
                sb.AppendLine($"<b>{fish.FishReference.Name}</b> <i>{fish.FishReference.Rarity}</i>");
            }

            return sb.ToString();
        }
    }
}