using Helper.Time;
// using Manager.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Manager.Time
{
    public class TimerKey
    {
        public const string BuskingPlay = "BuskingPlay";
        public const string SilverBlessing = "SilverBlessing";
        public const string SilverBlessingByAdCount = "SilverBlessingByAdCount";
        public const string WaitingNextClass = "WaitingNextClass";
        public const string SingingNoteByRecovering = "SingingNoteByCharging";
        public const string SingingNoteByAd = "SingingNoteByAd";
        public const string Dungeon = "Dungeon";
        public const string Mission = "Mission";
        public const string LessonSkipAdCount = "LessonSkipAdCount";
        public const string BoosterType1 = "BoosterType1";
        public const string BoosterType2 = "BoosterType2";
        public const string BoosterType3 = "BoosterType3";
        public const string AdsBonusNextCooltime = "AdsBonusNextCooltime";
        public const string BountyPouchThreshold = "BountyPouch";
        public const string NewDay = "NewDay";
        public const string PostFreeReward = "PostFreeReward";
        public const string DoubleSpeed = "DoubleSpeed";
        public const string Roulette = "Roulette";
    }

    public class Timer
    {
        public Timer() { }

        public Timer(float duration, bool isPaused)
        {
            Duration = duration;
            IsPaused = isPaused;
        }

        public Timer(double endTimestampDouble)
        {
            EndTimestampDouble = endTimestampDouble;
            IsPaused = false; // 서버 타이머는 멈추는 기능 없음
        }
        public float Duration { get; set; }
        public bool IsPaused { get; set; }
        public float TotalReducingServerDuration { get; set; } // 서버 타이머 전용
        public float TotalExtendingServerDuration { get; set; } // 서버 타이머 전용
        public double EndTimestampDouble { get; set; } = -1;
        
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public Coroutine TimerCoroutine { get; set; } = null;
    }

    public partial class TimeManager : Singleton<TimeManager>
    {
        private const float RESET_TIMER_SECONDS = 2f;
        private const float DELAY_SECONDS = 0.33f;
        
        private readonly WaitForSeconds _waitForSecondsForScaledTime = new(DELAY_SECONDS);
        // private readonly WaitForSeconds _waitForSecondsForDoubleTime = new(DELAY_SECONDS);

        private Dictionary<string, Timer> _scaledTimerDict;
        private Dictionary<string, Timer> _unscaledTimerDict;
        private Dictionary<string, Timer> _doubleTimerDict;

        public string FormattedRemainTypeForNewDay => TimeHelper.GetFormattedTime(TimeHelper.RoundUpFromCurrentDouble());
        public long CurrentTimestamp => (long)CurrentTimestampDouble;
        public double CurrentTimestampDouble { get; set; }

        public void ResetAllTimer()
        {
            _scaledTimerDict.Where(dt => dt.Key != TimerKey.NewDay).ForEach(dt => dt.Value.Duration = 2f);
            _unscaledTimerDict.Where(dt => dt.Key != TimerKey.NewDay).ForEach(dt => dt.Value.Duration = 2f);
            _doubleTimerDict.Where(dt => dt.Key != TimerKey.NewDay).ForEach(tt => tt.Value.EndTimestampDouble = Instance.CurrentTimestamp + 2);
        }

        public void Init(Action callback)
        {
            // InvokeRepeating(nameof(GetServerTimeFor300Seconds), 600f, 600f); // 앱기동 후 600초 뒤에 최초 실행, 그 이후에는 600초마다 반복
            
            if (CurrentTimestampDouble > 0)
            {
                callback?.Invoke();
                return;
            }
            
            // DateTime currentTime = DateTime.Now;
            // CurrentTimestampDouble = (currentTime - new DateTime(1970, 1, 1)).TotalSeconds;
            
            // NetworkManager.Instance.GetTimestamp(response =>
            // {
            //     DebugX.Log($"GetServerTimeFor10Minutes: {response}");
            //     CurrentTimestampDouble = response.Timestamp;
            //     callback?.Invoke();
            // });
            
            
            
            var now = DateTime.Now.ToUniversalTime();
            var span = (now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            int timestamp = (int)span.TotalSeconds;
            
            CurrentTimestampDouble = (int)span.TotalSeconds;
            callback?.Invoke();
        }
        
        public void Awake()
        {
            _scaledTimerDict = new Dictionary<string, Timer>();
            _unscaledTimerDict = new Dictionary<string, Timer>();
            _doubleTimerDict = new Dictionary<string, Timer>();
        }

        public void Update()
        {
            CurrentTimestampDouble += UnityEngine.Time.unscaledDeltaTime;
        }
    }
}