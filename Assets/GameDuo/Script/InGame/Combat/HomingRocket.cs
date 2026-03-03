using UnityEngine;

public class HomingRocket : MonoBehaviour
{
    [SerializeField] float speed           = 8f;
    [SerializeField] float explosionRadius = 1.2f;
    [SerializeField] float lifetime        = 3f; // 자동 디스폰 시간(초)

    private Vector2 _direction;
    private float _elapsed;

    void OnEnable()
    {
        _elapsed = 0f;
    }

    public void SetTarget(Enemy enemy, Vector2 directionHint)
    {
        if (enemy != null)
        {
            Vector2 toTarget = ((Vector2)enemy.transform.position - (Vector2)transform.position).normalized;
            Vector2 hinted   = (toTarget + directionHint * 0.35f).normalized;
            _direction = hinted.sqrMagnitude > 0.0001f ? hinted : toTarget;
        }
        else
        {
            _direction = Vector2.right;
        }

        _ApplyRotation();
    }

    private void Update()
    {
        _elapsed += Time.deltaTime;
        if (_elapsed >= lifetime)
        {
            _ReturnToPool();
            return;
        }

        transform.position += (Vector3)(_direction * speed * Time.deltaTime);
    }

    // 스프라이트 기본 앞방향이 -X 이므로 +180 오프셋
    private void _ApplyRotation()
    {
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg + 180f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() == null) 
            return;
        
        _Explode();
    }

    private void _Explode()
    {
        // AOE: Enemy.All 을 역순으로 순회 (TakeDamage 가 리스트를 수정할 수 있으므로)
        var     all = Enemy.All;
        float   radiusSq = explosionRadius * explosionRadius;
        Vector2 pos = transform.position;

        for (int i = all.Count - 1; i >= 0; i--)
        {
            if (all[i] == null) 
                continue;

            if (((Vector2) all[i].transform.position - pos).sqrMagnitude <= radiusSq)
            {
                all[i].TakeDamage(1);
            }
        }

        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.GetEffect(transform.position);
        }

        _ReturnToPool();
    }

    private void _ReturnToPool()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.ReturnRocket(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
