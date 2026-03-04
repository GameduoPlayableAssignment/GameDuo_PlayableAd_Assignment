using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private UIButton btn0;
    [SerializeField] private UIButton btn1;
    [SerializeField] private UIButton btn2;

    [SerializeField] private TMP_Text text_desc;
    [SerializeField] private TMP_Text text_desc1;
    [SerializeField] private TMP_Text text_desc2;

    UpgradePhase currentPhase;
    UpgradeOption[] currentOpts;
    System.Action<UpgradePhase, UpgradeOption> onPick;

    public void Show(UpgradePhase phase, UpgradeOption[] options, System.Action<UpgradePhase, UpgradeOption> onPickCb)
    {
        currentPhase = phase;
        currentOpts = options;
        onPick = onPickCb;

        text_desc.text = $"{options[0].Title}\n<size=70%>{options[0].Desc}</size>";
        text_desc1.text = $"{options[1].Title}\n<size=70%>{options[1].Desc}</size>";
        text_desc2.text = $"{options[2].Title}\n<size=70%>{options[2].Desc}</size>";
        
        btn0.onClick.RemoveAllListeners();
        btn1.onClick.RemoveAllListeners();
        btn2.onClick.RemoveAllListeners();

        btn0.onClick.AddListener(() => _Pick(0));
        btn1.onClick.AddListener(() => _Pick(1));
        btn2.onClick.AddListener(() => _Pick(2));

        _BtnAnim(true);
        
        root.SetActive(true);
    }

    public void Hide()
    {
        root.SetActive(false);
        _BtnAnim(false);
    }

    private void _BtnAnim(bool isOn)
    {
        btn0.transform.DOScale(isOn ? 1 : 0, 0.2f).SetUpdate(true);
        btn1.transform.DOScale(isOn ? 1 : 0, 0.2f).SetUpdate(true);
        btn2.transform.DOScale(isOn ? 1 : 0, 0.2f).SetUpdate(true);
    }

    private void _Pick(int idx)
    {
        onPick?.Invoke(currentPhase, currentOpts[idx]);
    }
}