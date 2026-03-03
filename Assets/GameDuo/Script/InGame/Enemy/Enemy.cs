using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    // 씬의 모든 활성 Enemy 관리 (TargetFinder, AOE 에서 사용)
    static readonly List<Enemy> _all = new();
    public static IReadOnlyList<Enemy> All => _all;

    // PlayerController 참조는 씬 전체에서 하나 → 정적 캐시
    private static Transform _player;

    [SerializeField] private int   damage        = 3;
    [SerializeField] private int   maxHp         = 3;
    [SerializeField] private float moveSpeed     = 2f;
    [SerializeField] private float contactRadius = 0.5f; // 플레이어 접촉 판정 반경

    private int _hp;
    private float _contactRadiusSq;

    private void OnEnable()
    {
        _all.Add(this);
        _hp = maxHp;
        _contactRadiusSq = contactRadius * contactRadius;

        if (_player != null) 
            return;
        
        var pc = FindObjectOfType<PlayerController>();
        
        if (pc == null) 
            return;
        
        _player = pc.transform;
    }

    private void OnDisable()
    {
        _all.Remove(this);
    }

    private void Update()
    {
        if (_player == null) 
            return;

        Vector2 delta = (Vector2)_player.position - (Vector2)transform.position;
        Vector2 dir   = delta.normalized;
        transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);

        // 이동 방향에 따라 좌우 플립
        if (Mathf.Abs(dir.x) > 0.01f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (dir.x < 0 ? -1f : 1f);
            transform.localScale = scale;
        }

        // 플레이어 접촉 판정 (물리 없이 거리 직접 체크)
        if (delta.sqrMagnitude <= _contactRadiusSq)
        {
            PlayerHealth.Instance?.TakeDamage(damage);

            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.ReturnEnemy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        _hp -= amount;
        if (_hp > 0) 
            return;

        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.ReturnEnemy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
