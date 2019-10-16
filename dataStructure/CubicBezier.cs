/** CubicBezier.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/16 15:43:13
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CubicBezier {
    
    [SerializeField]
    public Vector2 hIn {
        get { return m_hIn;}
        set {
            m_hIn = value;
            ReCalculate();
        }
    }
    [SerializeField]
    private Vector2 m_hIn = Vector2.right;
    
    public Vector2 hOut {
        get { return m_hOut;}
        set { 
            m_hOut = value;
            ReCalculate();
        }
    }
    [SerializeField]
    private Vector2 m_hOut = Vector2.right;

    public Vector3 h1Exp { get; protected set; } = Vector3.negativeInfinity;
    public Vector3 h2Exp { get; protected set; } = Vector3.negativeInfinity;

    private float accuracy = 0.001f;

    public CubicBezier(Vector2 handle1, Vector2 handle2, float accuracy = 0.001f) {
        hIn = handle1;
        hOut = handle2;
        this.accuracy = accuracy;
        ReCalculate();
    }

    public CubicBezier ReCalculate() {
        if (m_hIn.x < 0 || m_hIn.x > 1) {
            m_hIn = new Vector2(Mathf.Clamp01(m_hIn.x), m_hIn.y);
        }
        if (m_hOut.x < 0 || m_hOut.x > 1) {
            m_hOut = new Vector2(Mathf.Clamp01(m_hOut.x), m_hOut.y);
        }
        var z1 = 3f * hIn.x;
        var y1 = 3f * (hOut.x - hIn.x) - h1Exp.z;
        var x1 = 1f - h1Exp.z - h1Exp.y;
        this.h1Exp = new Vector3(x1, y1, z1);

        var z2 = 3f * hIn.y;
        var y2 = 3f * (hOut.y - hIn.y) - h2Exp.z;
        var x2 = 1f - h2Exp.z - h2Exp.y;
        this.h2Exp =  new Vector3(x2, y2, z2);
        return this;
    }

    public float GetX(float t) {
        if (h1Exp == Vector3.negativeInfinity || h2Exp == Vector3.negativeInfinity) ReCalculate();
        return ((h1Exp.x * t + h1Exp.y) * t + h1Exp.z) * t; // Horner's Rule
    }

    public float GetY(float t) {
        if (h1Exp == Vector3.negativeInfinity || h2Exp == Vector3.negativeInfinity) ReCalculate();
        return ((h2Exp.x * t + h2Exp.y) * t + h2Exp.z) * t;
    }

    public float Evaluate(float x) {
        return GetY(FindTOfX(x));
    }

    public float GetDerivativeX(float t) {
        return (3f * h1Exp.x * t + 2f * h1Exp.y) * t + h1Exp.z;
    }

    /// <summary>
    /// Find the t value of given x
    /// </summary>
    public float FindTOfX(float x) {
        if (x < 0) return 0;
        if (x > 1) return 1;

        // Newton's method
        var tTemp = x;
        for (var i = 0; i < 8; i++) {
            var offset = GetX(tTemp) - x;
            if (SmallEnough(offset)) return tTemp;
            var dx = GetDerivativeX(tTemp);
            if (SmallEnough(dx, 0.00001f)) break;
            tTemp = tTemp - offset / dx;
        }

        // Fallback to Bisection.
        tTemp = x;
        var tFrom = 0f;
        var tTo = 1f;
        while (tFrom < tTo) {
            var xAtTTemp = GetX(tTemp);
            if (SmallEnough(xAtTTemp - x)) return tTemp;

            if (x > xAtTTemp) tFrom = tTemp;
            else tTo = tTemp;
            tTemp = Mathf.Lerp(tFrom, tTo, 0.5f);
        }

        // Failure.
        return tTemp;
    }

    private bool SmallEnough(float delta) {
        return SmallEnough(delta, accuracy);
    }

    private bool SmallEnough(float delta, float limit) {
        return -limit < delta && delta < limit;
    }

    public List<Vector2> GetSample(int split) {
        if (split <= 0) return null;
        var distance = 1f / split;
        var ret = new List<Vector2>();
        for (var i = 0; i <= split; i++) {
            var tPos = i * distance;
            ret.Add(new Vector2(GetX(tPos), GetY(tPos)));
        }

        return ret;
    }
}