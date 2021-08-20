using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TelegramAspBot.Models.Entity
{
    public class FishPediaInfo
    {
        [Key]
        public int Id { get; set; }
        public virtual Item Fish { get; set; }
        public string Name { get; set; }
        public Rarity Rarity { get; set; }
        public FishType FishType { get; set; }
        public int Caught { get; set; } = 0;
        public int Seen { get; set; } = 0;
        public double MaxWeight { get; set; } = 0.0;
        public double MaxHeight { get; set; } = 0.0;

        public void UpdateWeight(double weight)
        {
            if (weight > MaxWeight)
            {
                MaxWeight = weight;
            }
        }
        
        public void UpdateHeight(double height)
        {
            if (height > MaxHeight)
            {
                MaxHeight = height;
            }
        }

        public void PlusCaught()
        {
            Caught++;
            Seen++;
        }

        public void PlusRunAway()
        {
            Seen++;
        }
    }
}