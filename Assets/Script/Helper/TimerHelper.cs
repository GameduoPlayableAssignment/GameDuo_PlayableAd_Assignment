namespace Helper
{
    public class TimerHelper
    {
        private const string TimerKeyFormat = "{0}_{1}";
        
        // (s) 중요! 라이브 서비스 이후에 절대 바꾸면 안되는 값
        public const string PrefixPackageTimerItemRunningKey = "PTIRK";
        public const string PrefixPackageTimerItemCooldownKey = "PTICK";
        public const string PrefixTimestampTimerItemKey = "TTIK";
        public const string PrefixDefaultTimerItemKey = "DTIK";
        public const string PrefixRealtimeTimerItemKey = "RTIK";
        // (e) 중요! 라이브 서비스 이후에 절대 바꾸면 안되는 값
        
        public static string GetPackageRunningKey(int packageIdx)
        {
            // return $"{PrefixPackageRunningTimerKey}_{packageIdx}";
            return string.Format(TimerKeyFormat, PrefixPackageTimerItemRunningKey, packageIdx);
        }
        
        public static string GetPackageCooldownKey(int packageIdx)
        {
            // return $"{PrefixPackageCooldownTimerKey}_{packageIdx}";
            return string.Format(TimerKeyFormat, PrefixPackageTimerItemCooldownKey, packageIdx);
        }
        
        public static string GetDoubleTimerKey(string timerKey)
        {
            // return $"{PrefixTimestampTimerItemKey}_{timerKey}";
            return string.Format(TimerKeyFormat, PrefixTimestampTimerItemKey, timerKey);
        }
        
        public static string GetDefaultTimerItemKey(string timerKey)
        {
            // return $"{PrefixDefaultTimerItemKey}_{timerKey}";
            return string.Format(TimerKeyFormat, PrefixDefaultTimerItemKey, timerKey);
        }
        
        public static string GetRealtimeTimerItemKey(string timerKey)
        {
            // return $"{PrefixRealtimeTimerItemKey}_{timerKey}";
            return string.Format(TimerKeyFormat, PrefixRealtimeTimerItemKey, timerKey);
        }
    }
}