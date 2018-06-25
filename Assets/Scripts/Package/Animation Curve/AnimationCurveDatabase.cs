using UnityEngine;

public enum Curve
{
    Linear = 0,
    EaseIn = 1,
    EaseOut = 2,
    EaseInOut = 3
}

public class AnimationCurveDatabase : ScriptableObject
{
    public AnimationCurve linear;
    public AnimationCurve easeIn;
    public AnimationCurve easeOut;
    public AnimationCurve easeInOut;
}