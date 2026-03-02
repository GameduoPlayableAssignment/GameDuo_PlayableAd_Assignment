using System.Collections;
using System.Collections.Generic;
using Base;
using Helper;
using Service.Timer;
using TMPro;
using UnityEngine;

public class PopupCraftShop : PopupBase
{
    [SerializeField] private TMP_Text textIdx;
    public int Idx { get; private set; }

    public void Init(int idx)
    {
        Idx = idx;

        SetData();
    }

    private void SetData()
    {
        textIdx.text = $"Test) 공방_{Idx}";
    }
    
    public void OnClick_Craft()
    {
        print("창조");
        
        TimerService.Instance.AddScaledTimer($"{Idx}", 5);
    }
    
    public void OnClick_AddMaterial()
    {
        print("추가선택");
    }
}
