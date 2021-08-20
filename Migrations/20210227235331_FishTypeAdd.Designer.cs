﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TelegramAspBot.Models;

namespace TelegramAspBot.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20210227235331_FishTypeAdd")]
    partial class FishTypeAdd
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Backpack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.HasKey("Id");

                    b.ToTable("Backpacks");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.BackpackItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int?>("BackpackId")
                        .HasColumnType("integer");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<int>("Durability")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsEquipped")
                        .HasColumnType("boolean");

                    b.Property<int?>("ItemId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeElapsed")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("BackpackId");

                    b.HasIndex("ItemId");

                    b.ToTable("BackpackItems");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.CharStat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("BaitUsingCount")
                        .HasColumnType("integer");

                    b.Property<int>("EventFishCount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("FirstEventFishCatchTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("FishCaughtCount")
                        .HasColumnType("integer");

                    b.Property<int>("FishOfMyDreams")
                        .HasColumnType("integer");

                    b.Property<int>("FishingTry")
                        .HasColumnType("integer");

                    b.Property<int>("HookCount")
                        .HasColumnType("integer");

                    b.Property<double>("MostHeightFish")
                        .HasColumnType("double precision");

                    b.Property<double>("MostWeightFish")
                        .HasColumnType("double precision");

                    b.Property<int>("Percent")
                        .HasColumnType("integer");

                    b.Property<int>("SeasonFishCaughtCount")
                        .HasColumnType("integer");

                    b.Property<int>("SeasonFishingTry")
                        .HasColumnType("integer");

                    b.Property<int>("SeasonHookCount")
                        .HasColumnType("integer");

                    b.Property<double>("SeasonMostHeightFish")
                        .HasColumnType("double precision");

                    b.Property<string>("SeasonMostHeighеName")
                        .HasColumnType("text");

                    b.Property<double>("SeasonMostWeightFish")
                        .HasColumnType("double precision");

                    b.Property<string>("SeasonMostWeightName")
                        .HasColumnType("text");

                    b.Property<int>("UnluckyTry")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("CharStat");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("ActionTimer")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Attack")
                        .HasColumnType("integer");

                    b.Property<int?>("BackpackId")
                        .HasColumnType("integer");

                    b.Property<bool>("Banned")
                        .HasColumnType("boolean");

                    b.Property<int?>("CharStatId")
                        .HasColumnType("integer");

                    b.Property<int>("CharState")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Defense")
                        .HasColumnType("integer");

                    b.Property<int?>("FishPediaId")
                        .HasColumnType("integer");

                    b.Property<Guid>("FishingSessionGuid")
                        .HasColumnType("uuid");

                    b.Property<bool>("HasStartedLure")
                        .HasColumnType("boolean");

                    b.Property<int>("HealthAll")
                        .HasColumnType("integer");

                    b.Property<int>("HealthNow")
                        .HasColumnType("integer");

                    b.Property<bool>("IsSetupNickname")
                        .HasColumnType("boolean");

                    b.Property<int>("Money")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("SeasonPoints")
                        .HasColumnType("integer");

                    b.Property<int?>("SpotId")
                        .HasColumnType("integer");

                    b.Property<int>("TelegramId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BackpackId");

                    b.HasIndex("CharStatId");

                    b.HasIndex("FishPediaId");

                    b.HasIndex("SpotId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("FishName")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.FishPedia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.HasKey("Id");

                    b.ToTable("FishPedias");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.FishPediaInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("Caught")
                        .HasColumnType("integer");

                    b.Property<int?>("FishId")
                        .HasColumnType("integer");

                    b.Property<int?>("FishPediaId")
                        .HasColumnType("integer");

                    b.Property<double>("MaxHeight")
                        .HasColumnType("double precision");

                    b.Property<double>("MaxWeight")
                        .HasColumnType("double precision");

                    b.Property<int>("Seen")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FishId");

                    b.HasIndex("FishPediaId");

                    b.ToTable("FishPediaInfos");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.FishReferenceSpot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int?>("FishReferenceId")
                        .HasColumnType("integer");

                    b.Property<int?>("SpotId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FishReferenceId");

                    b.HasIndex("SpotId");

                    b.ToTable("FishReferenceSpots");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.GlobalSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("LureTimeMinutes")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("GlobalSettings");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<double>("CatchBonus")
                        .HasColumnType("double precision");

                    b.Property<int>("FishBiteType")
                        .HasColumnType("integer");

                    b.Property<double>("Height")
                        .HasColumnType("double precision");

                    b.Property<double>("HookBonus")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsEvent")
                        .HasColumnType("boolean");

                    b.Property<int>("ItemType")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PlayerPrice")
                        .HasColumnType("integer");

                    b.Property<int>("Rarity")
                        .HasColumnType("integer");

                    b.Property<int>("ShopPrice")
                        .HasColumnType("integer");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Lure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("CharacterId")
                        .HasColumnType("integer");

                    b.Property<int?>("ItemId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LureTimeLeft")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId")
                        .IsUnique();

                    b.HasIndex("ItemId");

                    b.ToTable("Lures");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("IsEnded")
                        .HasColumnType("boolean");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("SeasonStats");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Spot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<double>("CatchBonus")
                        .HasColumnType("double precision");

                    b.Property<int?>("FishReferenceId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<double>("Temperature")
                        .HasColumnType("double precision");

                    b.Property<int>("WeatherType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FishReferenceId");

                    b.ToTable("Spots");
                });

            modelBuilder.Entity("TelegramAspBot.Models.FishReference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("FishType")
                        .HasColumnType("integer");

                    b.Property<bool>("IsEvent")
                        .HasColumnType("boolean");

                    b.Property<double>("MaxHeight")
                        .HasColumnType("double precision");

                    b.Property<double>("MaxWeight")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Rarity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("FishReferences");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.BackpackItem", b =>
                {
                    b.HasOne("TelegramAspBot.Models.Entity.Backpack", null)
                        .WithMany("BackpackItems")
                        .HasForeignKey("BackpackId");

                    b.HasOne("TelegramAspBot.Models.Entity.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Character", b =>
                {
                    b.HasOne("TelegramAspBot.Models.Entity.Backpack", "Backpack")
                        .WithMany()
                        .HasForeignKey("BackpackId");

                    b.HasOne("TelegramAspBot.Models.Entity.CharStat", "CharStat")
                        .WithMany()
                        .HasForeignKey("CharStatId");

                    b.HasOne("TelegramAspBot.Models.Entity.FishPedia", "FishPedia")
                        .WithMany()
                        .HasForeignKey("FishPediaId");

                    b.HasOne("TelegramAspBot.Models.Entity.Spot", "Spot")
                        .WithMany("Characters")
                        .HasForeignKey("SpotId");

                    b.Navigation("Backpack");

                    b.Navigation("CharStat");

                    b.Navigation("FishPedia");

                    b.Navigation("Spot");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.FishPediaInfo", b =>
                {
                    b.HasOne("TelegramAspBot.Models.Entity.Item", "Fish")
                        .WithMany()
                        .HasForeignKey("FishId");

                    b.HasOne("TelegramAspBot.Models.Entity.FishPedia", null)
                        .WithMany("FishPediaInfoList")
                        .HasForeignKey("FishPediaId");

                    b.Navigation("Fish");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.FishReferenceSpot", b =>
                {
                    b.HasOne("TelegramAspBot.Models.FishReference", "FishReference")
                        .WithMany()
                        .HasForeignKey("FishReferenceId");

                    b.HasOne("TelegramAspBot.Models.Entity.Spot", null)
                        .WithMany("FishReferenceSpots")
                        .HasForeignKey("SpotId");

                    b.Navigation("FishReference");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Lure", b =>
                {
                    b.HasOne("TelegramAspBot.Models.Entity.Character", "Character")
                        .WithOne("Lure")
                        .HasForeignKey("TelegramAspBot.Models.Entity.Lure", "CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TelegramAspBot.Models.Entity.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId");

                    b.Navigation("Character");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Spot", b =>
                {
                    b.HasOne("TelegramAspBot.Models.FishReference", null)
                        .WithMany("Spots")
                        .HasForeignKey("FishReferenceId");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Backpack", b =>
                {
                    b.Navigation("BackpackItems");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Character", b =>
                {
                    b.Navigation("Lure");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.FishPedia", b =>
                {
                    b.Navigation("FishPediaInfoList");
                });

            modelBuilder.Entity("TelegramAspBot.Models.Entity.Spot", b =>
                {
                    b.Navigation("Characters");

                    b.Navigation("FishReferenceSpots");
                });

            modelBuilder.Entity("TelegramAspBot.Models.FishReference", b =>
                {
                    b.Navigation("Spots");
                });
#pragma warning restore 612, 618
        }
    }
}
