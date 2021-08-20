using System.Linq;
using System.Text;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.Enum;

namespace TelegramAspBot.Models
{
    public class ItemControlSystem
    {
        private readonly Character _character;
        private readonly StringBuilder _sb = new StringBuilder();

        public ItemControlSystem(Character character)
        {
            _character = character;
        }

        public StringBuilder TryEquipItem(BackpackItem backpackItem)
        {
            if (_character.CharState == State.Fishing)
            {
                _sb.AppendLine($"Ты в процессе рыбалки, подожди");
                return _sb;
            }
            
            if (backpackItem.IsDeleted)
            {
                _sb.AppendLine($"У тебя нет этого предмета");
                return _sb;
            }
            
            if (backpackItem.IsEquipped)
            {
                _sb.AppendLine($"У тебя уже экипирован {backpackItem.ItemName}");
                return _sb;
            }

            if (backpackItem.ItemType != ItemType.FishingRod && backpackItem.ItemType != ItemType.Bait)
            {
                _sb.AppendLine($"Ты не можешь экипировать это");
                return _sb;
            }

            var equippedItem = _character.Backpack.BackpackItems
                .FirstOrDefault(p => p.ItemType == backpackItem.ItemType && p.IsEquipped);
            
            if (equippedItem != null)
            {
                equippedItem.IsEquipped = false;
                _sb.AppendLine($"Успех, ты снял с себя <b>{equippedItem.ItemName}</b> с тобой теперь {BackpackTextGenerator.GetRarityType(backpackItem.Rarity)}<b>{backpackItem.ItemName}</b>");
            }
            else
            {
                _sb.AppendLine($"Успех, с тобой теперь {BackpackTextGenerator.GetRarityType(backpackItem.Rarity)}<b>{backpackItem.ItemName}</b>");
            }

            backpackItem.IsEquipped = true;

            return _sb;
        }

        public StringBuilder TryUnequipItem(BackpackItem backpackItem)
        {
            if (_character.CharState == State.Fishing)
            {
                _sb.AppendLine($"Ты в процессе рыбалки, подожди");
                return _sb;
            }
            
            if (backpackItem.IsDeleted)
            {
                _sb.AppendLine($"У тебя нет этого предмета");
                return _sb;
            }
            
            if (backpackItem.ItemType != ItemType.FishingRod && backpackItem.ItemType != ItemType.Bait)
            {
                _sb.AppendLine($"Ты не можешь снять это");
                return _sb;
            }
            
            if (backpackItem.IsEquipped)
            {
                _sb.AppendLine($"Успех, {backpackItem.ItemName} снят!");

                backpackItem.IsEquipped = false;
                return _sb;
            }
            else
            {
                _sb.AppendLine($"Предмет не экипирован");
                return _sb;
            }
        }
    }
}
