using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 스택 기반 범용 오브젝트 풀.
/// PoolManager 에서 생성·관리하며 외부에서 직접 MonoBehaviour 없이 사용 가능.
/// </summary>
public class GameObjectPool
{
    readonly GameObject _prefab;
    readonly Transform  _parent;
    readonly Stack<GameObject> _stack = new();

    public GameObjectPool(GameObject prefab, int preload, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < preload; i++)
        {
            var go = Object.Instantiate(prefab, parent);
            go.SetActive(false);
            _stack.Push(go);
        }
    }

    public GameObject Get(Vector3 pos, Quaternion rot)
    {
        GameObject go;
        if (_stack.Count > 0)
        {
            go = _stack.Pop();
            go.transform.SetPositionAndRotation(pos, rot);
            go.SetActive(true);
        }
        else
        {
            go = Object.Instantiate(_prefab, pos, rot, _parent);
        }
        
        return go;
    }

    public void Return(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(_parent);
        _stack.Push(go);
    }
}
