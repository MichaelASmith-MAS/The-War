﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Terrain/Height Map Settings")]
public class HeightMapSettings : UpdatableSettings
{
    public float heightMultiplier;
    public bool useHeightCurve;
    public AnimationCurve heightCurve;

    public float MinHeight { get { return heightMultiplier * heightCurve.Evaluate(0); } }
    public float MaxHeight { get { return heightMultiplier * heightCurve.Evaluate(1); } }


//#if UNITY_EDITOR

//    protected override void OnValidate()
//    {
//        noiseSettings.ValidateValues();
//        base.OnValidate();

//    }

//#endif

}
