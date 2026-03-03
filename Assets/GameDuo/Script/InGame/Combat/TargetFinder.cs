using UnityEngine;

public static class TargetFinder
{
    // Enemy.All 리스트를 순회 → Collider 설정 불필요, sqrMagnitude 로 sqrt 회피
    public static Enemy FindClosest(Vector2 position, float range)
    {
        float rangeSq    = range * range;
        float minDistSq  = float.MaxValue;
        Enemy closest    = null;

        var all = Enemy.All;
        int count = all.Count;

        for (int i = 0; i < count; i++)
        {
            Enemy enemy = all[i];
            if (enemy == null) continue;

            float distSq = ((Vector2)enemy.transform.position - position).sqrMagnitude;
            if (distSq <= rangeSq && distSq < minDistSq)
            {
                minDistSq = distSq;
                closest   = enemy;
            }
        }

        return closest;
    }
}
