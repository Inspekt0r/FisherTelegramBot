using System.ComponentModel.DataAnnotations;

namespace TelegramAspBot.Models.Entity
{
    public class FishReferenceSpot
    {
        [Key]
        public int Id { get; set; }
        public virtual FishReference FishReference { get; set; }
    }
}