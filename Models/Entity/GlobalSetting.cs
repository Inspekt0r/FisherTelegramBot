using System;
using System.ComponentModel.DataAnnotations;

namespace TelegramAspBot.Models.Entity
{
    public class GlobalSetting
    {
        [Key]
        public int Id { get; set; }
        public int LureTimeMinutes { get; set; } = 30;
    }
}