using System;
using System.Linq;
using System.Text;
using TelegramAspBot.Job;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.Enum;

namespace TelegramAspBot.Models
{
    public class ProfileGenerator
    {
        private readonly Character _character;
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly JobLure _jobLure;
        private readonly Season _season;
        private readonly Event _activeEvent;

        public ProfileGenerator(Character character, JobLure lure)
        {
            _character = character;
            _jobLure = lure;
            using var dbContext = new ApplicationContext();
            _season = dbContext.SeasonStats.First(p => !p.IsEnded);
            _activeEvent = dbContext.Events.FirstOrDefault(p => p.IsActive);
        }

        public string GetProfile()
        {
            _sb.AppendLine($"<b>{_character.Name}</b>\n");
            _sb.AppendLine($"Текущий сезон #{_season.Number} ({_character.SeasonPoints} sP)");
            _sb.AppendLine($"Спот: {_character.Spot.Name} /spots\n");
            _sb.AppendLine($"<i>Твой статус:\n{GetState(_character.CharState)}</i>");

            if (_activeEvent!= null && _activeEvent.IsActive)
            {
                _sb.AppendLine();
                _sb.AppendLine($"Активное событие на поимку рыбы: {_activeEvent.FishName}");
                _sb.AppendLine($"Ты поймал: {_character.CharStat.EventFishCount}");
            }
            
            if (_jobLure != null)
            {
                var timeDif = DateTime.UtcNow - _jobLure.LureTimeLeft;
                _sb.AppendLine($"\n{_jobLure.LureItem.ItemName} <i>активно</i> {timeDif.Minutes * -1}:{timeDif.Seconds * -1} м");
            }
            
            _sb.AppendLine($"\nРюкзак: /backpack");
            _sb.AppendLine($"FishCoin: {_character.Money}");

            _sb.AppendLine();
            _sb.AppendLine($"<i>Настройки /settings</i>");

            return _sb.ToString();
        }
        
        public string GetStartProfile()
        {
            _sb.AppendLine($"<b>Привет {_character.Name}!</b>\n");
            _sb.AppendLine($"Текущий сезон #{_season.Number} ({_character.SeasonPoints} sP)");
            _sb.AppendLine($"Спот: {_character.Spot.Name} /spots\n");
            _sb.AppendLine($"<i>Твой статус:\n{GetState(_character.CharState)}</i>");
            
            if (_jobLure != null)
            {
                var timeDif = DateTime.UtcNow - _jobLure.LureTimeLeft;
                _sb.AppendLine($"\n{_jobLure.LureItem.ItemName} <i>активно</i> {timeDif.Minutes * -1}:{timeDif.Seconds * -1} м");
            }
            
            _sb.AppendLine($"\nРюкзак: /backpack");
            _sb.AppendLine($"FishCoin: {_character.Money}");

            return _sb.ToString();
        }

        private static string GetState(State state)
        {
            var str = state switch
            {
                State.Idle => "Ничего не делаешь",
                State.Walk => "Идёшь",
                State.Fight => "Пиздишься",
                State.Death => "Сдох",
                State.Fishing => "Ловишь рыбку",
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };

            return str;
        }
    }
}