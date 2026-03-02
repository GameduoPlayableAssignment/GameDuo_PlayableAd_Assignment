using DarkTonic.MasterAudio;
using Manager.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using Service.Timer;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public enum UIMode
{
    MainPlay,
}

public partial class UIControl : MonoBehaviour
{
    public static UIControl Instance;
    public UICanvasView canvasToast;
    public ToastMessage ToastMessage { get; set; }
    public ToastDamage ToastDamage { get; set; }
      
    public bool IsInit { get; set; }
    public ReactiveProperty<UIMode> UIModeRp { get; set; } = new();
    
    [field: SerializeField] public Material materialGrayScale;
    [SerializeField] private Transform allCanvasParent;
    [field: SerializeField] public Color[] colorTier { get; set; }
    
    
    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        ToastDamage = Instantiate(Resources.Load($"{Constant.PathUICommon}ToastDamage"), canvasToast.Rect).GetComponent<ToastDamage>();
        ToastMessage = Instantiate(Resources.Load($"{Constant.PathUICommon}ToastMessage"), canvasToast.Rect).GetComponent<ToastMessage>();
        
        IsInit = true;
    }


    // TODO: 테스트
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            print($"Test - {TimerService.Instance.GetScaledTimer("0")}");
        }
    }
#endif
}
