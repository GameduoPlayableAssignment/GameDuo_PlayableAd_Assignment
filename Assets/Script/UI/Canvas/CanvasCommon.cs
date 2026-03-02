using System.Collections;
using System.Collections.Generic;
using Manager.Popup;
using UnityEngine;

public class CanvasCommon : MonoBehaviour
{
    public void OnClick_Setting()
    {
        PopupManager.Instance.Create(PopupType.PopupSetting);
    }
    
}
