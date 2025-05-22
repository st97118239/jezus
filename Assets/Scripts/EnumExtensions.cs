public static class EnumExtensions
{
    public static string ToReadableString(this Upgrades upgrade)
    {
        return System.Text.RegularExpressions.Regex.Replace(upgrade.ToString(), @"([a-z])([A-Z])", "$1 $2");
    }
}