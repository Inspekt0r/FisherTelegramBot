using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models.Interfaces
{
    public interface IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemType ItemType { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public Rarity Rarity { get; set; }
        public double CatchBonus { get; set; }
        public double HookBonus { get; set; }
        public FishType FishBiteType { get; set; }
        public int ShopPrice { get; set; }
        public int PlayerPrice { get; set; }
        public bool IsEvent { get; set; }
    }
}