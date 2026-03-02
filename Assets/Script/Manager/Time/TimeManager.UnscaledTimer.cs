using System;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Manager.Time
{
    public partial class TimeManager
    {
        public bool HasUnscaledTimer(string unscaledTimerKey)
        {
            return unscaledTimerKey != string.Empty && _unscaledTimerDict.ContainsKey(unscaledTimerKey);
        }
        
        public void PauseUnscaledTimer(string unscaledTimerKey)
        {
            if (!HasUnscaledTimer(unscaledTimerKey))
                return;

            _unscaledTimerDict.First(dt => dt.Key == unscaledTimerKey).Value.IsPaused = true;
        }

        public void ResumeUnscaledTimer(string unscaledTimerKey)
        {
            if (!HasUnscaledTimer(unscaledTimerKey))
                return;
            
            _unscaledTimerDict.First(dt => dt.Key == unscaledTimerKey).Value.IsPaused = false;
        }
        
        public void ResetUnscaledTimer(string unscaledTimerKey)
        {
            _unscaledTimerDict.First(dt => dt.Key == unscaledTimerKey).Value.Duration = RESET_TIMER_SECONDS;
        }
        
        public Timer GetUnscaledTimer(string timerKey)
        {
            return !_unscaledTimerDict.ContainsKey(timerKey) ? null : _unscaledTimerDict[timerKey];
        }
        
        public void ReduceUnscaledTimerDuration(string unscaledTimerKey, int reducingSeconds)
        {
            // 예외 처리
            if (!HasUnscaledTimer(unscaledTimerKey))
                return;
            
            // 진행중인 타이머가 없으면 의미 없는 동작이기 때문에 예외 처리
            if (!IsRunningUnscaledTimer(unscaledTimerKey))
                return;

            _unscaledTimerDict[unscaledTimerKey].Duration = Math.Max(_unscaledTimerDict[unscaledTimerKey].Duration - reducingSeconds, 0f);
        }
        
        public void ExtendUnscaledTimerDuration(string unscaledTimerKey, int extendingSeconds)
        {
            // 예외 처리
            if (!HasUnscaledTimer(unscaledTimerKey))
                return;
            
            // 진행중인 타이머가 없으면 의미 없는 동작이기 때문에 예외 처리
            if (!IsRunningUnscaledTimer(unscaledTimerKey))
                return;

            _unscaledTimerDict[unscaledTimerKey].Duration += extendingSeconds;
        }
        
        public bool IsRunningUnscaledTimer(string unscaledTimerKey)
        {
            return _unscaledTimerDict.ContainsKey(unscaledTimerKey) && _unscaledTimerDict[unscaledTimerKey].Duration > 0f;
        }
        
        public void StopUnscaledTimer(string unscaledTimerKey)
        {
            if (!HasUnscaledTimer(unscaledTimerKey))
                return;
            
            _unscaledTimerDict[unscaledTimerKey].Duration = 0f;
        }

        public void StartUnscaledTimer(string unscaledTimerKey, float duration, Action<float> duringCallback, Action endCallback, bool isPaused = false, bool removeIfExistRunningTimer = false)
        {
            // 예외 처리
            if (unscaledTimerKey == string.Empty)
                return;
            
            // 기존 존재하는 타이머를 제거하는 파라미터 값에 대한 처리
            if (removeIfExistRunningTimer)
            {
                StopUnscaledTimer(unscaledTimerKey);
            }
            
            // 이미 진행중인 타이머가 있으면 중복 실행하려고 할 경우 진행을 막음
            if (IsRunningUnscaledTimer(unscaledTimerKey))
                return;

            _unscaledTimerDict[unscaledTimerKey] = new Timer(duration: duration, isPaused: isPaused);
    
            // 객체 이니셜라이저를 사용해서 위의 EndTimestamp가 합치면 안됨
            _StartUnscaledTimerCo(unscaledTimerKey, duringCallback, endCallback).Forget();
        }

        async UniTaskVoid _StartUnscaledTimerCo(string unscaledTimerKey, Action<float> duringCallback, Action endCallback)
        {
            _unscaledTimerDict[unscaledTimerKey].CancellationTokenSource = new();
            
            if (_unscaledTimerDict.ContainsKey(unscaledTimerKey) == false)
                return;
                
            while (_unscaledTimerDict[unscaledTimerKey].Duration - DELAY_SECONDS > 0f)
            {
                if (!_unscaledTimerDict[unscaledTimerKey].IsPaused)
                {
                    _unscaledTimerDict[unscaledTimerKey].Duration -= DELAY_SECONDS;
                }
                duringCallback?.Invoke(_unscaledTimerDict[unscaledTimerKey].Duration);
                await UniTask.Delay(TimeSpan.FromSeconds(DELAY_SECONDS), DelayType.UnscaledDeltaTime);
            }

            duringCallback?.Invoke(0f);
            _OnEndUnscaledTimer(unscaledTimerKey);
            endCallback?.Invoke();
        }
        
        
        private void _OnEndUnscaledTimer(string unscaledTimerKey)
        {
            if (!_unscaledTimerDict.ContainsKey(unscaledTimerKey))
                return;
          
            if (_unscaledTimerDict[unscaledTimerKey].CancellationTokenSource != null)
            {
                _unscaledTimerDict[unscaledTimerKey].CancellationTokenSource?.Cancel();
                _unscaledTimerDict[unscaledTimerKey].CancellationTokenSource = null;
            }
            
            // if (_unscaledTimerDict[unscaledTimerKey].TimerCoroutine != null)
            //     StopCoroutine(_unscaledTimerDict[unscaledTimerKey].TimerCoroutine);

            _unscaledTimerDict.Remove(unscaledTimerKey);
        }
    }
}