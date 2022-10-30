using UnityEngine;

public class Physics
{
    public static Vector3d CalculateForceOfGravity(Vector3d originBodyPos, double originBodyMass, Vector3d actingBodyPos, double actingBodyMass)
    {
        Vector3d direction = (actingBodyPos - originBodyPos).normalized;
        double distance = Vector3d.Distance(originBodyPos, actingBodyPos);
        double force = Constant.G * (originBodyMass * actingBodyMass / distance * distance);
        return force * direction;
    }
}
