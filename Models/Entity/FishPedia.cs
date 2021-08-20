using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TelegramAspBot.Models.Entity
{
    public class FishPedia
    {
        [Key]
        public int Id { get; set; }
        
        public virtual List<FishPediaInfo> FishPediaInfoList { get; set; } 
    }
}