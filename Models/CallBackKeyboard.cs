using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class CallBackKeyboard
    {
        public static InlineKeyboardMarkup GetKeyboardFish(Character character, Guid sessionGuid)
        {
            //Log.Info($"Получил запрос колбэка от {character}");
            
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                // first row
                new []
                {
                    InlineKeyboardButton.WithCallbackData("ПОДСЕКАЮ", $"hook_{character.TelegramId} session_{sessionGuid}")
                }
            });

            return inlineKeyboard;
        }
        
        public static InlineKeyboardMarkup GetShopKeyboard(Character character)
        {
            //Log.Info($"Получил запрос колбэка от {character}");
            
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                // first row
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Хищная", $"shopCarni"),
                    InlineKeyboardButton.WithCallbackData("Нехищная", $"shopHerb"),
                    InlineKeyboardButton.WithCallbackData("Комбо", $"shopCombo")
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Подкормка", $"shopLure"),
                    InlineKeyboardButton.WithCallbackData("Удочки", $"shopFishigRod")
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Продать девайсы", $"shopSellDevices"),
                    InlineKeyboardButton.WithCallbackData("Продать рыбу", $"shopSellFish")
                },
            });

            return inlineKeyboard;
        }

        public static InlineKeyboardMarkup GetFishKeyboard(List<BackpackItem> fishList, int pagePointer)
        {
            if (fishList.Count <= 20) return null;
            var pages = (fishList.Count / 20) + 1;
            InlineKeyboardMarkup inlineKeyboard;
            if (pagePointer == 1)
            {
                inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        //следующая страница
                        InlineKeyboardButton.WithCallbackData($">>{pagePointer + 1} стр.", $"page_fish_{pagePointer + 1}")
                    },
                });
                return inlineKeyboard;
            }

            if (pagePointer == pages)
            {
                inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        //предыдущая страница
                        InlineKeyboardButton.WithCallbackData($"<<{pagePointer - 1} стр.", $"page_fish_{pagePointer - 1}")
                    },
                });
                return inlineKeyboard;
            }
            
            inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData($"<<{pagePointer - 1} стр.", $"page_fish_{pagePointer - 1}"),
                    InlineKeyboardButton.WithCallbackData($">>{pagePointer + 1} стр.", $"page_fish_{pagePointer + 1}")
                },
            });
            return inlineKeyboard;
        }
    }
}