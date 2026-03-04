using Ad;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRadius = 8f;
    [SerializeField] private float spawnInterval = 0.8f;

    private float _timer;

    private void Update()
    {
        if (!AdGameFlow.Instance.IsPlaying)
            return;

        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            _timer = 0f;
            _SpawnEnemy();
            _SpawnEnemy();
        }
    }

    private void _SpawnEnemy()
    {
        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;

        Vector2 spawnPos = (Vector2)player.position + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * spawnRadius;

        PoolManager.Instance?.GetEnemy(spawnPos);
    }
}