using Helper;
using Helper.Time;
using Manager.Game;
using Manager.Time;
using Message.Timer;
using Newtonsoft.Json;
using System.Collections.Generic;
using UniRx;

namespace Service.Timer
{
    public class ScaledTimer : Timer
    {
        [JsonIgnore] public string FormattedRemainDuration => TimeHelper.GetFormattedTime(RemainDuration);
        [JsonIgnore] public string FormattedRemainDurationWithColons => TimeHelper.GetFormattedTimeWithColons(RemainDuration);
        public static ScaledTimer Create(string key, float maxDuration)
        {
            return new ScaledTimer
            {
                Key = key,
                MaxDuration = maxDuration,
                RemainDuration = maxDuration,
            };
        }
    }
    
    public partial class TimerService
    {
        private void _RunScaledTimers()
        {
            List<string> removeScaledTimerKeyList = new();
            foreach (ScaledTimer scaledTimer in Cursor.ScaledTimers)
            {
                if (scaledTimer.RemainDuration <= 0)
                {
                    removeScaledTimerKeyList.Add(scaledTimer.Key);
                }
                else
                {
                    _StartScaledTimer(scaledTimer.Key);
                }
            }
            removeScaledTimerKeyList.ForEach(RemoveScaledTimer);
        }
        
        public ScaledTimer GetScaledTimer(string scaledTimerKey)
        {
            // TimeService의 Init 이전에 호출되는 부분이 있을 수 있는 부분에 대한 예외처리
            if (Cursor?.ScaledTimers == null)
                return null;
            
            int idx = Cursor.ScaledTimers.FindIndex(e => e.Key == scaledTimerKey);
            return idx < 0 ? null : Cursor.ScaledTimers[idx];
        }
        
        public void AddScaledTimer(string scaledTimerKey, float maxDuration)
        {
            if (HasScaledTimer(scaledTimerKey))
                return;
            Cursor.ScaledTimers.Add(ScaledTimer.Create(scaledTimerKey, maxDuration));
            _StartScaledTimer(scaledTimerKey);
            DebugX.Log($"[타이머 - Scaled] 등록 : {scaledTimerKey}");
            MessageBroker.Default.Publish(new OnScaledTimerAddedMessage { TimerKey = scaledTimerKey });
        }

        private void _StartScaledTimer(string scaledTimerKey)
        {
            ScaledTimer scaledTimer = GetScaledTimer(scaledTimerKey);
            if (scaledTimer == null) return;
            
            TimeManager.Instance.StartScaledTimer(TimerHelper.GetDefaultTimerItemKey(scaledTimerKey), scaledTimer.RemainDuration, (float duration) =>
            {
                scaledTimer.RemainDuration = duration;
            }, () =>
            {
                RemoveScaledTimer(scaledTimerKey);
            });
        }

        public void RemoveScaledTimer(string scaledTimerKey)
        {
            ScaledTimer scaledTimer = GetScaledTimer(scaledTimerKey);
            if (scaledTimer == null)
                return;
            
            // 패키지 삭제 이전에 상태 값은 저장해 두어야 함
            float maxDuration = GetScaledTimer(scaledTimerKey).MaxDuration;
            float currentDuration = GetScaledTimer(scaledTimerKey).RemainDuration;
            
            // 우선 아래에 있는 이벤트 발생 시키기 전에 패키지는 삭제해야 함
            Cursor.ScaledTimers.Remove(scaledTimer);
            
            // 관련 타이머가 동작하고 있을지 모르니 모두 강제 종료
            TimeManager.Instance.StopScaledTimer(TimerHelper.GetDefaultTimerItemKey(scaledTimerKey));
            
            DebugX.Log($"[타이머 - Scaled] 제거 : {scaledTimerKey}");
            MessageBroker.Default.Publish(new OnScaledTimerRemovedMessage
            {
                TimerItemKey = scaledTimerKey,
                MaxDuration = maxDuration,
                CurrentDuration = currentDuration,
            });
        }

        public bool HasScaledTimer(string scaledTimerKey)
        {
            return GetScaledTimer(scaledTimerKey) != null;
        }
    }
}