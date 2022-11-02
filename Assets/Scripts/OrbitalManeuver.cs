using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitalManeuver : MonoBehaviour
{
    public OrbitalBody orbitalBody;
    public OrbitPropagator orbitPropagator;
    public Maneuver maneuver;

    private void Update()
    {
        if (orbitalBody.a >= 0)
        {
            orbitPropagator.steps = (int)((orbitalBody.T) / orbitPropagator.stepSize);
        }
        else if (orbitalBody.a < 0)
        {
            orbitPropagator.steps = 1000;
        }

        orbitPropagator.maneuver = maneuver;
    }
}

[System.Serializable]
public struct Maneuver
{
    public float acceleration;
    public double duration;
    public double startTime;
    public string altitudeLock;
    public Vector3 direction;
}

public enum Direction
{
    Prograde,
    Retrograde,
    Anti_Normal,
    Normal,
    Anti_Radial,
    Radial
}
