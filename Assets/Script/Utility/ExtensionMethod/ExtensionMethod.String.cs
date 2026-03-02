using Helper;
using System;
using System.Numerics;

public static partial class ExtensionMethod
{
    /// <summary>
    /// 문자열을 BigInteger로 만들 경우
    /// </summary>
    public static BigInteger ToBigInteger(this string value)
    {
        return BigIntegerHelper.Create(value);
    }
    
    
    /// <summary>
    /// 문자열 형태의 ConditionValue를 int형으로 변환
    /// </summary>
    public static int ConvertToConditionValue(this string value)
    {
        if (value == null)
            return -1;
        
        if (value.Trim().Length == 0)
            return -1;
        
        if (int.TryParse(value, out int intValue))
        {
            return intValue;
        }
        
        string[] splitValue = value.Split('.');

        try
        {
            Type enumType = Type.GetType($"Model.Enum.{splitValue[0]}");
            return Convert.ToInt32(Enum.Parse(enumType, $"{splitValue[1]}"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}