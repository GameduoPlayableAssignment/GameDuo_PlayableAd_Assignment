using System.Collections;
using UnityEngine;

public class CatUnit : MonoBehaviour
{
    [SerializeField] Transform firePoint;           // 로켓 발사 시작점 (미설정 시 자신)
    [SerializeField] float attackRange = 6f;
    [SerializeField] float burstDelay  = 0.05f; // 멀티샷 로켓 간 딜레이(초)

    private CombatStats _stats;
    private Transform _player;
    private Enemy _currentTarget;
    private float _timer;

    // CatOrbitManager 가 LateUpdate 에서 Lerp 속도로 사용
    public float FollowSpeed { get; private set; }

    // CatOrbitManager 에서 Instantiate 직후 호출
    public void Init(CombatStats sharedStats, Transform playerTransform, float followSpeed)
    {
        _stats       = sharedStats;
        _player      = playerTransform;
        FollowSpeed = followSpeed;
        _timer       = Random.Range(0f, sharedStats.attackInterval); // 동시 발사 방지
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
        var   wait       = new WaitForSeconds(burstDelay);

        for (int i = 0; i < count; i++)
        {
            _SpawnRocket(target, _Rotate(baseDir, startAngle + step * i));
            if (i < count - 1) yield return wait;
        }
    }

    private void _SpawnRocket(Enemy target, Vector2 dirHint)
    {
        if (PoolManager.Instance == null) return;
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject go = PoolManager.Instance.GetRocket(spawnPos);
        if (go == null) return;
        go.GetComponent<HomingRocket>().SetTarget(target, dirHint);
    }

    private static Vector2 _Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
    }
}
