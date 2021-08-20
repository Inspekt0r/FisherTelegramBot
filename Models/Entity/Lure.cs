using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramAspBot.Models.Entity
{
    public class Lure
    {
        [Key]
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public virtual Character Character { get; set; }
        public virtual BackpackItem Item { get; set; }
        public DateTime LureTimeLeft { get; set; }
    }
}