using System;
using System.Collections.Generic;
using System.Text;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class PageSystem
    {
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly Character _character;
        private readonly List<BackpackItem> _items;

        public PageSystem(Character character, List<BackpackItem> items)
        {
            _character = character;
            _items = items;
        }

        public string GenerateText(int pagePointer)
        {
            var allItemsNumber = _items.Count;
            var startItemNumber = (pagePointer * 20) - 20;
            var endItemNumber = pagePointer * 20;
            if (endItemNumber > allItemsNumber)
            {
                endItemNumber = allItemsNumber;
            }
            for (var i = startItemNumber; i < endItemNumber; i++)
            {
                _sb.AppendLine($"<b>*</b> {BackpackTextGenerator.GetRarityType(_items[i].Rarity)} {_items[i].ItemName}" +
                               $"\nТип рыбы: {FishPediaTextGenerator.GetFishType(_items[i].FishBiteType)} - {Math.Round(_items[i].Weight, 2)}кг. {Math.Round(_items[i].Height, 2)}м. - 🐟");
            }
            return _sb.ToString();
        }
    }
}