using UnityEngine;
using System.Collections.Generic;

public class CatOrbitManager : MonoBehaviour
{
    [SerializeField] GameObject catPrefab;
    [SerializeField] float radius = 2.5f;

    [Header("Follow Speed (자연스러운 차이)")]
    [SerializeField] float followSpeedMin = 8f;
    [SerializeField] float followSpeedMax = 14f;

    [SerializeField] CombatStats sharedStats = new CombatStats();

    readonly List<CatUnit> cats = new();
    private Transform _player;

    // 고양이 수가 바뀔 때만 재계산하는 궤도 오프셋 캐시 (sin/cos 반복 연산 제거)
    private Vector2[] _orbitOffsets = System.Array.Empty<Vector2>();
    private int _cachedCount  = 0;

    private void Awake()
    {
        _player = transform;
    }

    // LateUpdate: 플레이어 이동(Update) 이후 고양이 위치 확정
    private void LateUpdate()
    {
        int count = cats.Count;

        if (count == 0)
            return;

        // 고양이 수가 바뀐 경우에만 sin/cos 재계산
        if (count != _cachedCount)
            _RebuildOffsets(count);

        Vector2 playerPos = _player.position;

        for (int i = 0; i < count; i++)
        {
            Vector2 targetPos = playerPos + _orbitOffsets[i] * radius;

            // 개별 속도로 lerp → 이동 시 고양이마다 약간씩 다른 반응
            cats[i].transform.position = Vector2.Lerp(
                cats[i].transform.position,
                targetPos,
                cats[i].FollowSpeed * Time.deltaTime
            );
        }
    }

    private void _RebuildOffsets(int count)
    {
        _cachedCount  = count;
        _orbitOffsets = new Vector2[count];
        float step = 360f / count * Mathf.Deg2Rad;
        for (int i = 0; i < count; i++)
        {
            float rad = step * i;
            _orbitOffsets[i] = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }
    }

    public void AddCats(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(catPrefab);
            var unit = go.GetComponent<CatUnit>();
            float speed = Random.Range(followSpeedMin, followSpeedMax);
            unit.Init(sharedStats, _player, speed);
            cats.Add(unit);
        }
    }

    public CombatStats GetSharedStats() => sharedStats;
}
