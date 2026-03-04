using System.Collections;
using Ad;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private Transform firePoint;           // 로켓 발사 시작점 (미설정 시 자신)
    
    [SerializeField] private float moveSpeed   = 5f;
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float burstDelay  = 0.05f; // 멀티샷 로켓 간 딜레이(초)

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
            _SpawnRocket(target);
            return;
        }

        StartCoroutine(_FireBurstCo(target, count));
    }

    private IEnumerator _FireBurstCo(Enemy firstTarget, int count)
    {
        _SpawnRocket(firstTarget); // 첫 발 → 가장 가까운 타겟

        for (int i = 1; i < count; i++)
        {
            yield return _burstWait;

            var all = Enemy.All;
            Enemy t = all.Count > 0 ? all[Random.Range(0, all.Count)] : firstTarget;
            _SpawnRocket(t);
        }
    }

    private void _SpawnRocket(Enemy target)
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject go = PoolManager.Instance.GetRocket(spawnPos);

        if (go == null)
            return;

        go.GetComponent<HomingRocket>().SetTarget(target);
    }

    private void _Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }
}
