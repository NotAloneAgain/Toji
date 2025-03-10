﻿using UnityEngine;

namespace Toji.Global
{
    public static class ParseExtensions
    {
        public static string GetSecondsString(this double seconds) => Mathf.RoundToInt((float)seconds).GetSecondsString();

        public static string GetSecondsString(this int seconds)
        {
            var secondsInt = seconds;

            var secondsString = secondsInt switch
            {
                int n when n % 100 is >= 11 and <= 14 => "секунд",
                int n when n % 10 == 1 => "секунда",
                int n when n % 10 is >= 2 and <= 4 => "секунды",
                _ => "секунд"
            };

            return $"{secondsInt} {secondsString}";
        }

        public static string GetMinutesString(this double minutes) => Mathf.RoundToInt((float)minutes).GetMinutesString();

        public static string GetMinutesString(this int minutes)
        {
            var minutesInt = minutes;

            var minutesString = minutesInt switch
            {
                int n when n % 100 is >= 11 and <= 14 => "минут",
                int n when n % 10 == 1 => "минута",
                int n when n % 10 is >= 2 and <= 4 => "минуты",
                _ => "минут"
            };

            return $"{minutesInt} {minutesString}";
        }

        public static string GetPlayersString(this int players)
        {
            var playersInt = players;

            var playersString = playersInt switch
            {
                int n when n % 100 is >= 11 and <= 14 => "игроков",
                int n when n % 10 == 1 => "игрок",
                int n when n % 10 is >= 2 and <= 4 => "игрока",
                _ => "игроков"
            };

            return $"{playersInt} {playersString}";
        }


        public static string ToPrefix(this string str)
        {
            if (str.StartsWith("Toji."))
            {
                str = str.Remove(0, 5);
            }

            string result = string.Empty;

            str = str.Replace("FF", "Friendly_fire").Replace("SQL", "Sql");

            bool isFirst = true;

            for (int i = 0; i < str.Length; i++)
            {
                var c = str[i];
                var prev = i == 0 ? ' ' : str[i - 1];
                //var next = i == str.Length - 1 ? ' ' : str[i + 1];

                if (!isFirst && (char.IsUpper(c) || char.IsNumber(c) && prev != ' ' && !char.IsNumber(prev)))
                {
                    result += '_';
                }

                result += char.ToLower(c);

                isFirst = false;
            }

            return result;
        }
    }
}
