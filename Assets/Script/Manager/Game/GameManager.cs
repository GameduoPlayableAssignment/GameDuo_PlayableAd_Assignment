using Helper;
using Manager.DataTable;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Manager.Time;
using Model.Vo;
using Service.Dummy;
using Service.Game;
using Service.Timer;
using Service.User;
using UniRx;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Manager.Game
{
    public partial class GameManager : Singleton<GameManager>
    {
        private bool _isLoadScene;
        private GameObject _preventTouchObject;
        public SceneType sceneType;
        public bool IsInBackground { get; private set; }
        public bool IsInit { get; private set; } = false;
        public CompositeDisposable Disposables { get; } = new();

        public bool IsLockedScreen
        {
            set
            {
                if (!_preventTouchObject)
                {
                    _preventTouchObject = (GameObject)Instantiate(Resources.Load($"{Constant.PathUICommon}CanvasRaycasterBlock"), transform);
                }
                _preventTouchObject.SetActive(value);
            }
        }
        
        public void Init(Action completeCallback)
        {
            // 이미 초기화 되었을 경우에 대한 예외 처리 (아래 코드를 중복 실행하면 안되기 때문에)
            if (IsInit)
                return;

            Application.targetFrameRate = Math.Max(Application.targetFrameRate, 60); // 타켓 프레임율 60으로 고정 (60보다 큰 경우는 없다고 가정)
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
            TimeManager.Instance.Init(() => { 
                DataTableManager.Instance.ParseData(() => {
                    _InitManagerAndServices(); // 각 서비스 및 매니저 클래스 초기화
                    IsInit = true; // 모든 것들의 초기화가 끝났기 때문에 IsInit 값을 true로 설정
                    completeCallback?.Invoke(); // 콜백 실행
                });
            });
            
        }

        private void _InitManagerAndServices()
        {
            UserService.Instance.Init();
            GameService.Instance.Init();
            
            DummyService.Instance.Init();
            
            TimerService.Instance.Init();
        }

        public void LoadScene(SceneType type)
        {
            LoadSceneTask(type).Forget();
        }

        public async UniTask LoadSceneTask(SceneType type, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            if (_isLoadScene) return;
            _isLoadScene = true;

            await Resources.UnloadUnusedAssets();

            Application.backgroundLoadingPriority = ThreadPriority.Low; //성능에 영향을 주지 않고 로딩되도록 처리
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneType.ToString(), loadSceneMode);

            while (!async.isDone)
            {
                async.allowSceneActivation = async.progress >= 0.9f;
                await UniTask.Yield();
            }

            _isLoadScene = false;
        }

        public bool CheckScene(SceneType type)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (scene.name == type.ToString())
                {
                    // CtrBusking.Instance.Init();
                    return true;
                }
            }

            return false;
        }

        public bool IsMouseOverUI()
        {
            PointerEventData eventDataCurrentPosition = new(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        private void SetAudioMixer()
        {
            AudioMixer mixer = Resources.Load<AudioMixer>("Mixer/Mixer");
            
            float BgmAudio = PlayerPrefs.GetFloat("BgmAudio", 1);
            float audioValue = Mathf.Log10(BgmAudio) * 20;

            if (float.IsNegativeInfinity(audioValue))
            {
                audioValue = -80;
            }

            mixer.SetFloat("BGM", audioValue);

            float SFXAudio = PlayerPrefs.GetFloat("SFXAudio", 1);
            audioValue = Mathf.Log10(SFXAudio) * 20;

            if (float.IsNegativeInfinity(audioValue))
            {
                audioValue = -80;
            }

            mixer.SetFloat("Effect", audioValue);
        }
        // TODO: (e) 유니티 에디터에서 OnApplicationPause()가 동작하지 않아서 매소드를 전처리기를 통해서 분기 처리함

        private void CheckForeground(bool isForeground)
        {
            if (IsInit == false)
            {
                return;
            }

            if (isForeground)
            {
                IsInBackground = false;

                int backgroundSeconds = 0;
                if (GameService.Instance.Cursor.LastBackgroundTransitionTimestamp > 0)
                {
                    backgroundSeconds = (int)(TimeManager.Instance.CurrentTimestamp - GameService.Instance.Cursor.LastBackgroundTransitionTimestamp);
                }

                UserService.Instance.OnForeground(backgroundSeconds);
            }
            else
            {
                IsInBackground = true;

                GameService.Instance.Cursor.LastBackgroundTransitionTimestamp = TimeManager.Instance.CurrentTimestamp;

                UserService.Instance.OnBackground();
            }
        }

        private void CheckEscape()
        {
            if (!IsInit)
            {
                return;
            }

            // 뒤로 가기에 대한 상황 처리
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 1. 튜토리얼 중이라면 무시한다.

                // 2. 현재띄어진 팝업을 끈다.

                // 3. 던전 진행중이라면 던전 나가기를 누른다.

                // 4. 0번탭이 아닌 다른탭이라면 > 메인 1번탭으로 이동한다.

                // 5. 종료팝업을 띄운다.
            }
        }

        #region Speed Up
        public float defaultTimeScale = 1f; // TODO - isSpeedUp 임시

        public bool IsSpeedUp
        {
            get => LocalStorage.IsSpeedUp;
            set
            {
                LocalStorage.IsSpeedUp = value;
                if (UnityEngine.Time.timeScale == 0) return;
                SetTimeScale(LocalStorage.IsSpeedUp ? defaultTimeScale * CommonDataVo.FastForwardSpeed : 1f);
            }
        }

        public void IsPause(bool isPause)
        {
            if (isPause)
            {
                SetTimeScale(0f);
            }
            else
            {
                SetTimeScale(LocalStorage.IsSpeedUp ? defaultTimeScale * CommonDataVo.FastForwardSpeed : 1f);
            }
        }

        public void SetTimeScale(float timeScale)
        {
            UnityEngine.Time.timeScale = timeScale;
        }
        #endregion
    }
}