using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class DateExtensions
{
    // 固定祝日の定義
    private static readonly Dictionary<(int month, int day), string> FixedHolidays = new()
    {
        { (1, 1),   "元日" },
        { (2, 11),  "建国記念の日" },
        { (2, 23),  "天皇誕生日" },
        { (4, 29),  "昭和の日" },
        { (5, 3),   "憲法記念日" },
        { (5, 4),   "みどりの日" },
        { (5, 5),   "こどもの日" },
        { (8, 11),  "山の日" },
        { (11, 3),  "文化の日" },
        { (11, 23), "勤労感謝の日" }
    };

    // ハッピーマンデーの定義 (月, 第n月曜日)
    private static readonly Dictionary<(int month, int nth), string> HappyMondays = new()
    {
        { (1, 2),  "成人の日" },
        { (7, 3),  "海の日" },
        { (9, 3),  "敬老の日" },
        { (10, 2), "スポーツの日" }
    };

    /// <summary>
    /// yyyy/MM/dd HH:mm:ss 形式の文字列に変換して返す
    /// </summary>
    public static string ToPattern(this DateTime self)
    {
        return self.ToString("yyyy/MM/dd HH:mm:ss");
    }

    /// <summary>
    /// yyyy/MM/dd 形式の文字列に変換して返す
    /// </summary>
    public static string ToShortDatePattern(this DateTime self)
    {
        return self.ToString("yyyy/MM/dd");
    }

    /// <summary>
    /// yyyy年M月d日 形式の文字列に変換して返す
    /// </summary>
    public static string ToLongDatePattern(this DateTime self)
    {
        return self.ToString("yyyy年M月d日");
    }

    /// <summary>
    /// yyyy年M月d日 HH:mm:ss 形式の文字列に変換して返す
    /// </summary>
    public static string ToFullDateTimePattern(this DateTime self)
    {
        return self.ToString("yyyy年M月d日 HH:mm:ss");
    }

    /// <summary>
    /// HH:mm 形式の文字列に変換して返す
    /// </summary>
    public static string ToShortTimePattern(this DateTime self)
    {
        return self.ToString("HH:mm");
    }

    /// <summary>
    /// HH:mm:ss 形式の文字列に変換して返す
    /// </summary>
    public static string ToLongTimePattern(this DateTime self)
    {
        return self.ToString("HH:mm:ss");
    }

    /// <summary> 曜日を日本語(月〜日)で返す </summary>
    public static string GetDayOfWeekJp(this DateTime self)
    {
        return self.ToString("ddd", new CultureInfo("ja-JP"));
    }

    /// <summary> 曜日を日本語のカッコ付き((月)〜(日))で返す </summary>
    public static string GetDayOfWeekBracketJp(this DateTime self)
    {
        return $"({self.GetDayOfWeekJp()})";
    }

    /// <summary> yyyy/MM/dd(曜) 形式の文字列を返す </summary>
    public static string ToShortDateWithDayPattern(this DateTime self)
    {
        return self.ToString($"yyyy/MM/dd({self.GetDayOfWeekJp()})");
    }

    /// <summary> 曜日を英語(Sunday〜Saturday)で返す </summary>
    public static string GetDayOfWeekEn(this DateTime self)
    {
        return self.DayOfWeek.ToString();
    }

    /// <summary> 曜日を英語の略称(Sun〜Sat)で返す </summary>
    public static string GetDayOfWeekShortEn(this DateTime self)
    {
        return self.ToString("ddd", CultureInfo.InvariantCulture);
    }

    //// <summary>
    /// 日本の祝日名を返す（祝日でない場合は null）
    /// </summary>
    /// <param name="substituteFormat">振替休日だった場合の接尾辞などのフォーマット</param>
    public static string GetHolidayName(this DateTime self, string substituteFormat = "（振替）")
    {
        var dateKey = (self.Month, self.Day);

        // 1. 固定祝日の判定
        if (FixedHolidays.TryGetValue(dateKey, out var name)) return name;

        // 2. ハッピーマンデーの判定
        if (self.DayOfWeek == DayOfWeek.Monday)
        {
            int nth = (self.Day - 1) / 7 + 1;
            if (HappyMondays.TryGetValue((self.Month, nth), out var happyName)) return happyName;
        }

        // 3. 春分・秋分の日の判定
        if (self.Month == 3 && self.Day == CalculateVernalEquinox(self.Year)) return "春分の日";
        if (self.Month == 9 && self.Day == CalculateAutumnalEquinox(self.Year)) return "秋分の日";

        // 4. 振替休日の判定
        if (self.DayOfWeek == DayOfWeek.Monday)
        {
            // 前日の祝日名を取得
            var yesterdayHoliday = GetHolidayName(self.AddDays(-1), null);
            if (yesterdayHoliday != null)
            {
                // 元の祝日名に（振替）を付けて返す
                return $"{yesterdayHoliday}{substituteFormat}";
            }
        }

        return null;
    }

    /// <summary> 日本の祝日かどうかを返す </summary>
    public static bool IsHolidayJp(this DateTime self) => GetHolidayName(self) != null;

    #region Equinox Calculation (Astronomical calculations)

    private static int CalculateVernalEquinox(int year)
    {
        if (year < 1900 || year > 2099) return 0;
        double baseVal = year <= 1979 ? 20.8357 : 20.8431;
        int leapShift = year <= 1979 ? (year - 1983) / 4 : (year - 1980) / 4;
        return (int)(baseVal + 0.242194 * (year - 1980) - leapShift);
    }

    private static int CalculateAutumnalEquinox(int year)
    {
        if (year < 1900 || year > 2099) return 0;
        double baseVal = year <= 1979 ? 23.2588 : 23.2488;
        int leapShift = year <= 1979 ? (year - 1983) / 4 : (year - 1980) / 4;
        return (int)(baseVal + 0.242194 * (year - 1980) - leapShift);
    }

    #endregion
}
