using System;
using System.ComponentModel.DataAnnotations;
using TelegramAspBot.Models.Enum;

namespace TelegramAspBot.Models.Entity
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int TelegramId { get; set; }
        public virtual Backpack Backpack { get; set; }
        public int HealthNow { get; set; }
        public int HealthAll { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public State CharState { get; set; } = State.Idle;
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        public DateTime ActionTimer { get; set; } = DateTime.UtcNow;
        public bool IsSetupNickname { get; set; } = false;
        public Guid FishingSessionGuid { get; set; }
        public virtual CharStat CharStat { get; set; }
        public virtual FishPedia FishPedia { get; set; }
        public int Money { get; set; } = 1000;
        public bool Banned { get; set; } = false;
        public virtual Lure Lure { get; set; }
        /// <summary>
        /// Флаг была ли стартовая помощь в виде приманки или нет
        /// </summary>
        public bool HasStartedLure { get; set; } = false;
        
        public virtual Spot Spot { get; set; }
        public int SeasonPoints { get; set; }
        public bool SendPersonalAlert { get; set; } = true;

        public override string ToString()
        {
            return $"{TelegramId}/{Name}";
        }
    }
}