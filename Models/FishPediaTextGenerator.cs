using System;
using System.Text;
using System.Linq;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class FishPediaTextGenerator
    {
        private readonly Character _character;
        private readonly StringBuilder _sb = new StringBuilder();

        public FishPediaTextGenerator(Character character)
        {
            _character = character;
        }

        public StringBuilder GetAllFishPediaInfo()
        {
            var fishPedia = _character.FishPedia.FishPediaInfoList;
            var sortedFishPedia = fishPedia
                .OrderBy(p => p.Rarity)
                .ThenBy(p => p.Name).ToList();

            _sb.AppendLine($"<b>Рыбная Энциклопедия рыбака {_character.Name}:</b>");
            
            foreach (var fishInfo in sortedFishPedia)
            {
                _sb.AppendLine($"*{BackpackTextGenerator.GetRarityType(fishInfo.Rarity)} " +
                               $"<b>{fishInfo.Name}</b> <i>{GetFishType(fishInfo.FishType)}</i> - поймано: {fishInfo.Caught}");
            }

            return _sb;
        }

        public static string GetFishType(FishType type)
        {
            return type switch
            {
                FishType.Herbivorous => "нехищная",
                FishType.Carnivorous => "хищная",
                FishType.Combo => "комбо",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}