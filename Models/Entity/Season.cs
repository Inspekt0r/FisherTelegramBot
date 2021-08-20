using System;
using System.ComponentModel.DataAnnotations;

namespace TelegramAspBot.Models.Entity
{
    public class Season
    {
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }
        public int Month { get; set; } = DateTime.UtcNow.Month;
        public bool IsEnded { get; set; }
    }
}