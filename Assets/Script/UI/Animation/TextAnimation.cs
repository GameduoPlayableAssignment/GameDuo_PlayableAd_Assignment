using DG.Tweening;
using TMPro;
using UnityEngine;

enum TextAnimationType
{
    Linear,
    TouchAny,
    TouchAnyStartDelay,
}
public class TextAnimation : MonoBehaviour
{
    [SerializeField] private TextAnimationType _textAnimationType;
    private TMP_Text _text;

    private void Start()
    {
        _text = gameObject.GetComponent<TMP_Text>();
        _text.DOKill();
        SelectAnimation(_textAnimationType);
    }
    
    private void SelectAnimation(TextAnimationType animationType_)
    {
        switch (animationType_)
        {
            case TextAnimationType.Linear:
                _text.DOFade(1f, 0).SetUpdate(true);
                _text.DOFade(0.1f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
                break;
            case TextAnimationType.TouchAny:
                _text.DOFade(1f, 0).SetUpdate(true);
                _text.DOFade(0.1f, 1f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
                break;
            case TextAnimationType.TouchAnyStartDelay:
                _text.DOFade(0f, 0).SetUpdate(true);
                _text.DOFade(1f, 1f).SetEase(Ease.OutCubic).SetLoops(-1, LoopType.Yoyo).SetDelay(1f).SetUpdate(true);
                break;
        }
    }
    
}