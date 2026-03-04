using System;
using Cysharp.Threading.Tasks;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialHand : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic skeletonGraphic;
    
    private bool _isFirst;
    private Transform _target;
    
    private void Update()
    {
        if (_isFirst)
        {
            gameObject.transform.position = _target.position;
            return;
        }
        
        bool pressed = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                    || (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame);

        if (pressed)
        {
            _SetStep2();
            _isFirst = true;
        }
    }

    private void _SetStep2()
    {
        _SetHandAnimation();
        HideHand();
    }
    
    private void _SetHandAnimation(string animationName = "Touch")
    {
        skeletonGraphic.AnimationState.SetAnimation(0, animationName, true).MixDuration = 0f;
    }

    public void ShowHand(Transform target)
    {
        if (!_isFirst)
        {
            _SetStep2();
            _isFirst = true;
        }
            
        _target = target;
        gameObject.SetActive(true);
    }
    
    public void HideHand()
    {
        gameObject.SetActive(false);
    }
}
