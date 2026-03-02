using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class ToastMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text textMessage;
    [SerializeField] private RectTransform rectBar;
    [SerializeField] private CanvasGroup canvasGroup;
    private Coroutine _coroutine;

    private void Awake()
    {
        Hide(0f);
    }
    
    public void SetToast(string message, float duration = 2f)
    {
        transform.SetAsLastSibling();
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            Hide(0);
            
        }
        
        gameObject.SetActive(true);
        _coroutine = StartCoroutine(SetMessageTask(message, duration));
    }
    
    IEnumerator SetMessageTask(string message, float duration)
    {
        MasterAudio.PlaySoundAndForget(SoundList.sound_dummy);
        
        textMessage.text = message;
        rectBar.DOScaleX(1f, 0.25f).SetEase(Ease.OutBack).SetUpdate(true);
        canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetUpdate(true);
        yield return new WaitForSecondsRealtime(duration);
        Hide();
    }

    void Hide(float duratin = 0.25f)
    {
        rectBar.DOKill();
        rectBar.DOScaleX(0f, duratin).SetEase(Ease.OutSine).SetUpdate(true);

        canvasGroup.DOKill();
        canvasGroup.DOFade(0f, duratin).SetEase(Ease.OutCubic).SetUpdate(true);
        
        gameObject.SetActive(false);
    }
}