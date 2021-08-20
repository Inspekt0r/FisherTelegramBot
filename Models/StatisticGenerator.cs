using System.Text;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class StatisticGenerator
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public StringBuilder GetStatistic(Character character)
        {
            var stats = character.CharStat;
            
            _sb.AppendLine($"<b>Твоя статистика {character.Name}:</b>");
            _sb.AppendLine($"Дата создания персонажа: <i>{character.CreationTime:d}</i>");
            _sb.AppendLine($"~~~~~~~~~~~~~~~~~");
            _sb.AppendLine($"<b>Твои рыболовные успехи:</b>");
            _sb.AppendLine($"Поймал рыбы: <i>{stats.FishCaughtCount}</i> ({stats.PercentCatches()}%)");
            _sb.AppendLine($"Попыток поймать рыбу: <i>{stats.FishingTry}</i>");
            _sb.AppendLine($"Успешных подсечек: <i>{stats.HookCount}</i>");
            _sb.AppendLine($"Самая тяжелая рыба (в кг): <i>{stats.MostWeightFish}</i>");
            _sb.AppendLine($"Самая длинная рыба (в м): <i>{stats.MostHeightFish}</i>");
            //_sb.AppendLine($"Использовано приманок: <i>{stats.BaitUsingCount}</i>");
            _sb.AppendLine($"~~~~~~~~~~~~~~~~~");
            if (IsSpecialAchievements(stats))
            {
                _sb.AppendLine($"<b>Скрытые достижения:</b>");
                _sb.AppendLine($"~~~~~~~~~~~~~~~~~");
                _sb.AppendLine($"Поймано рыбы мечты (Язей): <i>{stats.FishOfMyDreams}</i>");
            }
            
            return _sb;
        }

        private bool IsSpecialAchievements(CharStat stat)
        {
            return stat.FishOfMyDreams != 0;
        }
    }
}