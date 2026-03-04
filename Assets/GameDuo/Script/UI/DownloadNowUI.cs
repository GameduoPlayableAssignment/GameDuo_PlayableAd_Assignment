using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DownloadNowUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform logo;
    [SerializeField] private Transform playNow;
    [SerializeField] private Transform handTarget;
    [SerializeField] private TutorialHand hand;

    private void Awake()
    {
        if (canvasGroup != null)
        {
            canvasGroup.gameObject.SetActive(false);
            logo.gameObject.SetActive(false);
            playNow.gameObject.SetActive(false);
            
            canvasGroup.alpha = 0;
            logo.DOScale(1.5f, 0);
            playNow.DOScale(1.5f, 0f);
        }
    }

    public void Show(float slowmoDuration)
    {
        canvasGroup.gameObject.SetActive(true);
        _Animation(slowmoDuration);
    }

    private async UniTask _Animation(float slowmoDuration)
    {
        canvasGroup.DOFade(0.9f, slowmoDuration).SetUpdate(true);
        
        await UniTask.WaitForSeconds(slowmoDuration, true);

        logo.gameObject.SetActive(true);
        logo.DOScale(1f, 0.25f).SetEase(Ease.InOutBack).SetUpdate(true);
        
        await UniTask.WaitForSeconds(0.35f, true);
        
        playNow.gameObject.SetActive(true);
        playNow.DOScale(1f, 0.25f).SetEase(Ease.InOutBack).SetUpdate(true);
        
        await UniTask.WaitForSeconds(0.65f, true);
        
        hand.ShowHand(handTarget);
        
        await UniTask.WaitForSeconds(1.2f, true);
        
        playNow.DOScale(1.1f, 0.08f).SetEase(Ease.InOutSine).SetLoops(4, LoopType.Yoyo).SetUpdate(true);
    }

    public void OnClick_Download()
    {
        Application.OpenURL("https://gameduo.net/");
    }
}