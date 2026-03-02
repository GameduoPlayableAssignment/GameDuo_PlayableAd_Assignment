using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastDamage : MonoBehaviour
{
    [SerializeField] private TMP_Text textDamage;
    [SerializeField] private RectTransform rectBar;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image imageFxGlow;
    private Coroutine _coroutine;
    private float firstY;

    private void Awake()
    {
        firstY = transform.localPosition.y;
        Hide(0f);
    }

    public void SetToast(string stringDamage, bool isUp = true, float duration = 2f)
    {
        transform.SetAsFirstSibling();
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            Hide(0);
        }

        gameObject.SetActive(true);
        _coroutine = StartCoroutine(SetToastCo(stringDamage, duration, isUp));
    }

    IEnumerator SetToastCo(string stringDamage, float duration, bool isUp)
    {

        if (isUp)
        {
            textDamage.text = $"+{stringDamage}";
            textDamage.color = Utility.HexToColor("6be346");
        }
        else
        {
            textDamage.text = $"{stringDamage}";
            textDamage.color = Utility.HexToColor("d94446");
        }

        transform.DOLocalMoveY(firstY, 0.2f).SetEase(Ease.OutExpo).SetUpdate(true);
        // rectBar.DOScale(1f, 0.25f).SetEase(Ease.OutBounce).SetUpdate(true);
        canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.1f);
        Effect();
        
        MasterAudio.PlaySoundAndForget(SoundList.sound_dummy);

        yield return new WaitForSecondsRealtime(duration);
        Hide();
    }


    private void Effect()
    {
        // imageFxLine.DOFade(1f, 0.1f).SetEase(Ease.OutCubic).SetUpdate(true);
        imageFxGlow.DOFade(0.5f, 0.1f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            imageFxGlow.DOFade(0f, 0.5f).SetEase(Ease.OutSine).SetUpdate(true);
            // imageFxLine.DOFade(0f, 0.5f).SetEase(Ease.OutSine).SetUpdate(true);
        }).SetUpdate(true);
    }

    void Hide(float duratin = 0.25f)
    {
        imageFxGlow.DOKill();
        rectBar.DOKill();
        canvasGroup.DOKill();
        transform.DOLocalMoveY(firstY - 200f, 0f).SetUpdate(true);
        imageFxGlow.DOFade(0f, 0f).SetUpdate(true);
        // imageFxLine.DOFade(0f, 0f).SetUpdate(true);
        // rectBar.DOScale(1.5f, duratin).SetEase(Ease.OutSine).SetUpdate(true);
        canvasGroup.DOFade(0f, duratin).SetEase(Ease.OutCubic).SetUpdate(true);
        gameObject.SetActive(false);
    }
}

