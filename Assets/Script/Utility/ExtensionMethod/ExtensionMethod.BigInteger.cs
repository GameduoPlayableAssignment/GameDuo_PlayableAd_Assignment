using Helper;
using System.Numerics;
using System.Text.RegularExpressions;

public static partial class ExtensionMethod
{
    /// <summary>
    /// BigInteger * decimal 연산 이후 소수점 내림 처리
    /// </summary>
    public static BigInteger MultiplyFloor(this BigInteger bigIntegerValue, decimal doubleValue)
    {
        return BigIntegerHelper.MultiplyFloor(bigIntegerValue, doubleValue);
    }
    
    /// <summary>
    /// BigInteger * double 연산 이후 소수점 내림 처리
    /// </summary>
    public static BigInteger MultiplyFloor(this BigInteger bigIntegerValue, double doubleValue)
    {
        return BigIntegerHelper.MultiplyFloor(bigIntegerValue, doubleValue);
    }

    /// <summary>
    /// BigInteger / BigInteger 연산 결과 double (비율 구할 경우에 사용)
    /// </summary>
    public static double Divide(this BigInteger lValue, BigInteger rValue)
    {
        return BigIntegerHelper.Divide(lValue, rValue);
    }

    public static string ToShortString(this BigInteger value, int decimalPlaces = 2)
    {
        return BigIntegerHelper.ToShortString(value, decimalPlaces);
    }
    public static string GetAlphabet(this BigInteger bigIntegerValue)
    {
        string str = ToShortString(bigIntegerValue);
        return Regex.Replace(str, @"[^a-zA-Z]", "");
    }
    public static float GetValue(this BigInteger bigIntegerValue)
    {
        string str = ToShortString(bigIntegerValue);
        string Alphabet = Regex.Replace(str, @"[^a-zA-Z]", "");
        return !string.IsNullOrEmpty(Alphabet) ? float.Parse(str.Replace(Alphabet, "")) : float.Parse(str);
    }
}
