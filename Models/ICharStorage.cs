using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public interface ICharStorage
    {
        public Character GetCharacter(int telegramId);
        public void SetCharacter(Character character);
    }
}