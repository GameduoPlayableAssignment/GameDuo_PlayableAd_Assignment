using Helper.Time;
using Manager.Game;
using Manager.Time;
// using Message.Time;
using Model.Vo;
using Service.Timer;
using Service.User;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
// using Script.Service.Timer;
using UniRx;
#if false
using BackendFederation;
#endif

namespace Service.Game
{
    /// <summary>
    /// 한번만 체크해야 할 것들에 대한 키값들을 이곳에서 정리하도록 함
    /// </summary>
    public class OneTimeValue
    {
        public const string SampleKeyOneTime = "SampleKeyOneTime";
    }
    
    public class GameCursor
    {
        public long NewDayTimestamp { get; set; }
        public long LastBackgroundTransitionTimestamp { get; set; } // 가장 최근 백그라운드로 이동했을 때 기록한 타임스탬프 값
        public Dictionary<string, bool> OneTimeValueDict { get; set; }
        public static GameCursor Create()
        {
            return new GameCursor
            {
                NewDayTimestamp = -1,
                LastBackgroundTransitionTimestamp = -1,
                OneTimeValueDict = new Dictionary<string, bool>(),
            };
        }
    }
    
    public class GameService : Singleton<GameService>
    {
        private const int STORE_USERDATA_INTERVAL = 300;

        public GameCursor Cursor { get; set; }
        public bool IsRunningDoubleSpeed => TimerService.Instance.HasUnscaledTimer(TimerKey.DoubleSpeed);
        public string FormattedDoubleSpeedRemainTime
        {
            get
            {
                UnscaledTimer item = TimerService.Instance.GetUnscaledTimer(TimerKey.DoubleSpeed);
                return item == null ? string.Empty : item.FormattedRemainDurationWithColons;
            }
        }
        // public string FormattedNewDayRemainTime => _GetFormattedNewDayRemainTime();
        public void Init()
        {
            Cursor = UserService.Instance.GetUserDataForInitializing().GameCursor ?? GameCursor.Create();
        }

        public void Run()
        {
            Observable.Interval(TimeSpan.FromSeconds(STORE_USERDATA_INTERVAL)).Subscribe(x =>
            {
                UserService.Instance.StoreUserData(true, true);
            }).AddTo(GameManager.Instance.Disposables);
        }

        public bool IsCheckedOneTimeValue(string onceTimeKey)
        {
            return Cursor.OneTimeValueDict.TryGetValue(onceTimeKey, out bool _);
        }
        
        public void CheckOneTimeValue(string onceTimeKey)
        {
            Cursor.OneTimeValueDict[onceTimeKey] = true;
        }
        
        public void RunDoubleSpeed()
        {
            // TimerService.Instance.AddUnscaledTimer(TimerKey.DoubleSpeed, CommonDataVo.FastForwardDuration);
        }

        // private string _GetFormattedNewDayRemainTime()
        // {
        //     if (TimeManager.Instance.GetDoubleTimer(TimerKey.NewDay) != null)
        //     {
        //         return TimeHelper.GetFormattedTime(TimeManager.Instance.GetDoubleTimer(TimerKey.NewDay).EndTimestampDouble);
        //     }
        //     return TimeHelper.GetFormattedTime(TimeManager.Instance.CurrentTimestampDouble);
        // }
        
        // public void StartNewDayTimer()
        // {
        //     if (Cursor.NewDayTimestamp != -1)
        //     {
        //         if (TimeHelper.RoundUp(Cursor.NewDayTimestamp) <= TimeManager.Instance.CurrentTimestamp)
        //         {
        //             MessageBroker.Default.Publish(new OnNewDayStartedMessage());
        //         }
        //     }
        //
        //     Cursor.NewDayTimestamp = TimeManager.Instance.CurrentTimestamp;
        //     
        //     TimeManager.Instance.StartDoubleTimer(TimerKey.NewDay, TimeHelper.RoundUp(Cursor.NewDayTimestamp), null, () =>
        //     {
        //         MessageBroker.Default.Publish(new OnNewDayStartedMessage());
        //         StartNewDayTimer();
        //     });
        // }
        
        public void StartGoogleLogin()
        {
#if false && UNITY_ANDROID
            string message;
            var result = BackendFederation.Android.GoogleLogin("398755968487-leum1j53mf28fc2s3nkpf56nig68e9ru.apps.googleusercontent.com", out message);
            //var result = BackendFederation.Android.GoogleLogin("398755968487-sadsdawasda4563453482.apps.googleusercontent.com", out message);
            if (result == false)
            {
                DebugX.LogError(message);
            }
#endif
        }

        public void OnGoogleLogin()
        {
#if false && UNITY_ANDROID
            //구글 로그인 호출 후에 결과를알려주는 콜백 등록
            BackendFederation.Android.OnGoogleLogin += (bool isSuccess, string errorMessage, string token) =>
            {
                if (isSuccess == false)
                {
                    DebugX.LogError(errorMessage);
                }
                else
                {
                    //BackendReturnObject bro = Backend.BMember.ChangeCustomToFederation("federationToken", FederationType.Google);
                    //if (bro.IsSuccess())
                    //{
                    //    DebugX.Log("로그인 타입 전환에 성공했습니다");
                    //}
                    //else
                    //{
                    //    DebugX.Log("로그인 타입 전환에 실패했습니다");
                    //}
                    //DebugX.Log(bro.GetStatusCode());
                    //DebugX.Log(bro.GetErrorCode());
                    //DebugX.Log(bro.GetMessage());
                    
                    Backend.BMember.ChangeCustomToFederation(token, FederationType.Google, callback => {
                        DebugX.Log(callback.IsSuccess() ? "로그인 타입 전환에 성공했습니다" : "로그인 타입 전환에 실패했습니다");
                        DebugX.Log("Token : "+token);
                        DebugX.Log(callback.GetStatusCode());
                        DebugX.Log(callback.GetErrorCode());
                        DebugX.Log(callback.GetMessage());
                    });
                }
                BackendReturnObject loginBro = Backend.BMember.AuthorizeFederation(token, FederationType.Google);
                DebugX.Log("로그인 결과 : " + loginBro);
            };
#endif
        }
        
        public void OnForeground()
        {
            Cursor.LastBackgroundTransitionTimestamp = -1;
        }

        public void OnBackground()
        {
            Cursor.LastBackgroundTransitionTimestamp = TimeManager.Instance.CurrentTimestamp;
        }
    }
}
