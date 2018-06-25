using UnityEngine;
using Package.EventManager;
using System.Collections.Generic;

public enum AnimationCurveTopics
{
    /// <summary> 1 Result = AnimationCurve | 1 Parameter = Curve (Enum) </summary>
    GetAnimationCurveRequest = 0
}

public class AnimationCurveManager : BaseMonoBehaviour
{
    [SerializeField] private AnimationCurveDatabase database;
    private Dictionary<Curve, AnimationCurve> animationCurveDictionary;

    protected override void PreInitialization()
    {
        base.PreInitialization();

        animationCurveDictionary = new Dictionary<Curve, AnimationCurve>
        {
            { Curve.Linear, database.linear },
            { Curve.EaseIn, database.easeIn },
            { Curve.EaseOut, database.easeOut },
            { Curve.EaseInOut, database.easeInOut }
        };
    }

    protected override void AddEvents()
    {
        base.AddEvents();

        ServerPattern.AddEvent<AnimationCurve, Curve>(AnimationCurveTopics.GetAnimationCurveRequest, TopicType.Function);
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        ServerPattern.AddListener<AnimationCurve, Curve>(AnimationCurveTopics.GetAnimationCurveRequest, _curve => animationCurveDictionary[_curve]);
    }
}