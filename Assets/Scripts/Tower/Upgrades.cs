public enum Upgrades
{
    ReloadSpeed,
    AttackDamage,
    Range,
    ProjectileSpeed
}

public static class EnumExtensionsUpgrades
{
    public static string ToReadableString(this Upgrades i)
    {
        return System.Text.RegularExpressions.Regex.Replace(i.ToString(), @"([a-z])([A-Z])", "$1 $2");
    }
}