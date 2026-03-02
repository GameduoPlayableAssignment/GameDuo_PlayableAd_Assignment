using System.Numerics;

public static partial class ExtensionMethod
{
    public static string ToShortString(this decimal value, int decimalPlaces = 3)
    {
        switch (decimalPlaces)
        {
            case 2: return string.Format(UI_FORMAT_WITH_TWO_DIGIT, value);
            case 3: return string.Format(UI_FORMAT_WITH_THREE_DIGIT, value);
        }
        return string.Format(UI_FORMAT_WITH_TWO_DIGIT, value);
    }
    
    public static string ToShortStringMultipliedBy100(this decimal value)
    {
        return string.Format(UI_FORMAT_WITH_THREE_DIGIT, value * 100);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="isBigIntegerFormat">이 값이 true일 경우, 1000%를 1A%로 표시하게 됨 (추가 예시로, 23000000% 일 경우에는 23B%로 표시)</param>
    /// <returns></returns>
    public static string ToShortPercentString(this decimal value, bool isBigIntegerFormat = true)
    {
        return string.Format(UI_FORMAT_WITH_THREE_DIGIT_PERCENT, value >= 1000 && isBigIntegerFormat ? new BigInteger(value).ToShortString() : value);
    }
}