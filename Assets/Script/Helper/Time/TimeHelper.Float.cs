using SRF;
using System;

namespace Helper.Time
{
    public partial class TimeHelper
    {
        /// <summary>
        /// 남은 시간을 표현하는 포멧 (언어별로 번역함)
        /// (형식: dd일hh시, hh시mm분, mm분ss초, ss초)
        /// </summary>
        /// <param name="duration">전체 시간 (초)</param>
        /// <param name="isShowZero">30분0초 -> 30분, 1시간0분 -> 1시간의 값으로 표시할지 여부를 결정하는 값</param>
        /// <param name="useDaysThreshold">48시간을 기준으로 그 이상부터 Day, Hour를 사용할지 여부를 결정하는 값</param>
        /// <returns></returns>
        public static string GetFormattedTime(float duration, bool isShowZero = true, bool useDaysThreshold = true)
        {
            if (duration.ApproxZero() || !(duration > 0f))
                return string.Empty;

            (int days, int hours, int minutes, int seconds) = _GetSplitTime(duration);

            if (!useDaysThreshold)
            {
                if (days > 0)
                {
                    if (!isShowZero && hours == 0)
                        return $"{string.Format(CommonHelper.Translation(TIME_D), days)}";
                    return $"{string.Format(CommonHelper.Translation(TIME_D), days)} {string.Format(CommonHelper.Translation(TIME_H), hours)}";
                }

                if (hours > 0)
                {
                    if (!isShowZero && minutes == 0)
                        return $"{string.Format(CommonHelper.Translation(TIME_H), (days * 24) + hours)}";
                    return $"{string.Format(CommonHelper.Translation(TIME_H), (days * 24) + hours)} {string.Format(CommonHelper.Translation(TIME_M), minutes)}";
                }
            }
            else
            {
                if (days >= 2)
                {
                    if (!isShowZero && hours == 0)
                        return $"{string.Format(CommonHelper.Translation(TIME_D), days)}";
                    return $"{string.Format(CommonHelper.Translation(TIME_D), days)} {string.Format(CommonHelper.Translation(TIME_H), hours)}";
                }

                if (days > 0 || hours > 0)
                {
                    if (!isShowZero && minutes == 0)
                        return $"{string.Format(CommonHelper.Translation(TIME_H), (days * 24) + hours)}";
                    return $"{string.Format(CommonHelper.Translation(TIME_H), (days * 24) + hours)} {string.Format(CommonHelper.Translation(TIME_M), minutes)}";
                }   
            }

            if (minutes > 0)
            {
                if (!isShowZero && seconds == 0)
                    return $"{string.Format(CommonHelper.Translation(TIME_M), minutes)}";
                return $"{string.Format(CommonHelper.Translation(TIME_M), minutes)} {string.Format(CommonHelper.Translation(TIME_S), seconds)}";
            }

            return seconds >= 0 ? $"{string.Format(CommonHelper.Translation(TIME_S), seconds)}" : string.Empty;
        }
        
        // 남은 시간을 표현하는 포멧 (콜론으로 구분)
        // (형식: dd:hh, hh:mm, mm:ss, 00:ss)
        public static string GetFormattedTimeWithColons(float duration)
        {
            if (duration.ApproxZero() || !(duration > 0f))
                return string.Empty;

            (int days, int hours, int minutes, int seconds) = _GetSplitTime(duration);
            if (days > 0 || hours > 0)
                return string.Format(CommonHelper.Translation(TIME_DOUBLE_DIGIT), (days * 24) + hours, minutes);
            
            if (minutes > 0)
                return string.Format(CommonHelper.Translation(TIME_DOUBLE_DIGIT), minutes, seconds);
            
            if (seconds >= 0)
                return string.Format(CommonHelper.Translation(TIME_DOUBLE_DIGIT), 0, seconds);
            
            return string.Empty;
        }
        
        private static Tuple<int, int, int, int> _GetSplitTime(float duration)
        {
            // 소수점 이하의 시간은 필요하지 않기 때문에 int형으로 형변환
            return _GetSplitTime((int)duration);
        }
    }
}