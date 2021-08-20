using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class DurabilitySystem
    {
        private readonly Character _character;
        private readonly StringBuilder _sb = new StringBuilder();

        public DurabilitySystem(Character character)
        {
            _character = character;
        }

        public string CheckBrokenItems()
        {
            var backpackItems = _character.Backpack.BackpackItems;
            var needSend = false;
            
            foreach (var backpackItem in backpackItems)
            {
                if (backpackItem.GetDurability() <= 0.0)
                {
                    needSend = true;
                    
                    _sb.AppendLine($"\n{backpackItem.ItemName} сломался, не забудь поменять его на что-то аналогичное!");
                    backpackItem.Count--;
                    backpackItem.Durability = 200;
                    if (backpackItem.Count <= 0)
                    {
                        backpackItem.IsDeleted = true;
                        backpackItem.IsEquipped = false;
                    }
                }
            }

            return needSend ? _sb.ToString() : string.Empty;
        }

        public void RecalculateDurability(Rarity rarity)
        {
            var fishingRod =
                _character.Backpack.BackpackItems.FirstOrDefault(p =>
                    p.IsEquipped && p.ItemType == ItemType.FishingRod);
            var bait = _character.Backpack.BackpackItems.FirstOrDefault(p =>
                p.IsEquipped && p.ItemType == ItemType.Bait);
            
            fishingRod?.ReCalcDurability(rarity);
            bait?.ReCalcDurability(rarity);
        }
    }
}