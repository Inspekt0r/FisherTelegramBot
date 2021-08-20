using System.Linq;
using System.Text;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class TopPlayersSystem
    {
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly Character _character;

        public TopPlayersSystem(Character character)
        {
            _character = character;
        }

        public StringBuilder GetTopListByPercentCatches()
        {
            using var dbContext = new ApplicationContext();
            var characters = dbContext.Characters
                .Where(p => p.Name != null && !p.Banned)
                .OrderByDescending(p => p.CharStat.Percent)
                .ToList();
            
            var counter = 1;
            
            _sb.AppendLine($"Топ 20 игроков по % успешных попыток");
            
            foreach (var character in characters)
            {
                if (character.TelegramId == _character.TelegramId && counter < 21)
                {
                    _sb.AppendLine($"<b>{counter}) {character.Name} {character.CharStat.PercentCatches()}%</b>");
                }
                else if (counter < 21)
                {
                    _sb.AppendLine($"{counter}) {character.Name} {character.CharStat.PercentCatches()}%");
                } else if (character.TelegramId == _character.TelegramId)
                {
                    _sb.AppendLine($"...");
                    _sb.AppendLine($"<b>{counter}) {character.Name} {character.CharStat.PercentCatches()}%</b>");
                }

                counter++;
            }

            return _sb;
        }

        public StringBuilder GetTopListByCatchedFish()
        {
            using var dbContext = new ApplicationContext();
            var characters = dbContext.Characters
                .Where(p => p.Name != null && !p.Banned)
                .OrderByDescending(p => p.CharStat.FishCaughtCount)
                .ToList();
            
            var counter = 1;

            _sb.AppendLine($"Топ 10 игроков по пойманой рыбе");
            
            foreach (var character in characters)
            {
                if (character.TelegramId == _character.TelegramId && counter < 11)
                {
                    _sb.AppendLine($"<b>{counter}) {character.Name} {character.CharStat.FishCaughtCount}</b>");
                }
                else if (counter < 11)
                {
                    _sb.AppendLine($"{counter}) {character.Name} {character.CharStat.FishCaughtCount}");
                } else if (character.TelegramId == _character.TelegramId)
                {
                    _sb.AppendLine($"...");
                    _sb.AppendLine($"<b>{counter}) {character.Name} {character.CharStat.FishCaughtCount}</b>");
                }

                counter++;
            }

            return _sb;
        }

        public StringBuilder GetTopByCurrentSeasonPoint()
        {
            using var dbContext = new ApplicationContext();
            var players = dbContext.Characters
                .Where(p => p.Name != null && !p.Banned)
                .ToList();
            var orderedByPoints = players.OrderByDescending(p => p.SeasonPoints).ToList();
            
            var position = 1;
            _sb.AppendLine($"Топ игроков по сезонным очкам");
            foreach (var character in orderedByPoints)
            {
                if (character.TelegramId == _character.TelegramId && position < 11)
                {
                    _sb.AppendLine($"<b>{position}) {character.Name} {character.SeasonPoints} sP</b>");
                }
                else if (position < 11)
                {
                    _sb.AppendLine($"{position}) {character.Name} {character.SeasonPoints} sP");
                } else if (character.TelegramId == _character.TelegramId)
                {
                    _sb.AppendLine($"...");
                    _sb.AppendLine($"<b>{position}) {character.Name} {character.SeasonPoints} sP</b>");
                }

                position++;
            }
            
            return _sb;
        }
    }
}