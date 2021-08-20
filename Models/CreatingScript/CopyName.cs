using System.Linq;

namespace TelegramAspBot.Models.CreatingScript
{
    public class CopyName
    {
        public static void CopyNameFromItemToBackpackItems(ApplicationContext dbContext)
        {
            var backpackItems = dbContext.BackpackItems.ToList();
            foreach (var backpackItem in backpackItems)
            {
                backpackItem.ItemName = backpackItem.Item.Name;
                backpackItem.Rarity = backpackItem.Item.Rarity;
                backpackItem.ItemType = backpackItem.Item.ItemType;
            }
            dbContext.SaveChanges();
        }

        public static void MoveItemToBackpackItem(ApplicationContext dbContext)
        {
            var backpackItems = dbContext.BackpackItems.ToList();
            foreach (var backpackItem in backpackItems)
            {
                //backpackItem.ReFillFromItem();
            }

            dbContext.SaveChanges();
        }
    }
}