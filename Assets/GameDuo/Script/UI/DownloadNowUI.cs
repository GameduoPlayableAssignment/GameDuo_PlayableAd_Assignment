using DG.Tweening;
using UnityEngine;

public class DownloadNowUI : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    private void Awake()
    {
        if (canvasGroup != null)
        {
            canvasGroup.gameObject.SetActive(false);
            canvasGroup.alpha = 0;
        }
    }

    public void Show()
    {
        if (canvasGroup != null)
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.DOFade(0.9f, 0.5f).SetUpdate(true);
        }
    }

    public void OnClick_Download()
    {
        Application.OpenURL("https://gameduo.net/");
    }
}