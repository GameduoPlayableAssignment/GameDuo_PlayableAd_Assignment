using Base;
using Manager.Game;
using Service.Game;

namespace Script.Ctr
{
    public class CtrPlay : SceneBase
    {
        public static CtrPlay Instance { get; set; }

        protected override void Awake()
        {
            // 여러번 호출하더라도 중복 초기화 되지는 않음
            GameManager.Instance.Init(() => { });
            
            Instance = this;
            
            base.Awake();
        }
    }
}