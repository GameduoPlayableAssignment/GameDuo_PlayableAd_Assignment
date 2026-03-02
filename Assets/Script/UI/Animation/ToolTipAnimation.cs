using DG.Tweening;
using Helper;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 툴팁에다가 컴포넌트 추가하고 버튼 온클릭에 OnToolTip 추가
/// 툴팁 피벗 조절
/// </summary>
public class ToolTipAnimation : MonoBehaviour
{
    [SerializeField] private string textKey;

    private Image _tooltip;
    private Coroutine _autoCloseCo;
    private TMP_Text textTooltip;
    private const float AnimDuration = 0.2f;
    private bool _isOn;
    private bool _isInit;

    private void Awake()
    {
        if (gameObject.activeSelf && !_isInit)
            gameObject.SetActive(false);
    }

    private void Init()
    {
        textTooltip = GetComponentInChildren<TMP_Text>();
        _tooltip = GetComponent<Image>();
        _tooltip.DOKill();
        _tooltip.DOFade(0f, 0f);
        _tooltip.transform.DOScale(0f, 0f);
        _tooltip.gameObject.SetActive(false);
        SetText();
        _isInit = true;
    }

    private void SetText()
    {
        if (textKey != "" && textTooltip != null)
        {
            textTooltip.text = $"{CommonHelper.Translation(textKey)}";
        }
    }

    public void SetToolTip()
    {
        if (_isOn)
        {
            _tooltip.DOFade(1f, AnimDuration).SetEase(Ease.OutCubic).SetUpdate(true);
            _tooltip.transform.DOScale(1f, AnimDuration).SetEase(Ease.OutBack).SetUpdate(true);
            _tooltip.gameObject.SetActive(_isOn);
        }
        else
        {
            _tooltip.DOFade(0f, AnimDuration * 0.5f).SetEase(Ease.OutCubic).SetUpdate(true);
            _tooltip.transform.DOScale(0f, AnimDuration * 0.5f).SetEase(Ease.OutSine).OnComplete(() => _tooltip.gameObject.SetActive(_isOn)).SetUpdate(true);
        }
    }

    IEnumerator AutoClose()
    {
        yield return new WaitForSecondsRealtime(3f);
        OnToolTip();
    }

    IEnumerator ClickCheck()
    {
        while (_isOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerData = new (EventSystem.current);
                pointerData.position = Input.mousePosition;
                
                List<RaycastResult> results = new ();
                EventSystem.current.RaycastAll(pointerData, results);

                if (results.Count == 0)
                {
                    OnToolTip();
                    break;
                }
                
                if (results[0].gameObject.CompareTag("ToolTipAnimationException"))
                {
                    yield return null;
                    continue;
                }
                
                OnToolTip();
                break;
            }

            yield return null;
        }
    }

    public void OnToolTip()
    {
        if (!_isInit) Init();
        
        if (!_tooltip.gameObject.activeSelf)
        {
            _isOn = true;
            SetToolTip();
            _autoCloseCo = StartCoroutine(AutoClose());
            StartCoroutine(ClickCheck());
        }
        else
        {
            _isOn = false;
            SetToolTip();
            StopCoroutine(_autoCloseCo);
        }
    }
}