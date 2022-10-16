using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRocketCalculator : MonoBehaviour
{
    public float force;

    const float Ru = 8314.462f; //J⋅(K⋅kmol)
    const float g0 = 9.806f;

    public float Mw = 2.016f;
    public float T0 = 3200; //K
    public float k = 1.66f;

    public float De = 3; //m
    public float Dt = 0.25f; //m
    public float Ae;
    public float At;
    public float Aratio;

    public float p0 = 5000000;
    public float pe = 1000;
    public float pa = 10000;

    public float R;
    public float density0;

    public float engineMass = 1200;

    public float mdot;
    public float ve;
    public float veq;
    public float isp;
    public float thrust;
    public float twr;
    public float mach;
    public float a;

    [ContextMenu("Cal Stats")]
    private void CalculateStats()
    {
        R = Ru / Mw;

        density0 = p0 / (R * T0); //kg/m3

        Ae = DiamToRadi(De);
        At = DiamToRadi(Dt);
        Aratio = Ae / At;

        mdot = (At * p0 / Mathf.Sqrt(R * T0) * Mathf.Sqrt(k * Mathf.Pow((1 + k) / 2, (1 + k) / (1 - k))));

        ve = Mathf.Sqrt(R * T0 * (2 * k / (k - 1)) * (1 - Mathf.Pow(pe / p0, (k - 1) / k)));

        veq = ve + (pe - pa) / mdot * Ae;
        isp = veq / g0;

        thrust = mdot * veq;
        twr = thrust / (engineMass * g0);

        a = Mathf.Sqrt(k * R * T0);
        mach = ve / a;
    }

    private float RadiToDiam(float value)
    {
        value = Mathf.Sqrt((value / Mathf.PI) * 4);
        return value;
    }

    private float DiamToRadi(float value)
    {
        value = (Mathf.PI * Mathf.Pow(value, 2)) / 4;
        return value;
    }
}
