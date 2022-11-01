using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitalManeuver : MonoBehaviour
{
    public OrbitalBody orbitalBody;
    public OrbitPropagator orbitPropagator;
    public ManeuverData maneuverData;

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

        // prop.maneuverData = maneuverData;

        // if (!maneuverData.done && prop.currentTime >= maneuverData.startTime)
        // {
        //     for (int i = 0; i < prop.orbitData.Length; i++)
        //     {
        //         prop.orbitData[0].cartesian.velocity += prop.orbitData[0].ApplyManeuver(maneuverData, Time.deltaTime);
        //     }

        //     // direction = ((Vector3)body.cartesian.velocity + direction).normalized;
        // }
        // else if (!done && prop.currentTime > (startTime + duration))
        // {
        //     done = true;
        // }
    }
}

[System.Serializable]
public struct ManeuverData
{
    public double acceleration;
    public double duration;
    public double startTime;
    public Vector3 direction;
    public bool done;
}
