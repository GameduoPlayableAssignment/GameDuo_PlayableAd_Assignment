using System.Collections;
using Ad;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] FloatingJoystick joystick;
    [SerializeField] Transform firePoint;           // 로켓 발사 시작점 (미설정 시 자신)
    
    [SerializeField] float moveSpeed   = 5f;
    [SerializeField] float attackRange = 6f;
    [SerializeField] float burstDelay  = 0.05f; // 멀티샷 로켓 간 딜레이(초)

    private CombatStats _stats;
    private WaitForSeconds _burstWait;
    private float _attackTimer;
    private bool _facingLeft;

    private void Start()
    {
        var orbit = GetComponent<CatOrbitManager>();
        if (orbit != null)
        {
            _stats = orbit.GetSharedStats();
        }

        _attackTimer = _stats != null ? Random.Range(0f, _stats.attackInterval) : 0f;
        _burstWait   = new WaitForSeconds(burstDelay);
    }

    private void Update()
    {
        _Move();
        _AutoAttack();
    }

    // ── 이동 ──────────────────────────────────────────────
    private void _Move()
    {
        if (AdGameFlow.Instance.State != AdState.Playing)
            return;
        
        Vector2 input = joystick.Direction;
        if (input.sqrMagnitude > 0.001f)
        {
            transform.position += (Vector3)(input * moveSpeed * Time.deltaTime);

            if (Mathf.Abs(input.x) > 0.01f)
            {
                bool shouldFaceLeft = input.x > 0f; // 반전
                if (shouldFaceLeft != _facingLeft)
                {
                    _facingLeft = shouldFaceLeft;
                    _Flip();
                }
            }
        }
    }

    // ── 자동 공격 ─────────────────────────────────────────
    private void _AutoAttack()
    {
        if (_stats == null || PoolManager.Instance == null) 
            return;

        _attackTimer += Time.deltaTime;
        if (_attackTimer < _stats.attackInterval) 
            return;

        _attackTimer = 0f;
        Enemy target = TargetFinder.FindClosest(transform.position, attackRange);
        if (target == null) 
            return;

        _FaceTarget(target.transform.position);
        _Fire(target);
    }

    private void _FaceTarget(Vector2 targetPos)
    {
        float dirX = targetPos.x - transform.position.x;
        if (Mathf.Abs(dirX) > 0.01f)
        {
            bool shouldFaceLeft = dirX > 0f; // 반전
            if (shouldFaceLeft != _facingLeft)
            {
                _facingLeft = shouldFaceLeft;
                _Flip();
            }
        }
    }

    private void _Fire(Enemy target)
    {
        int count = _stats.projectileCount;

        if (count == 1)
        {
            _SpawnRocket(target, Vector2.zero);
            return;
        }

        StartCoroutine(_FireBurstCo(target, count));
    }

    private IEnumerator _FireBurstCo(Enemy target, int count)
    {
        Vector2 baseDir  = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
        float spread     = 24f;
        float step       = spread / (count - 1);
        float startAngle = -spread * 0.5f;

        for (int i = 0; i < count; i++)
        {
            _SpawnRocket(target, _Rotate(baseDir, startAngle + step * i));

            if (i < count - 1)
            {
                yield return _burstWait;
            }
        }
    }

    private void _SpawnRocket(Enemy target, Vector2 dirHint)
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject go = PoolManager.Instance.GetRocket(spawnPos);
        
        if (go == null) 
            return;
        
        go.GetComponent<HomingRocket>().SetTarget(target, dirHint);
    }

    private void _Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    private static Vector2 _Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
    }
}
