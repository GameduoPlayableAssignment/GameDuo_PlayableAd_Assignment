using UnityEngine;

/// <summary>
/// 씬에 하나 배치. Inspector 에서 세 프리팹을 연결하면 됩니다.
///   ① Rocket Prefab  — HomingRocket 컴포넌트가 붙은 로켓 프리팹
///   ② Enemy Prefab   — Enemy 컴포넌트가 붙은 적 프리팹
///   ③ Effect Prefab  — PooledEffect + ParticleSystem 이 붙은 폭발 이펙트 프리팹
/// </summary>
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject effectPrefab;   // ← 폭발 이펙트 프리팹 연결 위치

    [Header("Preload Count")]
    [SerializeField] private int rocketPreload = 20;
    [SerializeField] private int enemyPreload  = 10;
    [SerializeField] private int effectPreload = 5;

    private GameObjectPool _rocketPool;
    private GameObjectPool _enemyPool;
    private GameObjectPool _effectPool;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); 
            return;
        }
        
        Instance = this;

        if (rocketPrefab != null) _rocketPool = new GameObjectPool(rocketPrefab, rocketPreload, transform);
        if (enemyPrefab  != null) _enemyPool  = new GameObjectPool(enemyPrefab,  enemyPreload,  transform);
        if (effectPrefab != null) _effectPool  = new GameObjectPool(effectPrefab, effectPreload, transform);
    }

    // ── Rocket ────────────────────────────────────────────
    public GameObject GetRocket(Vector3 pos) => _rocketPool?.Get(pos, Quaternion.identity);
    public void ReturnRocket(GameObject go)  => _rocketPool?.Return(go);

    // ── Enemy ─────────────────────────────────────────────
    public GameObject GetEnemy(Vector3 pos) => _enemyPool?.Get(pos, Quaternion.identity);
    public void ReturnEnemy(GameObject go)  => _enemyPool?.Return(go);

    // ── Effect ────────────────────────────────────────────
    public GameObject GetEffect(Vector3 pos) => _effectPool?.Get(pos, Quaternion.identity);
    public void ReturnEffect(GameObject go)  => _effectPool?.Return(go);
}
