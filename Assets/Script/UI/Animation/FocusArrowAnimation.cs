using DG.Tweening;
using UnityEngine;

public enum FocusArrowType
{
    None,
    ScaleXY,
}
public class FocusArrowAnimation : MonoBehaviour
{
    public FocusArrowType focusType;
    public float duration = 0.3f;
    public float distance = 20f;
    public float widthMax = 1.25f;
    public float heightMin = 0.8f;
    
    private float _startPosX;
    private float _startPosY;
    private float _targetY;

    private void Awake()
    {
        _startPosX = transform.localPosition.x;
        _startPosY = transform.localPosition.y;
        _targetY = _startPosY;
        _targetY -= distance;
    }

    void OnEnable()
    {
        transform.DOLocalMoveX(_startPosX, 0).SetUpdate(true);
        transform.DOLocalMoveY(_targetY, duration).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);

        switch (focusType)
        {
            case FocusArrowType.ScaleXY:
                transform.DOScale(Vector3.one, 0f);
                transform.DOScaleX(widthMax, 0.3f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
                transform.DOScaleY(heightMin, 0.3f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
                break;
        }
    }

    private void OnDisable()
    {
        transform.DOKill();
        Vector3 pos = Vector3.zero;
        pos.y = _startPosY;
        transform.localPosition = pos;
    }
}