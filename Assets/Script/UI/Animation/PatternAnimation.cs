using Service.Timer;
using UnityEngine;
using UnityEngine.UI;

enum PatternAnimationType
{
    LeftBottom_to_RightTop,
    LeftTop_to_RightBottom,
    RightTop_to_LeftBottom,
    RightBottom_to_LeftTop,
}

public class PatternAnimation : MonoBehaviour
{
    [SerializeField] private PatternAnimationType patternAnimationType;
    [SerializeField] private float speedX = 0.1f;
    [SerializeField] private float speedY = 0.1f;
    private RawImage _rawImage;
    private Rect _imgUVRect;

    private void Awake()
    {
        _rawImage = GetComponent<RawImage>();
        if(_rawImage)_imgUVRect = _rawImage.uvRect;
    }

    private void OnEnable()
    {
        if(_rawImage)_rawImage.uvRect = Rect.zero;
    }

    private void Update()
    {
        if(!_rawImage) return;
        SelectAnimation(patternAnimationType);
    }

    private void SelectAnimation(PatternAnimationType animationType_)
    {
        switch (animationType_)
        {
            case PatternAnimationType.LeftBottom_to_RightTop:
                _imgUVRect.x -= speedX * Time.deltaTime;
                _imgUVRect.y -= speedY * Time.deltaTime;
                break;
            case PatternAnimationType.LeftTop_to_RightBottom:
                _imgUVRect.x -= speedX * Time.deltaTime;
                _imgUVRect.y += speedY * Time.deltaTime;
                break;
            case PatternAnimationType.RightTop_to_LeftBottom:
                _imgUVRect.x += speedX * Time.deltaTime;
                _imgUVRect.y += speedY * Time.deltaTime;
                break;
            case PatternAnimationType.RightBottom_to_LeftTop:
                _imgUVRect.x += speedX * Time.deltaTime;
                _imgUVRect.y -= speedY * Time.deltaTime;
                break;
        }

        if(_rawImage)_rawImage.uvRect = _imgUVRect;
    }
}