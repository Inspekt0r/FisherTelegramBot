using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.Enum;

namespace TelegramAspBot.Models
{
    public class EventGifts
    {
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly int _eventId;
        private readonly Random _random = new Random();

        public EventGifts(int eventId)
        {
            _eventId = eventId;
        }

        public string GetEventResult()
        {
            _sb.AppendLine($"<b>Подошло к концу событие #{_eventId}</b>");
            _sb.AppendLine();
            BestFivePlayer();

            return _sb.ToString();
        }

        public string GetTempResult()
        {
            _sb.AppendLine($"<b>Промежуточный результат события #{_eventId}</b>");
            _sb.AppendLine();
            TempBestTenPlayers();

            return _sb.ToString();
        }

        private void TempBestTenPlayers()
        {
            using var dbContext = new ApplicationContext();
            var participants = dbContext.Characters.Where(p => p.CharStat.EventFishCount > 0).ToList();
            if (participants.Count < 1)
            {
                _sb.AppendLine($"Пока никто не поймал нужную рыбёшку!");
                return;
            }
            
            participants = participants.OrderByDescending(p => p.CharStat.EventFishCount)
                .ThenBy(p => p.CharStat.FirstEventFishCatchTime)
                .ToList();
            
            var position = 1;
            foreach (var character in participants)
            {
                _sb.AppendLine($"{position} место: \"{character.Name}\" поймано рыбы: {character.CharStat.EventFishCount}🐠");

                if (position == 10)
                {
                    break;
                }
                position++;
            }
        }

        private void BestFivePlayer()
        {
            using var dbContext = new ApplicationContext();
            var participants = dbContext.Characters.Where(p => p.CharStat.EventFishCount > 0).ToList();
            if (participants.Count < 1)
            {
                _sb.AppendLine($"К сожалению участников оказалось недостаточно, ждите следующий эвент");
                return;
            }
            
            participants = participants.OrderByDescending(p => p.CharStat.EventFishCount)
                .ThenBy(p => p.CharStat.FirstEventFishCatchTime)
                .ToList();

            if (EventSystem.IsItemsEvent(DateTime.Now))
            {
                _sb.AppendLine($"Лучшие 5 игроков получают предметы:");
                var position = 1;
                var itemReference = dbContext.Items.Where(p=> p.IsEvent).ToList();
                foreach (var character in participants)
                {
                    _sb.AppendLine($"{position} место: \"{character.Name}\" с результатом: {character.CharStat.EventFishCount}🐠\n" +
                                   $"Получает {SetItemByPosition(position, character, itemReference)}!");
                    character.SeasonPoints += GetSpByPosition(position);
                    if (position == 5)
                    {
                        break;
                    }
                
                    position++;
                } 
            }
            else
            {
                _sb.AppendLine($"Лучшие 5 игроков получают sP!");
            
                var position = 1;
                foreach (var character in participants)
                {
                    _sb.AppendLine($"{position} место: \"{character.Name}\" с результатом: {character.CharStat.EventFishCount}🐠\nПолучает {GetSpByPosition(position)} sP!");
                    character.SeasonPoints += GetSpByPosition(position);
                    if (position == 5)
                    {
                        break;
                    }
                
                    position++;
                }   
            }

            ResetAllEventResult(participants);
            
            dbContext.SaveChanges();
        }

        private static void ResetAllEventResult(List<Character> participants)
        {
            participants.ForEach(p => p.CharStat.EventFishCount = 0);
        }

        private string SetItemByPosition(int position, Character character, IReadOnlyList<Item> itemReferences)
        {
            var itemReference = itemReferences
                .Where(p => p.EventPosition == GetEventPosition(position))
                .ToList();
            var randomItem = itemReference[_random.Next(0, itemReference.Count)];
            character.Backpack.BackpackItems.Add(randomItem.ConvertToBackpack());

            return randomItem.Name;
        }

        private static EventPosition GetEventPosition(int position)
        {
            return position switch
            {
                1 => EventPosition.First,
                2 => EventPosition.Second,
                3 => EventPosition.Third,
                _ => EventPosition.Other
            };
        }
        private static int GetSpByPosition(int position)
        {
            return position switch
            {
                1 => 15,
                2 => 12,
                3 => 10,
                4 => 8,
                5 => 5,
                _ => 0
            };
        }
    }
}