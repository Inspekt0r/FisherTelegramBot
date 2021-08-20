using System.Collections.Generic;
using System.Linq;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class CharStorage : ICharStorage
    {
        private readonly List<Character> _characters;

        public CharStorage()
        {
            _characters = new List<Character>();
        }

        public Character GetCharacter(int telegramId)
        {
            return _characters.FirstOrDefault(p => p.TelegramId == telegramId);
        }

        public void SetCharacter(Character character)
        {
            var charExist = _characters.FirstOrDefault(p => p.TelegramId == character.TelegramId);
            if (charExist == null)
            {
                _characters.Add(character);
            }
            else
            {
                var index = _characters.IndexOf(_characters.FirstOrDefault(p => p.TelegramId == character.TelegramId));
                _characters[index] = character;
            }
        }
    }
}