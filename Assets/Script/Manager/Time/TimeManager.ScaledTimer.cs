using System;
using System.Collections;
using System.Linq;

namespace Manager.Time
{
    public partial class TimeManager
    {
        public bool HasScaledTimer(string scaledTimerKey)
        {
            return scaledTimerKey != string.Empty && _scaledTimerDict.ContainsKey(scaledTimerKey);
        }
        
        public void PauseScaledTimer(string scaledTimerKey)
        {
            if (!HasScaledTimer(scaledTimerKey))
                return;

            _scaledTimerDict.First(dt => dt.Key == scaledTimerKey).Value.IsPaused = true;
        }

        public void ResumeScaledTimer(string scaledTimerKey)
        {
            if (!HasScaledTimer(scaledTimerKey))
                return;
            
            _scaledTimerDict.First(dt => dt.Key == scaledTimerKey).Value.IsPaused = false;
        }
        
        public void ResetScaledTimer(string scaledTimerKey)
        {
            _scaledTimerDict.First(dt => dt.Key == scaledTimerKey).Value.Duration = RESET_TIMER_SECONDS;
        }
        
        public Timer GetScaledTimer(string scaledTimerKey)
        {
            return !_scaledTimerDict.ContainsKey(scaledTimerKey) ? null : _scaledTimerDict[scaledTimerKey];
        }
        
        public void ReduceScaledTimerDuration(string scaledTimerKey, int reducingSeconds)
        {
            // 예외 처리
            if (scaledTimerKey == string.Empty)
                return;
            
            if (!_scaledTimerDict.ContainsKey(scaledTimerKey))
                return;
            
            // 진행중인 타이머가 없으면 의미 없는 동작이기 때문에 예외 처리
            if (!IsRunningScaledTimer(scaledTimerKey))
                return;

            _scaledTimerDict[scaledTimerKey].Duration = Math.Max(_scaledTimerDict[scaledTimerKey].Duration - reducingSeconds, 0f);
        }
        
        public void ExtendScaledTimerDuration(string scaledTimerKey, int extendingSeconds)
        {
            // 예외 처리
            if (scaledTimerKey == string.Empty)
                return;
            
            if (!_scaledTimerDict.ContainsKey(scaledTimerKey))
                return;
            
            // 진행중인 타이머가 없으면 의미 없는 동작이기 때문에 예외 처리
            if (!IsRunningScaledTimer(scaledTimerKey))
                return;

            _scaledTimerDict[scaledTimerKey].Duration += extendingSeconds;
        }
        
        public bool IsRunningScaledTimer(string scaledTimerKey)
        {
            return _scaledTimerDict.ContainsKey(scaledTimerKey) && _scaledTimerDict[scaledTimerKey].Duration > 0f;
        }

        public void StopScaledTimer(string scaledTimerKey)
        {
            if (!_scaledTimerDict.ContainsKey(scaledTimerKey))
                return;
            
            _scaledTimerDict[scaledTimerKey].Duration = 0f;
        }
        
        public void StartScaledTimer(string scaledTimerKey, float duration, Action<float> duringCallback, Action endCallback, bool isPaused = false, bool removeIfExistRunningTimer = false)
        {
            // 예외 처리
            if (scaledTimerKey == string.Empty)
                return;
            
            // 기존 존재하는 타이머를 제거하는 파라미터 값에 대한 처리
            if (removeIfExistRunningTimer)
            {
                StopScaledTimer(scaledTimerKey);
            }
            
            // 이미 진행중인 타이머가 있으면 중복 실행하려고 할 경우 진행을 막음
            if (IsRunningScaledTimer(scaledTimerKey))
                return;

            _scaledTimerDict[scaledTimerKey] = new Timer(duration: duration, isPaused: isPaused);
    
            // 객체 이니셜라이저를 사용해서 위의 EndTimestamp가 합치면 안됨
            _scaledTimerDict[scaledTimerKey].TimerCoroutine = StartCoroutine(_StartScaledTimerCo(scaledTimerKey, duringCallback, endCallback));
        }
        
        private IEnumerator _StartScaledTimerCo(string scaledTimerKey, Action<float> duringCallback, Action endCallback)
        {
            while (true)
            {
                if (_scaledTimerDict.ContainsKey(scaledTimerKey) == false)
                    yield break;

                if (!_scaledTimerDict[scaledTimerKey].IsPaused)
                {
                    if (_scaledTimerDict[scaledTimerKey].Duration - DELAY_SECONDS <= 0f)
                        break;
                    
                    _scaledTimerDict[scaledTimerKey].Duration -= DELAY_SECONDS;
                }

                // 진행중 콜백 실행
                duringCallback?.Invoke(_scaledTimerDict[scaledTimerKey].Duration);
                
                yield return _waitForSecondsForScaledTime;
            }

            duringCallback?.Invoke(0f);
            
            _OnEndScaledTimer(scaledTimerKey);
            
            endCallback?.Invoke();
        }
        
        private void _OnEndScaledTimer(string scaledTimerKey)
        {
            if (!_scaledTimerDict.ContainsKey(scaledTimerKey))
                return;
          
            if (_scaledTimerDict[scaledTimerKey].TimerCoroutine != null)
                StopCoroutine(_scaledTimerDict[scaledTimerKey].TimerCoroutine);

            _scaledTimerDict.Remove(scaledTimerKey);
        }
    }
}