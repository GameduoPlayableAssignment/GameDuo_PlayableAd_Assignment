using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [SerializeField] int maxHp = 100;

    public int   CurrentHp { get; private set; }
    public float HpRatio   => (float)CurrentHp / maxHp;

    private void Awake()
    {
        Instance = this;
        CurrentHp = maxHp;
    }

    public void TakeDamage(int amount)
    {
        CurrentHp = Mathf.Max(0, CurrentHp - amount);
    }
}
