using UnityEngine;

public enum AnchorPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottonCenter,
    BottomRight,
    BottomStretch,

    VertStretchLeft,
    VertStretchRight,
    VertStretchCenter,

    HorStretchTop,
    HorStretchMiddle,
    HorStretchBottom,

    StretchAll
}
public static class RectTransformExtensions
{
    public static void SetAnchor(this RectTransform source, AnchorPresets allign, float offsetX = 0, float offsetY = 0)
    {
        source.anchoredPosition = new Vector3(offsetX, offsetY, 0);

        switch (allign)
        {
            case (AnchorPresets.TopLeft):
                {
                    source.anchorMin = new Vector2(0, 1);
                    source.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (AnchorPresets.TopCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 1);
                    source.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (AnchorPresets.TopRight):
                {
                    source.anchorMin = new Vector2(1, 1);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (AnchorPresets.MiddleLeft):
                {
                    source.anchorMin = new Vector2(0, 0.5f);
                    source.anchorMax = new Vector2(0, 0.5f);
                    break;
                }
            case (AnchorPresets.MiddleCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0.5f);
                    source.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (AnchorPresets.MiddleRight):
                {
                    source.anchorMin = new Vector2(1, 0.5f);
                    source.anchorMax = new Vector2(1, 0.5f);
                    break;
                }

            case (AnchorPresets.BottomLeft):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(0, 0);
                    break;
                }
            case (AnchorPresets.BottonCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0);
                    source.anchorMax = new Vector2(0.5f, 0);
                    break;
                }
            case (AnchorPresets.BottomRight):
                {
                    source.anchorMin = new Vector2(1, 0);
                    source.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (AnchorPresets.HorStretchTop):
                {
                    source.anchorMin = new Vector2(0, 1);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
            case (AnchorPresets.HorStretchMiddle):
                {
                    source.anchorMin = new Vector2(0, 0.5f);
                    source.anchorMax = new Vector2(1, 0.5f);
                    break;
                }
            case (AnchorPresets.HorStretchBottom):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (AnchorPresets.VertStretchLeft):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (AnchorPresets.VertStretchCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0);
                    source.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (AnchorPresets.VertStretchRight):
                {
                    source.anchorMin = new Vector2(1, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (AnchorPresets.StretchAll):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
        }
    }
    public static AnchorPresets GetAnchor(this RectTransform source)
    {
        var one = Vector2.one;
        var zero = Vector2.zero;
        var half = new Vector2(.5f, .5f);

        var zero_half = new Vector2(0, .5f);
        var half_zero = new Vector2(.5f, 0);

        var one_zero = new Vector2(1, 0);
        var zero_one = new Vector2(0, 1);

        var one_half = new Vector2(1, .5f);
        var half_one = new Vector2(.5f, 1);

        if (source.anchorMin == zero_one && source.anchorMax == zero_one)
        {
            return AnchorPresets.TopLeft;
        }

        if (source.anchorMin == half_one && source.anchorMax == half_one)
        {
            return AnchorPresets.TopCenter;
        }

        if (source.anchorMin == one && source.anchorMax == one)
        {
            return AnchorPresets.TopRight;
        }

        if (source.anchorMin == zero_half && source.anchorMax == zero_half)
        {
            return AnchorPresets.MiddleLeft;
        }

        if (source.anchorMin == half && source.anchorMax == half)
        {
            return AnchorPresets.MiddleCenter;
        }

        if (source.anchorMin == one_half && source.anchorMax == one_half)
        {
            return AnchorPresets.MiddleRight;
        }

        if (source.anchorMin == zero && source.anchorMax == zero)
        {
            return AnchorPresets.BottomLeft;
        }

        if (source.anchorMin == half_zero && source.anchorMax == half_zero)
        {
            return AnchorPresets.BottonCenter;
        }

        if (source.anchorMin == one_zero && source.anchorMax == one_zero)
        {
            return AnchorPresets.BottomRight;
        }

        if (source.anchorMin == zero_one && source.anchorMax == one)
        {
            return AnchorPresets.HorStretchTop;
        }

        if (source.anchorMin == zero_half && source.anchorMax == one_half)
        {
            return AnchorPresets.HorStretchMiddle;
        }

        if (source.anchorMin == zero && source.anchorMax == zero_one)
        {
            return AnchorPresets.HorStretchBottom;
        }

        if (source.anchorMin == zero && source.anchorMax == zero_one)
        {
            return AnchorPresets.VertStretchLeft;
        }

        if (source.anchorMin == half_zero && source.anchorMax == half_one)
        {
            return AnchorPresets.VertStretchCenter;
        }

        if (source.anchorMin == one_zero && source.anchorMax == one)
        {
            return AnchorPresets.VertStretchRight;
        }

        if (source.anchorMin == zero && source.anchorMax == one)
        {
            return AnchorPresets.StretchAll;
        }
        return AnchorPresets.StretchAll;
    }
}