using Manager.Time;
using System;

namespace Helper.Time
{
    public partial class TimeHelper
    {
        // 남은 시간을 표현하는 포멧 (언어별로 번역함)
        // (형식: dd일hh시, hh시mm분, mm분ss초, ss초)
        public static string GetFormattedTime(double endTimestampDouble)
        {
            if (endTimestampDouble - TimeManager.Instance.CurrentTimestampDouble <= 0)
                return string.Empty;

            (int days, int hours, int minutes, int seconds) = _GetSplitTime(endTimestampDouble);
            
            if (days >= 2)
                return $"{string.Format(CommonHelper.Translation(TIME_D), days)} {string.Format(CommonHelper.Translation(TIME_H), hours)}";
                
            if (days > 0 || hours > 0)
                return $"{string.Format(CommonHelper.Translation(TIME_H), (days * 24) + hours)} {string.Format(CommonHelper.Translation(TIME_M), minutes)}";
                
            if (minutes > 0)
                return $"{string.Format(CommonHelper.Translation(TIME_M), minutes)} {string.Format(CommonHelper.Translation(TIME_S), seconds)}";
                
            if (seconds >= 0)
                return $"{string.Format(CommonHelper.Translation(TIME_S), seconds)}";
            
            return string.Empty;
        }
        
        // 남은 시간을 표현하는 포멧 (콜론으로 구분)
        // (형식: dd:hh, hh:mm, mm:ss, 00:ss)
        public static string GetFormattedTimeWithColons(double endTimestampDouble)
        {
            if (endTimestampDouble - TimeManager.Instance.CurrentTimestampDouble <= 0)
                return string.Empty;

            (int days, int hours, int minutes, int seconds) = _GetSplitTime(endTimestampDouble);
            
            if (days > 0 || hours > 0)
                return string.Format(CommonHelper.Translation(TIME_DOUBLE_DIGIT), (days * 24) + hours, minutes);
                
            if (minutes > 0)
                return string.Format(CommonHelper.Translation(TIME_DOUBLE_DIGIT), minutes, seconds);
                
            if (seconds >= 0)
                return string.Format(CommonHelper.Translation(TIME_DOUBLE_DIGIT), 0, seconds);
            
            return string.Empty;
        }
        
        private static Tuple<int, int, int, int> _GetSplitTime(double endTimestampDouble)
        {
            // 남은 시간은 int형으로 충분히 표현 가능하다고 가정하고 이렇게 구성함
            return _GetSplitTime((int)(endTimestampDouble - TimeManager.Instance.CurrentTimestampDouble));
        }
        
        /// Timestamp 값과 현재 시간 기준으로 남은 시간이 존재하는지 판단
        public static bool HasRemainTime(double endTimestampDouble)
        {
            if (endTimestampDouble <= 0) // 예외 처리
                return false;
            
            return endTimestampDouble - TimeManager.Instance.CurrentTimestampDouble > 0;
        }

        /// <summary>
        /// 입력 파라미터의 타임스탬프 기준, 일종의 내림 계산하여 "자정"을 만듦
        /// (ex. 2003년 4월 3일 13시 23분 -> 2003년 4월 3일 00시 00분)
        /// </summary>
        public static long RoundDown(double startTimestamp)
        {
            if (DEBUG_ONE_DAY_IS_ONE_MINUTE)
            {
                DateTime dt = ((long)startTimestamp).ToDateTime();
                dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, DateTimeKind.Utc);
                return ((DateTimeOffset)dt).ToUnixTimeSeconds();    
            }
            else
            {
                DateTime dt = ((long)startTimestamp).ToDateTime();
                dt = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, DateTimeKind.Utc);
                return ((DateTimeOffset)dt).ToUnixTimeSeconds();   
            }
        }
        
        /// <summary>
        /// 입력 파라미터의 타임스탬프 기준, 일종의 내림 계산하여 "하루 뒤 자정"을 만듦
        /// (ex. 2003년 4월 3일 13시 23분 -> 2003년 4월 4일 00시 00분)
        /// </summary>
        public static double RoundUp(double startTimestampDouble)
        {
            if (DEBUG_ONE_DAY_IS_ONE_MINUTE)
            {
                DateTime dt = ((long)startTimestampDouble).ToDateTime();
                dt = dt.AddMinutes(1);
                dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, DateTimeKind.Utc);
                return ((DateTimeOffset)dt).ToUnixTimeSeconds();
            }
            else
            {
                DateTime dt = ((long)startTimestampDouble).ToDateTime();
                dt = dt.AddDays(1);
                dt = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, DateTimeKind.Utc);
                return ((DateTimeOffset)dt).ToUnixTimeSeconds();   
            }
        }

        /// <summary>
        /// 현재 타임스탬프 기준, 일종의 내림 계산하여 "하루 뒤 자정"을 만듦
        /// (ex. 2003년 4월 3일 13시 23분 -> 2003년 4월 4일 00시 00분)
        /// </summary>
        public static double RoundUpFromCurrentDouble()
        {
            return RoundUp(TimeManager.Instance.CurrentTimestampDouble);
        }
    }
}