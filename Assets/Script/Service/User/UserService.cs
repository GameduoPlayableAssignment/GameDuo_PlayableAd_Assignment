using Manager.Game;
using Newtonsoft.Json;
using Service.Game;
using Service.Timer;
using System;
using Manager.Time;
using Service.Dummy;
using UniRx;
using UnityEngine;

namespace Service.User
{
    public class UserService : Singleton<UserService>
    {
        private const int MIN_SECONDS_FOR_USERDATA_STORAGE = 300;
        // 유저 데이터 저장 건너뛰기 위한 플래그값
        public bool ByPassSaveUserData { get; set; } = false;

        // (s) UserService에서 자체적으로 컨트롤 하는 값들

        public ReactiveProperty<uint> ScrollObservable { get; } = new (); // Scroll 값의 구독이 필요할 때 이것을 사용
        public uint Scroll { get => ScrollObservable.Value; set => ScrollObservable.Value = value; }

        public ReactiveProperty<bool> IsPurchaseObservable { get; } = new(); // 광고제거 값의 구독이 필요할 때 이것을 사용
        public bool IsPurchase { get => IsPurchaseObservable.Value; set => IsPurchaseObservable.Value = value; }

        public ReactiveProperty<bool> IsAutoModeRp { get; set; } = new();
        public bool IsAutoMode { get => IsAutoModeRp.Value; set => IsAutoModeRp.Value = value; }
        public int CharacterCursor { get; set; }
        public bool HaveNewCharacterSkin { get; set; }

        public long LastSaveTimestamp { get; set; } = 0;

        public bool IsAlreadySetUserData { get; set; } = false; // 중요!! 중복 저장을 막기 위한 장치
        // (e) UserService에서 자체적으로 컨트롤 하는 값들
        
        public UserData UserData { private get; set; }

        
        /// <summary>
        /// Service 클래스 초기화시에만 사용하는 매소드
        /// </summary>
        /// <returns></returns>
        public UserData GetUserDataForInitializing()
        {
            return UserData;
        }
        
        public void Init()
        {
            LastSaveTimestamp = 0;
            
            // TODO: 중요!! 이 부분이 한번 true가 되면, 껐다 켜졌더라도 계속 true가 유지되는 문제가 있음 (그래서 앱 기동시 강제 false 처리함)
            // TODO: (이렇게 처리해 주지 않으면, 앱이 백그라운드로 갈 때, 유저 데이터가 저장이 되지 않을 수 있음)
            // TODO: 이 부분 코드가 없었을 때, 유저의 데이터가 제대로 저장되지 않는 이슈가 있었던 것으로 생각됨
            ByPassSaveUserData = false;
            
            // 로컬에 저장된 userData가 없을 경우, _GetInitializedUserData()를 통해 초기화 된 값을 가져옴 (앱 설치 후 최초 상황)
            UserData userData = _GetUserDataFromLocal() ?? _GetInitializedUserData();

            if (LocalStorage.IsResetUserData)
            {
                PlayerPrefs.DeleteAll();
                // LocalStorage.SerializedUserData = String.Empty;
                LocalStorage.IsResetUserData = false;
                userData = _GetInitializedUserData();
            }

            // IsPurchase, Scroll, IsAutoMode 경우 UserService에서 컨트롤하기 때문에 아래와 같이 UserData.XXX 형식으로 대입하지 않음
            IsPurchase = userData.IsPurchase;
            Scroll = userData.Scroll;
            IsAutoMode = userData.IsAutoMode;
            
            // IsPurchase, Scroll, IsAutoMode 이외의 값들은 UserData의 프로퍼티에 저장 (다른 Service 클래스에서 가져다 쓰게됨)
            UserData = new UserData
            {
                TimerCursor = userData.TimerCursor,
                GameCursor = userData.GameCursor,
                DummySlots = userData.DummySlots,
                DummyCursor = userData.DummyCursor,
                
            };
        }

        /// <summary>
        /// 로컬의 json 데이터 불러옴
        /// </summary>
        private UserData _GetUserDataFromLocal()
        {
            string sud = LocalStorage.SerializedUserData;
            return sud == string.Empty ? null : GetConvertedUserData(sud);
        }

        private UserData GetConvertedUserData(string serializedUserData)
        {
            try
            {
                UserData userData = JsonConvert.DeserializeObject<UserData>(serializedUserData);
                
                return userData;
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogException(e);
#else
                PopupHelper.ShowClientErrorCodePopup(ErrorCode.CLIENT_USERDATA_CONVERTING, e.Message);
#endif
                throw;
            }
        }

        public UserData GetCurrentGameData()
        {
            return new UserData
            {
                IsPurchase = IsPurchase,
                Scroll = Scroll,
                IsAutoMode = IsAutoMode,
                TimerCursor = TimerService.Instance.Cursor,
                GameCursor = GameService.Instance.Cursor,
                DummyCursor = DummyService.Instance.Cursor,
            };
        }
        
        /// <summary>
        /// UserData가 없을 경우 초기화 해서 리턴 해주는 기능
        /// </summary>
        /// <returns></returns>
        private UserData _GetInitializedUserData()
        {
            return new UserData
            {
                IsPurchase = false,
                Scroll = 10,
                IsAutoMode = false,
                TimerCursor = null,
                GameCursor = null,
                DummyCursor = null,
            };
        }

        public void OnForeground(int backgroundSeconds)
        {
            // TODO: 중요!! 이 값이 true가 되면 UserService의 StoreUserData() 함수가 동작하지 않기 때문에, 이 곳에서 값을 false로 만들어 줘야함
            IsAlreadySetUserData = false;
        }
        
        
        // TODO: 현재 활성화된 같은 이름의 매소드 대신에 다시 사용해야 함 by 유현규 (알로하 테스트 때문에 임시 주석)
        /// <summary>
        /// 유저 데이터로 저장
        /// </summary>
        /// <param name="isForced">IsAlreadySetUserData 값 여부에 상관없이 강제 저장 (로컬/서버)</param>
        /// <param name="isSetServer">데이터를 서버에도 저장할지 여부</param>
        public void StoreUserData(bool isForced = false, bool isSetServer = false)
        {
            // 초기화 되기 전에는 저장할 필요가 없음 (일종의 예외처리)
            if (!GameManager.Instance.IsInit)
            {
                Debug.Log($"[유저 데이터 저장] 저장하지 않음 - GameManager.Instance.IsInit 조건");
                return;
            }
            
            // 디버그 패널에서 유저 데이터 초기화 기능을 사용할 경우, 앱이 종료될 때 값을 저장하면 안됨 
            if (ByPassSaveUserData)
            {
                Debug.Log($"[유저 데이터 저장] 저장하지 않음 - ByPassSaveUserData 조건");
                return;
            }
            
            // TODO: 중요!! 중복 저장을 막기 위한 장치
            // (앱이 종료 될때에는 "OnApplicationPause 또는 OnApplicationFocus", OnApplicationQuit 이 두 이벤트가 발생하기 때문에 한번만 저장하기 위해서)
            // (OnApplicationQuit 시점에 저장되는 경우는, 이미 "OnApplicationPause 또는 OnApplicationFocus" 시점에 한번 저장한 있는 상태에서 딜레이가 있는 상태라면 OnApplicationQuit 시점에 저장이 한번 이뤄지게 됨)
            if (!isForced && IsAlreadySetUserData)
            {
                Debug.Log($"[유저 데이터 저장] 저장하지 않음 - !isForced && IsAlreadySetUserData 조건");
                return;
            }
            
            // =========================================================================================================
            // 아래는 실제 저장이 진행되는 프로세스
            // =========================================================================================================
            
            JsonSerializerSettings setting = new() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            // 로컬에 유저 데이터 저장
            string serializedUserData = JsonConvert.SerializeObject(GetCurrentGameData(), setting);
            LocalStorage.SerializedUserData = serializedUserData;
            Debug.Log($"[유저 데이터 저장] 로컬에 저장 완료");
            
            // 로컬에 저장이 이뤄지면 우선 이 값을 true로 설정함
            IsAlreadySetUserData = true;

            if (isSetServer)
            {
                // 서버에 유저 데이터 저장
            }
        }
        
        // TODO: 현재 활성화된 같은 이름의 매소드 대신에 다시 사용해야 함 by 유현규 (알로하 테스트 때문에 임시 주석)
        public void OnBackground()
        {
            // 이 값은 앱이 종료될 때, 한번 OnBackground에서 서버에 데이터를 저장했다면, 앱이 종료될 때에는 중복 저장하지 않도록 하기 위해서 구현함
            IsAlreadySetUserData = false;

            // 광고 시청을 위해서 백그라운드로 이동할 경우 저장하지 않음
            // if (GameManager.Instance.IsRunningAdRevenue)
            //     return;

            // 최초 백그라운드 이동이 아니면서 마지막으로 유저 데이터를 저장한 시간이 임계치 이하일 경우 로컬에만 저장함
            if (LastSaveTimestamp != 0 && TimeManager.Instance.CurrentTimestamp - LastSaveTimestamp < MIN_SECONDS_FOR_USERDATA_STORAGE)
            {
                StoreUserData(false, false);
                return;
            }

            // 유저 데이터를 저장하게 되면, 그 시점의 시간을 기록해 두도록 함
            LastSaveTimestamp = TimeManager.Instance.CurrentTimestamp;
            StoreUserData(false, true);
        }
        
        /*
        /// <summary>
        /// 유저 데이터로 저장
        /// </summary>
        public void StoreUserData()
        {
            if (!GameManager.Instance.IsInit)
                return;

            if (ByPassSaveUserData)
                return;
            
            // UnityEngine.Vector3 구조체 저장을 위한 장치 (그냥은 Serialize 되지 않음)
            JsonSerializerSettings setting = new() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            // 로컬에 유저 데이터 저장
            string serializedUserData = JsonConvert.SerializeObject(GetCurrentGameData(), setting);
            LocalStorage.SerializedUserData = serializedUserData;
            DebugX.Log($"[UserService] [StoreUserData] 로컬에 데이터 저장: {serializedUserData.Substring(0, 100)}");
        }
        public void OnBackground()
        {
            DebugX.Log($"[UserService] [OnBackground]");
            
            // 광고 시청을 위해서 백그라운드로 이동할 경우 저장하지 않음
            if (GameManager.Instance.IsRunningAdRevenue)
                return;

            StoreUserData();
        }
        */
    }
}