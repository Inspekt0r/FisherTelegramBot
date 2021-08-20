using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TelegramAspBot.Models.Entity
{
    public class Backpack
    {
        [Key]
        public int Id { get; set; }
        public virtual List<BackpackItem> BackpackItems { get; set; }
    }
}