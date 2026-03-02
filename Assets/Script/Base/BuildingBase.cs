using System;
using UnityEngine;

public class BuildingBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        GetComponent<UIButton>().onClick.AddListener(OnClick_This);
    }

    protected virtual void OnClick_This() { }
}