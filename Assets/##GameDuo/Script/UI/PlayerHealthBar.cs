using UnityEngine;

/// <summary>
/// 플레이어 위에 떠다니는 월드 스페이스 체력 바.
/// fillRect: 초록색 fill 오브젝트의 RectTransform. Pivot X = 0 (왼쪽 기준 스케일).
/// </summary>
public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Transform     playerTransform;
    [SerializeField] private Vector3       offset   = new Vector3(0f, 1.2f, 0f);
    [SerializeField] private RectTransform fillRect; // Pivot X = 0 으로 설정

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;
        }

        if (fillRect == null || PlayerHealth.Instance == null) 
            return;

        // localScale.x 로 너비 조절 (Image Type 상관없이 동작)
        Vector3 s = fillRect.localScale;
        s.x = PlayerHealth.Instance.HpRatio;
        fillRect.localScale = s;
    }
}
