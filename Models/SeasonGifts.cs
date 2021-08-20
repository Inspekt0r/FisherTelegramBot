using System;
using System.Collections.Generic;
using System.Text;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class SeasonGifts
    {
        private readonly Character _character;
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly Season _season;
        private readonly List<Character> _orderedByPoints;

        public SeasonGifts(Character character, Season season, List<Character> orderedByPoints)
        {
            _character = character;
            _season = season;
            _orderedByPoints = orderedByPoints;
        }

        public StringBuilder GetGiftForPlayer()
        {
            _sb.AppendLine($"<b>Результаты сезона #{_season.Number}</b>");
            _sb.AppendLine($"Ты заработал за этот сезон: {_character.SeasonPoints} рыбОчков");
            _sb.AppendLine($"Ты занял по результатам сезона: {PositionOnLeaderBoards()} место");
            _sb.AppendLine();
            _sb.AppendLine($"Поймал рыбы: {_character.CharStat.SeasonFishCaughtCount}");
            _sb.AppendLine($"Ты был успешен в {_character.CharStat.SeasonPercentCatches()}% случаев");
            _sb.AppendLine($"Самый длинный улов: {_character.CharStat.SeasonMostHeightFish} " +
                           $"и это был(а) {_character.CharStat.SeasonMostHeighеName}");
            _sb.AppendLine($"Самый тяжелый улов: {_character.CharStat.SeasonMostWeightFish} " +
                           $"и это был(а) {_character.CharStat.SeasonMostWeightName}");
            _sb.AppendLine();
            _sb.AppendLine($"За сезон тебе подарок: {CountMoneyForSeason()} FishCoin'ов!");

            ResetSeasonStats();
            
            return _sb;
        }

        private int CountMoneyForSeason()
        {
            var caught = _character.CharStat.SeasonFishCaughtCount;
            var hook = _character.CharStat.SeasonHookCount;
            var mostHeight = _character.CharStat.SeasonMostHeightFish;
            var mostWeight = _character.CharStat.SeasonMostWeightFish;

            var money = (caught / 10) + (hook / 20) + mostHeight * 10;

            money += MoneyRewardForSeasonPointsPosition();
            
            _character.Money += (int) money;

            return (int) money;
        }

        private void ResetSeasonStats()
        {
            _character.CharStat.SeasonFishingTry = 0;
            _character.CharStat.SeasonHookCount = 0;
            _character.CharStat.SeasonFishCaughtCount = 0;
            _character.CharStat.SeasonMostHeightFish = 0.0;
            _character.CharStat.SeasonMostHeighеName = string.Empty;
            _character.CharStat.SeasonMostWeightFish = 0.0;
            _character.CharStat.SeasonMostWeightName = string.Empty;
            _character.SeasonPoints = 0;
        }

        private int MoneyRewardForSeasonPointsPosition()
        {
            return PositionOnLeaderBoards() switch
            {
                1 => 3000,
                2 => 2000,
                3 => 1500,
                4 => 1250,
                5 => 1000,
                6 => 750,
                7 => 550,
                8 => 400,
                9 => 350,
                10 => 300,
                _ => 50
            };
        }

        private int PositionOnLeaderBoards()
        {
            var position = 1;

            foreach (var character in _orderedByPoints)
            {
                if (character.TelegramId == _character.TelegramId)
                {
                    return position;
                }

                position++;
            }

            return position;
        }
    }
}