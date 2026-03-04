using UnityEngine;

/// <summary>
/// 이펙트 프리팹에 부착. 활성화되면 자동으로 ParticleSystem 재생 후 풀에 반환.
///
/// 설정 방법:
///   1. 폭발 이펙트 프리팹을 만들고 이 컴포넌트를 추가
///   2. 같은 오브젝트(또는 자식)에 ParticleSystem 추가
///   3. Duration 은 ParticleSystem Duration + Start Lifetime 보다 크게 설정
///   4. PoolManager Inspector 의 Effect Prefab 슬롯에 연결
/// </summary>
public class PooledEffect : MonoBehaviour
{
    [SerializeField] private float duration = 1.5f;

    private ParticleSystem[] _systems;
    private float _timer;

    private void Awake()
    {
        _systems = GetComponentsInChildren<ParticleSystem>(true);
    }

    private void OnEnable()
    {
        _timer = 0f;
        for (int i = 0; i < _systems.Length; i++)
            _systems[i].Play();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= duration)
        {
            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.ReturnEffect(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
