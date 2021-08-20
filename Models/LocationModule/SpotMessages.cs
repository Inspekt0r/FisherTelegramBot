using System.Collections.Generic;
using System.Text;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models.LocationModule
{
    public class SpotMessages
    {
        private readonly List<Spot> _spots;
        private readonly Character _character;
        private readonly StringBuilder _sb = new StringBuilder();

        public SpotMessages(List<Spot> spots, Character character)
        {
            _spots = spots;
            _character = character;
        }

        public string GetSpotList()
        {
            _sb.AppendLine($"<b>Список локаций:</b>");
            foreach (var spot in _spots)
            {
                if (spot.Name == _character.Spot.Name)
                {
                    _sb.AppendLine($"<i>{spot.Name} (ты здесь) </i>");
                }
                else
                {
                    _sb.AppendLine($"<i>{spot.Name} перейти: /move_{spot.Id} </i>");
                }
            }

            return _sb.ToString();
        }
    }
}