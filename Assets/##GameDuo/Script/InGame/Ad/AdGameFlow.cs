using UnityEngine;

namespace Ad
{
    public enum AdState
    {
        Playing,  // 인게임 진행 중
        Upgrade,  // 업그레이드 선택 UI 표시 중 (timeScale = 0)
        Cut,      // 최종 컷 연출 중 (슬로모션, 첫 폭발 대기)
        CTA       // 광고 종료 CTA 표시 중 (timeScale = 0)
    }

    public sealed class AdGameFlow : MonoBehaviour
    {
        private static AdGameFlow _instance;
        public static AdGameFlow Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[AdGameFlow]");
                    _instance = go.AddComponent<AdGameFlow>();
                }
                
                return _instance;
            }
        }

        public AdState State { get; private set; } = AdState.Playing;
        public bool IsPlaying => State == AdState.Playing;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject); 
                return;
            }
            
            _instance = this;
        }

        private void OnDestroy()
        {
            if (_instance != this) 
                return;
            
            _instance = null;
        }

        public void SetState(AdState state) => State = state;
    }
}