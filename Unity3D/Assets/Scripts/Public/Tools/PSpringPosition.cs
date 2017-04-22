//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spring-like motion -- the farther away the object is from the target, the stronger the pull.
/// </summary>

[AddComponentMenu("Custom/PSpringPosition")]
public class PSpringPosition : MonoBehaviour
{
    static public PSpringPosition current;

    /// <summary>
    /// Target position to tween to.
    /// </summary>

    public Vector3 target = Vector3.zero;

    /// <summary>
    /// Strength of the spring. The higher the value, the faster the movement.
    /// </summary>

    public float strength = 10f;

    /// <summary>
    /// Is the calculation done in world space or local space?
    /// </summary>

    public bool worldSpace = false;

    /// <summary>
    /// Whether the time scale will be ignored. Generally UI components should set it to 'true'.
    /// </summary>

    public bool ignoreTimeScale = false;

    public delegate void OnFinished(PSpringPosition go);

    /// <summary>
    /// Delegate to trigger when the spring finishes.
    /// </summary>

    public OnFinished onFinished;

    Transform mTrans;
    float mThreshold = 0f;

    /// <summary>
    /// Cache the transform.
    /// </summary>

    void Start()
    {
        mTrans = transform;
    }

    /// <summary>
    /// Advance toward the target position.
    /// </summary>

    void Update()
    {
        float delta = ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;

        if (worldSpace)
        {
            if (mThreshold == 0f) mThreshold = (target - mTrans.position).sqrMagnitude * 0.001f;
            mTrans.position = NGUIMath.SpringLerp(mTrans.position, target, strength, delta);

            if (mThreshold >= (target - mTrans.position).sqrMagnitude)
            {
                mTrans.position = target;
                NotifyListeners();
                enabled = false;
            }
        }
        else
        {
            if (mThreshold == 0f) mThreshold = (target - mTrans.localPosition).sqrMagnitude * 0.00001f;
            mTrans.localPosition = NGUIMath.SpringLerp(mTrans.localPosition, target, strength, delta);

            if (mThreshold >= (target - mTrans.localPosition).sqrMagnitude)
            {
                mTrans.localPosition = target;
                NotifyListeners();
                enabled = false;
            }
        }
    }

    /// <summary>
    /// Notify all finished event listeners.
    /// </summary>

    void NotifyListeners()
    {
        current = this;

        if (onFinished != null) onFinished(this);

        current = null;
    }

    /// <summary>
    /// Start the tweening process.
    /// </summary>

    static public PSpringPosition Begin(GameObject go, Vector3 pos, float strength)
    {
        PSpringPosition sp = go.GetComponent<PSpringPosition>();
        if (sp == null) sp = go.AddComponent<PSpringPosition>();
        sp.target = pos;
        sp.strength = strength;
        sp.onFinished = null;

        if (!sp.enabled)
        {
            sp.mThreshold = 0f;
            sp.enabled = true;
        }
        return sp;
    }
}
