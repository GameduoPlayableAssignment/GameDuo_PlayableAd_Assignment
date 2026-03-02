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
    public class UnscaledTimer : Timer
    {
        [JsonIgnore] public string FormattedRemainDuration => TimeHelper.GetFormattedTime(RemainDuration);
        [JsonIgnore] public string FormattedRemainDurationWithColons => TimeHelper.GetFormattedTimeWithColons(RemainDuration);
        public static UnscaledTimer Create(string key, float maxDuration)
        {
            return new UnscaledTimer
            {
                Key = key,
                MaxDuration = maxDuration,
                RemainDuration = maxDuration,
            };
        }
    }
    
    public partial class TimerService
    {
        private void _RunUnscaledTimers()
        {
            List<string> removeUnscaledTimerKeyList = new();
            foreach (UnscaledTimer unscaledTimer in Cursor.UnscaledTimers)
            {
                if (unscaledTimer.RemainDuration <= 0)
                {
                    removeUnscaledTimerKeyList.Add(unscaledTimer.Key);
                }
                else
                {
                    _StartUnscaledTimer(unscaledTimer.Key);
                }
            }
            removeUnscaledTimerKeyList.ForEach(RemoveUnscaledTimer);
        }
        
        public UnscaledTimer GetUnscaledTimer(string unscaledTimerKey)
        {
            // TimeService의 Init 이전에 호출되는 부분이 있을 수 있는 부분에 대한 예외처리
            if (Cursor.UnscaledTimers == null)
                return null;
            
            int idx = Cursor.UnscaledTimers.FindIndex(e => e.Key == unscaledTimerKey);
            return idx < 0 ? null : Cursor.UnscaledTimers[idx];
        }
        
        public void AddUnscaledTimer(string unscaledTimerKey, float maxDuration)
        {
            if (HasUnscaledTimer(unscaledTimerKey))
                return;
            Cursor.UnscaledTimers.Add(UnscaledTimer.Create(unscaledTimerKey, maxDuration));
            _StartUnscaledTimer(unscaledTimerKey);
            DebugX.Log($"[타이머 - Unscaled] 등록 : {unscaledTimerKey}");
            MessageBroker.Default.Publish(new OnUnscaledTimerAddedMessage { TimerKey = unscaledTimerKey });
        }

        private void _StartUnscaledTimer(string unscaledTimerKey)
        {
            UnscaledTimer unscaledTimer = GetUnscaledTimer(unscaledTimerKey);
            if (unscaledTimer == null) return;
            
            TimeManager.Instance.StartUnscaledTimer(TimerHelper.GetRealtimeTimerItemKey(unscaledTimerKey), unscaledTimer.RemainDuration,
                (float duration) =>
                {
                    unscaledTimer.RemainDuration = duration;
                }, () =>
                {
                    RemoveUnscaledTimer(unscaledTimerKey);
                });
        }

        public void RemoveUnscaledTimer(string unscaledTimerKey)
        {
            UnscaledTimer unscaledTimer = GetUnscaledTimer(unscaledTimerKey);
            if (unscaledTimer == null)
                return;
            
            // 패키지 삭제 이전에 상태 값은 저장해 두어야 함
            float maxDuration = GetUnscaledTimer(unscaledTimerKey).MaxDuration;
            float currentDuration = GetUnscaledTimer(unscaledTimerKey).RemainDuration;
            
            // 우선 아래에 있는 이벤트 발생 시키기 전에 패키지는 삭제해야 함
            Cursor.UnscaledTimers.Remove(unscaledTimer);
            
            // 관련 타이머가 동작하고 있을지 모르니 모두 강제 종료
            TimeManager.Instance.StopUnscaledTimer(TimerHelper.GetRealtimeTimerItemKey(unscaledTimerKey));
            
            DebugX.Log($"[타이머 - Unscaled] 제거 : {unscaledTimerKey}");
            MessageBroker.Default.Publish(new OnUnscaledTimerRemovedMessage
            {
                TimerKey = unscaledTimerKey,
                MaxDuration = maxDuration,
                CurrentDuration = currentDuration,
            });
        }

        public bool HasUnscaledTimer(string unscaledTimerKey)
        {
            return GetUnscaledTimer(unscaledTimerKey) != null;
        }
    }
}