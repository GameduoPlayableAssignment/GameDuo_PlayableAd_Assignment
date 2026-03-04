using UnityEngine;

[System.Serializable]
public class CombatStats
{
    [Min(1)] public int projectileCount = 1;        // 멀티샷(발당 로켓 수)
    [Min(0.05f)] public float attackInterval = 0.8f; // 발사 간격(초) - 낮을수록 빠름

    // 공속 합산: attacksPerSecond += add → attackInterval = 1 / attacksPerSecond
    // 예) 기본 1.25회/초 + 1.0 → 2.25회/초 → 간격 0.444초
    public void AddAttackSpeed(float attacksPerSecondAdd)
    {
        float currentAps = 1f / Mathf.Max(0.001f, attackInterval);
        float newAps     = Mathf.Max(0.5f, currentAps + attacksPerSecondAdd); // 최소 0.5회/초
        attackInterval   = 1f / newAps;
    }

    public void AddProjectiles(int add)
    {
        projectileCount = Mathf.Clamp(projectileCount + add, 1, 64);
    }
}
