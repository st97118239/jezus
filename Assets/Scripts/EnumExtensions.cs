using System;

public static class EnumExtensions
{
    public static string ToReadableString<TEnum>(this TEnum i) where TEnum : Enum
    {
        return System.Text.RegularExpressions.Regex.Replace(i.ToString(), @"([a-z])([A-Z])", "$1 $2");
    }
}