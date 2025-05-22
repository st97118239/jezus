public enum TowerTypes
{
    ArcheryTower,
    Ballista,
    Barracks,
    SuicideBombers,
    AutismCube // remove when all towers finished
}

public static class EnumExtensionsTowerTypes
{
    public static string ToReadableString(this TowerTypes i)
    {
        return System.Text.RegularExpressions.Regex.Replace(i.ToString(), @"([a-z])([A-Z])", "$1 $2");
    }
}