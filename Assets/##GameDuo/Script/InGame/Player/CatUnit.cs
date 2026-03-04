using System.Collections;
using UnityEngine;

public class CatUnit : MonoBehaviour
{
    [SerializeField] private Transform firePoint;           // 로켓 발사 시작점 (미설정 시 자신)
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float burstDelay  = 0.05f; // 멀티샷 로켓 간 딜레이(초)

    private CombatStats    _stats;
    private Transform      _player;
    private Enemy          _currentTarget;
    private WaitForSeconds _burstWait;
    private float          _timer;

    // CatOrbitManager 가 LateUpdate 에서 Lerp 속도로 사용
    public float FollowSpeed { get; private set; }

    // CatOrbitManager 에서 Instantiate 직후 호출
    public void Init(CombatStats sharedStats, Transform playerTransform, float followSpeed)
    {
        _stats     = sharedStats;
        _player    = playerTransform;
        FollowSpeed = followSpeed;
        _timer     = Random.Range(0f, sharedStats.attackInterval); // 동시 발사 방지
        _burstWait = new WaitForSeconds(burstDelay);
    }

    private void Update()
    {
        if (_stats == null) 
            return;

        // 타겟 없으면 플레이어 방향 미러링
        if (_currentTarget == null)
            _MirrorPlayerFacing();

        _timer += Time.deltaTime;

        if (_timer >= _stats.attackInterval)
        {
            _timer = 0f;
            _currentTarget = TargetFinder.FindClosest(transform.position, attackRange);

            if (_currentTarget != null)
            {
                _FaceTarget(_currentTarget.transform.position);
                _Fire(_currentTarget);
            }
        }
    }

    private void _MirrorPlayerFacing()
    {
        if (_player == null) 
            return;

        float playerSign = _player.localScale.x < 0 ? -1f : 1f;
        float mySign     = transform.localScale.x < 0 ? -1f : 1f;

        if (mySign == playerSign) 
            return;
        
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * playerSign;
        transform.localScale = scale;
    }

    private void _FaceTarget(Vector2 targetPos)
    {
        float dirX = targetPos.x - transform.position.x;
        if (Mathf.Abs(dirX) > 0.01f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (dirX < 0 ? -1f : 1f);
            transform.localScale = scale;
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
        if (PoolManager.Instance == null) return;
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject go = PoolManager.Instance.GetRocket(spawnPos);
        if (go == null) return;
        go.GetComponent<HomingRocket>().SetTarget(target);
    }
}
