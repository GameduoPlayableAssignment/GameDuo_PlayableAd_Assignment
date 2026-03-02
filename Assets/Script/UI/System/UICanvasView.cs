using UnityEngine;
using UnityEngine.UI;

public class UICanvasView : MonoBehaviour
{
    public RectTransform Rect { get; set; }
    public Canvas[] Canvas { get; set; }
    public GraphicRaycaster GraphicRaycaster { get; set; }

    private void OnEnable()
    {
        if(Rect == null) Rect = GetComponent<RectTransform>();
        if(Canvas == null) Canvas = GetComponentsInChildren<Canvas>(true);
        if(GraphicRaycaster == null) GraphicRaycaster = GetComponent<GraphicRaycaster>();
    }

    public void SetView(bool isOn)
    {
        for (int i = 0; i < Canvas.Length; i++) Canvas[i].enabled = isOn;
        if(GraphicRaycaster) GraphicRaycaster.enabled = isOn;
    }
}