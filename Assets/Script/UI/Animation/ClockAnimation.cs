using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ClockAnimation : MonoBehaviour
{
    [SerializeField] private Transform center;

    [SerializeField] private WaitForSecondsRealtime _waitForSecondsRealtime = new(1f);
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private bool isLinear;
    private void OnEnable ()
    {
        center.localRotation = Quaternion.identity;
        StartCoroutine(AnimationCo());
    }

    IEnumerator AnimationCo()
    {
        Vector3 rot = Vector3.zero;
        rot.z = -90;
        
        while (true)
        {
            if (isLinear)
            {
                center.DOLocalRotate(rot, duration).SetEase(Ease.Linear).SetRelative(true).SetUpdate(true);
            }
            else
            {
                center.DOLocalRotate(rot, duration).SetEase(Ease.OutBack).SetRelative(true).SetUpdate(true);
                yield return _waitForSecondsRealtime;
            }
            yield return null;
        }
    }
}