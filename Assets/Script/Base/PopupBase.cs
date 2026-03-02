using DarkTonic.MasterAudio;
using DG.Tweening;
using Manager.Game;
using Manager.Popup;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Base
{
    public enum PanelMoveType
    {
        None,
        LeftToRight,
        RightToLeft, 
        TopToBot,
        BotToTop
    }

    [Flags]
    public enum ResourceState
    {
        None = 0,
        Gold = 1 << 1,
        Gem = 1 << 2,
        StarLight = 1 << 4,
        Scroll = 1 << 8,
        Essence = 1 << 16,
        All = int.MaxValue
            
    }
    
    public class PopupBase : MonoBehaviour
    {
        protected bool IsShow { get; set; }
        [HideInInspector] public PopupType popupType;
        public bool isMoveToClose;
        [field: SerializeField] public bool IsFrontResourceBar { get; set; }
        [field: SerializeField] public ResourceState IsForceOnResource { get; set; } = ResourceState.None;
        public PopupSameType popupSameType;
        private IDisposable _disposable;
        public UnityEvent OnCloseEvent { get; set; } = new();
        public Canvas popupBaseCanvas { get; private set; }
        private PopupAnimation _popupAnimation;

        public void SetCanvasOrder(int currentOrder)
        {
            popupBaseCanvas.sortingOrder = currentOrder + 1;
        }
        
        protected virtual void Awake()
        {
            Init();
        }

        /// <summary>
        ///     초기화
        /// </summary>
        protected virtual void Init()
        {
            popupBaseCanvas = GetComponent<Canvas>();
            TryGetComponent(out PopupAnimation panelAnimation);
            if (panelAnimation == null)
            {
                return;
            }

            // if (UIControl.Instance && IsFrontResourceBar)
            // {
            //     ResourceBar.Instance.SetResourceType(IsForceOnResource);
            // }

            _popupAnimation = panelAnimation;
            _popupAnimation.Init();
        }

        /// <summary>
        ///     데이터 세팅이 끝나면 패널을 UI에 표시한다.
        /// </summary>
        public virtual void Show()
        {
            IsShow = true;
            
            // if (IsFrontResourceBar)
            // {
            //     ResourceBar.Instance.SetOrderFront();
            // }

            // if (!TutorialService.Instance.IsGuideRunning)
            // {
            //     GameManager.Instance.IsLockedScreen = false;
            // }
            if (_popupAnimation)
            {
                _popupAnimation.OpenAnimation();
            }

            MasterAudio.PlaySoundAndForget(SoundList.sound_dummy);
        }


        /// <summary>
        ///     패널 닫기
        /// </summary>
        public virtual void Close()
        {
            IsShow = false;
            _disposable?.Dispose();

            OnCloseEvent?.Invoke();
            OnCloseEvent?.RemoveAllListeners();

            if (_popupAnimation)
            {
                _popupAnimation.CloseAnimation(() =>
                {
                    // if (UIControl.Instance && IsFrontResourceBar) ResourceBar.Instance.SetDefaultOrder();
                    PopupManager.Instance.RemovePopup(this);
                    Destroy(gameObject);
                });
            }
            else
            {
                // if (UIControl.Instance && IsFrontResourceBar) ResourceBar.Instance.SetDefaultOrder();
                PopupManager.Instance.RemovePopup(this);
                Destroy(gameObject);
            }
        }
    }
}