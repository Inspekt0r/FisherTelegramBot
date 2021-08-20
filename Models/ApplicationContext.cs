using System;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        
        public DbSet<Backpack> Backpacks { get; set; }
        public DbSet<BackpackItem> BackpackItems { get; set; }
        
        public DbSet<Item> Items { get; set; }
        
        public DbSet<FishReference> FishReferences { get; set; }
        
        public DbSet<FishPedia> FishPedias { get; set; }
        public DbSet<FishPediaInfo> FishPediaInfos { get; set; }
        public DbSet<FishReferenceSpot> FishReferenceSpots { get; set; }
        
        public DbSet<Lure> Lures { get; set; }
        public DbSet<GlobalSetting> GlobalSettings { get; set; }
        
        public DbSet<Spot> Spots { get; set; }
        
        public DbSet<Season> SeasonStats { get; set; }
        
        public DbSet<Event> Events { get; set; }    
        public ApplicationContext()
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            
            optionsBuilder
                .UseNpgsql(GetConnectionString())
                .UseLazyLoadingProxies();

        }

        private static string GetConnectionString()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            return builder.ToString();
        }
    }
}
