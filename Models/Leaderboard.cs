using System.Linq;
using System.Text;

namespace TelegramAspBot.Models
{
    public class Leaderboard
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public StringBuilder GetCaughTheMostFishChars()
        {
            using var dbContext = new ApplicationContext();
            var charStats = dbContext.Characters.Where(p => p.Banned == false).ToList();
            var topFisherman = charStats.OrderBy(p => p.CharStat.FishCaughtCount).ToList();
            _sb.AppendLine($"Топ рыбаков по пойманной рыбе: \n");
            int counter = 1;
            foreach (var character in topFisherman)
            {
                if (counter == 21)
                {
                    break;
                }
                _sb.AppendLine($"{counter}] {character.Name} - {character.CharStat.FishCaughtCount}");
                counter++;
            }

            return _sb;
        }
    }
}