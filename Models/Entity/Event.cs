using System.ComponentModel.DataAnnotations;

namespace TelegramAspBot.Models.Entity
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string FishName { get; set; }
        public bool IsActive { get; set; }
    }
}