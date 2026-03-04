using System.Collections;
using Ad;
using UnityEngine;

public class CutSequenceController : MonoBehaviour
{
    public static CutSequenceController Instance { get; private set; }

    [Header("Refs")]
    [SerializeField] private DownloadNowUI ctaUI;

    [Header("Cut Tuning")]
    [SerializeField] private float slowmoScale            = 0.15f;
    [SerializeField] private float slowmoDurationUnscaled = 1.2f;

    private bool _ctaTriggered;

    private void Awake() => Instance = this;

    public void PlayFinalCut()
    {
        _ctaTriggered = false;
        AdGameFlow.Instance.SetState(AdState.Cut);
        _OnFinalRocketExploded();
    }

    // 마지막 선택 이후 발사된 로켓 폭발 시 호출
    private void _OnFinalRocketExploded()
    {
        if (_ctaTriggered) 
            return;
        
        _ctaTriggered = true;
        StartCoroutine(_SlowmoThenFreezeCo());
    }

    private IEnumerator _SlowmoThenFreezeCo()
    {
        Time.timeScale = slowmoScale;

        AdGameFlow.Instance.SetState(AdState.CTA);
        
        if (ctaUI != null)
        {
            ctaUI.Show(slowmoDurationUnscaled);
        }
        
        float t = 0f;
        while (t < slowmoDurationUnscaled)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 0f;
        
    }
}
