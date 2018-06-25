﻿using UnityEngine;

/// <summary> Base MonoBehaviour class. </summary>
public abstract class BaseMonoBehaviour : MonoBehaviour
{
    /// <summary> Base Awake() will call AddListeners() and then Initialization(). </summary>
    protected virtual void Awake()
    {
        PreInitialization();
        AddEvents();
        AddListeners();
        Initialization();
    }

    /// <summary> It is usually used to get components. </summary>
    protected virtual void PreInitialization() {}

    ///<summary> Second Method called in the base Awake() </summary>
    protected virtual void AddEvents() {}

    /// <summary> It is used to add listeners to events. </summary>
    protected virtual void AddListeners() {} 

    /// <summary> It is used to common actions on Awake(). </summary>
    protected virtual void Initialization() {}
}