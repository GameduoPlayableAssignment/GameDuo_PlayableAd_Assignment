using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }

    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 8f;

    [Header("Shake")]
    [SerializeField] private float shakeIntensity = 0.04f;
    [SerializeField] private float shakeDuration   = 0.15f;

    private Vector3 _offset;
    private Vector3 _basePos;
    private Vector3 _shakeOffset;

    private void Awake()
    {
        Instance = this;
        _offset  = target != null
            ? new Vector3(0f, 0f, transform.position.z - target.position.z)
            : new Vector3(0f, 0f, transform.position.z);
        _basePos = transform.position;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + _offset;
        // _basePos 로 추적 → shake 는 최종 위치에 덧씀 (drift 방지)
        _basePos = Vector3.Lerp(_basePos, desired, Mathf.Clamp01(smoothSpeed * Time.deltaTime));
        transform.position = _basePos + _shakeOffset;
    }

    // 기본값으로 흔들기 (폭발 등에서 호출)
    public void Shake() => StartCoroutine(_ShakeCo(shakeIntensity, shakeDuration));

    // 강도/시간 직접 지정
    public void Shake(float intensity, float duration) => StartCoroutine(_ShakeCo(intensity, duration));

    private IEnumerator _ShakeCo(float intensity, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // slowmo 중에도 동작
            float t = 1f - elapsed / duration; // 시간 지날수록 감소
            _shakeOffset = (Vector3)(Random.insideUnitCircle * intensity * t);
            yield return null;
        }
        _shakeOffset = Vector3.zero;
    }
}
