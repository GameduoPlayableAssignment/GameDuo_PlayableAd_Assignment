using System.Numerics;

public static partial class ExtensionMethod
{
    public static string ToShortString(this float value)
    {
        return string.Format(UI_FORMAT_WITH_THREE_DIGIT, value);
    }

    public static string ToShortPercentString(this float value)
    {
        return string.Format(UI_FORMAT_WITH_THREE_DIGIT_PERCENT, value >= 1000 ? new BigInteger(value).ToShortString() : value);
    }
}