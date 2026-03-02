using Helper.Time;
using System;

public static partial class ExtensionMethod
{
    public static long ToUnixTimestamp(this DateTime value)
    {
        return TimeHelper.UnixTimestampFromDateTime(value);
    }
}