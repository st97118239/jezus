public enum UnitType
{
    Spearman,
    Knight,
    DaVinciTank
}

public static class EnumExtensionsUnitTypes
{
    public static string ToReadableString(this UnitType i)
    {
        return System.Text.RegularExpressions.Regex.Replace(i.ToString(), @"([a-z])([A-Z])", "$1 $2");
    }
}