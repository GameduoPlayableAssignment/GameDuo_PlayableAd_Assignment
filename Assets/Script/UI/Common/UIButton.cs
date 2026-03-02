using DarkTonic.MasterAudio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ButtonSoundType
{
    None,
    Default1,
    Default2,
    MainMenuOpen,
    BottomDepth1,
    BottomDepth2,
    BuildAll,
    Close
}

public enum ButtonAnimation
{
    None,
    Scale
}


public class UIButton : Selectable, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{

    //Type
    public ButtonSoundType buttonSoundType = ButtonSoundType.None;
    
    [SerializeField] private bool isScaleAnim = true;

    [field: SerializeField] public bool IsCustomCenterPostion { get; set; }
    public RectTransform customCenterPoint;

    public bool IsScaleAnim
    {
        set { isScaleAnim = value; }
    }
    
    
    //ScaleValue
    private float _scaleValue;

    //UnityEvent
    public UnityEvent onClick = new ();
    public UnityEvent onDown = new ();
    public UnityEvent onUp = new ();
    public UnityEvent onEnter = new ();
    public UnityEvent onExit = new ();

    protected override void Start()
    {
        SetButton();
        base.Start();
    }

    private void SetButton()
    {
        //트랜지션 끄기
        transition = Transition.ColorTint;

        SetButtonNavigation();
        SetButtonScalePower();
    }

    private void SetButtonNavigation()
    {
        //네비게이션 끄기
        var buttonNavigation = navigation;
        buttonNavigation.mode = Navigation.Mode.None;
        navigation = buttonNavigation;
    }

    public void SetButtonScalePower()
    {
        if (isScaleAnim)
        {
            _scaleValue = 1.03f;
        }
        else
        {
            _scaleValue = 1f;
        }
    }
    
    
    #region 버튼 이벤트
    /// <summary>
    /// OnDown
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (buttonSoundType != ButtonSoundType.None) MasterAudio.PlaySoundAndForget(SoundList.GetButtonSoundName(buttonSoundType));
        
        if(!interactable)
            return;

        IsScaleReset = false;
        
        if (isScaleAnim)
        {
            transform.transform.DOKill();
            transform.transform.DOScale(_scaleValue, 0.1f).SetEase(Ease.OutCubic).SetUpdate(true);
        }
        
        //이벤트 실행
        onDown?.Invoke();   
    }
    private bool IsScaleReset;

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        
        if (isScaleAnim && !IsScaleReset)
        {
            IsScaleReset = true;
            transform.DOKill();
            transform.DOScale(1f, 1f).SetEase(Ease.OutElastic).SetUpdate(true);   
        }
        
        onUp?.Invoke();
    }


    /// <summary>
    /// OnClick
    /// </summary>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable)
            return;
        
        //이벤트 실행
        onClick?.Invoke();
    }
    #endregion


    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!interactable)
            return;

   
        
        //이벤트 실행
        onEnter?.Invoke();
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!interactable)
            return;
            
        if (isScaleAnim && !IsScaleReset)
        {
            IsScaleReset = true;
            transform.DOKill();
            transform.DOScale(1f, 1f).SetEase(Ease.OutElastic).SetUpdate(true);   
        }
        
        //이벤트 실행
        onExit?.Invoke();
    }

    
    


    protected override void OnDestroy()
    {
        base.OnDestroy();
        onClick.RemoveAllListeners();
        transform.DOKill();
    }
}
