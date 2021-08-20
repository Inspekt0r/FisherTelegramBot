using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelegramAspBot.Models;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.Enum;

namespace TelegramAspBot.Job
{
    /// <summary>
    /// Класс отвечающий за функцию окончания рыбалки
    /// </summary>
    public class JobFunction
    {
        private readonly Character _character;
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly Random _random = new Random();
        private readonly ApplicationContext _dbContext = new ApplicationContext();
        private readonly FishGenerator _fishGenerator = new FishGenerator();
        private readonly FishpediaWorker _fishpedia = new FishpediaWorker();
        private readonly JobManager _jobManager;
        public readonly List<FishTest> TestFishes = new List<FishTest>();
        private readonly bool _testFlag;
        private readonly List<FishReferenceSpot> _fishReferences;

        /// <summary>
        /// Переменная отвечающая за подсечку
        /// </summary>
        private readonly bool _fishingPushButton;

        public JobFunction(int telegramId, bool fishingPushButton, JobManager jobManager)
        {
            _character = _dbContext.Characters.FirstOrDefault(p => p.TelegramId == telegramId);
            _fishingPushButton = fishingPushButton;
            _jobManager = jobManager;
        }

        /// <summary>
        /// Конструктор тестового класса
        /// </summary>
        /// <returns></returns>
        public JobFunction(int telegramId, bool fishingPushButton, JobManager jobManager, bool test)
        {
            _character = _dbContext.Characters.FirstOrDefault(p => p.TelegramId == telegramId);
            _fishingPushButton = fishingPushButton;
            _jobManager = jobManager;
            _testFlag = test;
            _fishReferences = _character?.Spot.FishReferenceSpots.ToList();
        }

        public StringBuilder GetFishingText()
        {
            if (IsFishing())
            {
                GenerateTextForFishing();
            }
            else
            {
                //игрок не рыбачит и не подсекает, короче ничего он не получит
                _sb.Append($"Трям");
            }

            return _sb;
        }

        public void GenerateTextForFishing()
        {
            var currentEvent = _dbContext.Events.FirstOrDefault(p => p.IsActive);
            var fishingModule = new WeightedFishingModule(_fishingPushButton, _character, _jobManager.GetActiveLureJob(_character));
            var durabilitySystem = new DurabilitySystem(_character);
            
            if (_testFlag)
            {
                var testModule = new WeightedFishingModule(false, _character, _jobManager.GetActiveLureJob(_character));
                var fishNew = testModule.CatchFishTest(_fishReferences);
                if (fishNew != null)
                {
                    TestFishes.Add( new FishTest()
                    {
                        Fish = fishNew,
                        IsCatch = true
                    });
                }
                else
                {
                    TestFishes.Add( new FishTest()
                    {
                        Fish = null,
                        IsCatch = false
                    });
                }
                
                return;
            }

            var fishThatCaught = fishingModule.CatchFishOrDefault();
            if (fishThatCaught != null)
            {
                //добавляем рыбу, пишем поздравления, кидаем чепчик
                fishThatCaught.Count = 1;
                _character.Backpack.BackpackItems.Add(fishThatCaught);

                _character.CharStat.IsBiggerHeight(fishThatCaught);
                _character.CharStat.IsBiggerWeight(fishThatCaught);
                _character.CharStat.IsFishOfMyDream(fishThatCaught);

                _fishpedia.AddInfoAboutFishToCharacter(_character, fishThatCaught);

                _sb.AppendLine($"<b>Рыба поймалась на твой крючок, охуеть!</b>");
                _sb.AppendLine($"<i>{BackpackTextGenerator.GetRarityType(fishThatCaught.Rarity)} {fishThatCaught.ItemName}</i> висит на крючке");
                durabilitySystem.RecalculateDurability(fishThatCaught.Rarity);
                
                _character.CharStat.FishCaughtCount++;
                _character.CharStat.SeasonFishCaughtCount++;
                _character.CharStat.PercentCatches();

                if (currentEvent != null && fishThatCaught.ItemName == currentEvent.FishName)
                {
                    _character.CharStat.EventFishCount++;
                    if (_character.CharStat.EventFishCount == 1)
                    {
                        _character.CharStat.FirstEventFishCatchTime = DateTime.UtcNow;
                    }
                }
                    
                //todo балы сезона за редкость рыбы
                _character.SeasonPoints += SeasonPointByRarity(fishThatCaught.Rarity);
                UnluckyCounter(false);
            }
            else
            {
                _sb.Append($"Рыба соскочила, ну бывает");
                _character.CharStat.PercentCatches();
                UnluckyCounter(true);
            }
            
            _character.CharState = State.Idle;
            
            _sb.AppendLine(durabilitySystem.CheckBrokenItems());
            
            _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Функция которая возвращает поймал рыбу игрок без подсечки или нет
        /// </summary>
        /// <returns></returns>
        private bool IsCatchFishWithoutHook(Item fish)
        {
            var fishManager = new HookAndFishManager(_character, _jobManager);
            return fishManager.IsFished(0, fish);
        }

        /// <summary>
        /// Функция которая возвращает поймал рыбу игрок c подсечкой или нет
        /// </summary>
        /// <returns></returns>
        private bool IsCatchFishWithHook(Item fish)
        {
            var fishManager = new HookAndFishManager(_character, _jobManager);
            return fishManager.IsFished(GetHookBonusByFishRarity(fish.Rarity), fish);
        }
        
        private double GetHookBonusByFishRarity(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => 0.05,
                Rarity.Uncommon => 0.07,
                Rarity.Rare => 0.1,
                Rarity.Elite => 0.12,
                Rarity.Mythical => 0.15,
                Rarity.Legendary => 0.17,
                _ => 1.00
            };
        }
        
        /// <summary>
        /// Функция проверяющая рыбачит персонаж или нет
        /// </summary>
        /// <returns>True - рыбачит</returns>
        private bool IsFishing()
        {
            return _character.CharState == State.Fishing;
        }

        private void UnluckyCounter(bool isUnlucky)
        {
            if (isUnlucky)
            {
                _character.CharStat.UnluckyTry++;
            }
            else
            {
                if (_character.CharStat.UnluckyTry > 0)
                {
                    _character.CharStat.UnluckyTry = 0;
                }
            }
        }

        private static int SeasonPointByRarity(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Elite => 3,
                Rarity.Mythical => 5,
                Rarity.Legendary => 10,
                _ => 0
            };
        }
    }
}