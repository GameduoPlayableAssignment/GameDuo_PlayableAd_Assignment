using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }

    [SerializeField] Transform target;
    [SerializeField] float smoothSpeed = 8f;

    private Vector3 _offset;

    private void Awake()
    {
        Instance = this;

        _offset = target != null ? new Vector3(0f, 0f, transform.position.z - target.position.z) : new Vector3(0f, 0f, transform.position.z);
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + _offset;
        float z = transform.position.z;
        // Clamp01: smoothSpeed * deltaTime > 1 이면 overshoot → 진짜 흔들림 발생
        transform.position = Vector3.Lerp(transform.position, desired, Mathf.Clamp01(smoothSpeed * Time.deltaTime));
    }
}
