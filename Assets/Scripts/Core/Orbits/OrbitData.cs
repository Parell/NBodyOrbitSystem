using Unity.Collections;
using UnityEngine;

[System.Serializable]
public struct OrbitData
{
    public int index;
    public double mass;
    public Vector3d velocity;
    public Vector3d position;
    public Vector3d nextForce;

    public OrbitData(int index, double mass, Vector3d velocity, Vector3d position)
    {
        this.index = index;
        this.mass = mass;
        this.velocity = velocity;
        this.position = position;
        this.nextForce = Vector3d.zero;
    }

    public void CalculateForces(NativeArray<OrbitData> otherBodyData)
    {
        Vector3d force = Vector3d.zero;
        for (int i = 0; i < otherBodyData.Length; i++)
        {
            if (i == index) { continue; }
            force += CalculateForceOfGravity(position, mass, otherBodyData[i].position, otherBodyData[i].mass);
        }
        nextForce = force;
    }

    public void ApplyForces(float time)
    {
        double acceleration = nextForce.magnitude / mass;
        velocity += acceleration * time * nextForce.normalized;
        position += velocity * time;
    }

    public Vector3d CalculateForceOfGravity(Vector3d originBodyPos, double originBodyMass, Vector3d actingBodyPos, double actingBodyMass)
    {
        Vector3d direction = (actingBodyPos - originBodyPos).normalized;
        double force = Constants.G * (originBodyMass * actingBodyMass / (Vector3d.Distance(originBodyPos, actingBodyPos) * Vector3d.Distance(originBodyPos, actingBodyPos)));
        return force * direction;
    }
}

/* [System.Serializable]
public class KeplerianElements
{
    public OrbitalBody parent;
    public double specificMechanicalEnergy;
    public double semiMajorAxis;
    public double eccentricity;
    public Vector3d eccentricityVector;
    public Vector3d specificAngularMomentum;

    public void ToKeplerian(Vector3d r, Vector3d v)
    {
        if (parent == null) return;

        // set referance frame
        
        // Vector3d r = new Vector3d(position.x, position.y, position.z);
        // Vector3d v = new Vector3d(velocity.x, velocity.y, velocity.z);

        specificMechanicalEnergy = (Mathd.Pow(v.magnitude, 2) / 2) - ((parent.mass * Constants.G) / r.magnitude);
        semiMajorAxis = (-(parent.mass * Constants.G) / specificMechanicalEnergy) / 2;

        specificAngularMomentum = Vector3d.Cross(r, v);
        eccentricityVector = (1 / (parent.mass * Constants.G)) * Vector3d.Cross(v, specificAngularMomentum) - (r / Vector3d.Abs(r));
        eccentricity = eccentricityVector.magnitude;
    }
} */
