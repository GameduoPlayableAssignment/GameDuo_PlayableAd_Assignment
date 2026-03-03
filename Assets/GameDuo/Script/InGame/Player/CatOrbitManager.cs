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

        float step = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = step * i;
            float rad   = angle * Mathf.Deg2Rad;

            Vector2 targetPos = (Vector2)_player.position
                              + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

            // 개별 속도로 lerp → 이동 시 고양이마다 약간씩 다른 반응
            cats[i].transform.position = Vector2.Lerp(
                cats[i].transform.position,
                targetPos,
                cats[i].FollowSpeed * Time.deltaTime
            );
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
