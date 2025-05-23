public enum EnemyType
{
    Crawler,
    Demon,
    Necromancer,
    HellHound
}

public static class EnumExtensionsEnemyTypes
{
    public static string ToReadableString(this EnemyType i)
    {
        return System.Text.RegularExpressions.Regex.Replace(i.ToString(), @"([a-z])([A-Z])", "$1 $2");
    }
}