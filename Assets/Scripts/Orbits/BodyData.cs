using System;
using UnityEngine;

[Serializable]
public struct BodyData
{
    public int index;
    public double mass;
    public Vector3d velocity;
    public Vector3d position;
    private Vector3d acceleration;

    public BodyData(int index, double mass, Vector3d velocity, Vector3d position)
    {
        this.index = index;
        this.mass = mass;
        this.velocity = velocity;
        this.position = position;
        acceleration = Vector3d.zero;
    }

    private Vector3d Gravity(BodyData[] bodyData)
    {
        var newAcceleration = Vector3d.zero;

        for (int i = 0; i < bodyData.Length; i++)
        {
            if (i == index) { continue; }

            Vector3d r = (bodyData[i].position - position);

            newAcceleration += (r * (Constant.G * bodyData[i].mass)) / (r.magnitude * r.magnitude * r.magnitude);
        }

        return newAcceleration;
    }

    public void Integrator(BodyData[] bodyData, float deltaTime, IntegratorType integratorType)
    {
        if (integratorType == IntegratorType.Euler)
        {
            acceleration += Gravity(bodyData);

            position += deltaTime * velocity;
            velocity += deltaTime * acceleration;

            acceleration = Vector3d.zero;
        }
        else if (integratorType == IntegratorType.SemiImplicitEuler)
        {
            acceleration += Gravity(bodyData);

            position += deltaTime * (velocity + acceleration * deltaTime);
            velocity += deltaTime * acceleration;

            acceleration = Vector3d.zero;
        }
        else if (integratorType == IntegratorType.RungeKutta4)
        {
            // Vector3d dx1, dv1, dx2, dv2, dx3, dv3, dx4, dv4;

            // acceleration += Gravity(bodyData);

            // dx1 = deltaTime * velocity;
            // dv1 = deltaTime * acceleration;
            // dx2 = deltaTime * velocity + (dv1 / 2);


            // position = position;

            // velocity = velocity;

            // acceleration = Vector3d.zero;
        }
    }

    // public void AddConstantAcceleration(Maneuver maneuver, double time)
    // {
    //     maneuver.direction = (maneuver.deltaV + (Vector3)velocity.normalized).normalized;
    //     Debug.DrawLine((Vector3)position, (Vector3)position + maneuver.direction * maneuver.deltaV.magnitude * 10, Color.red);
    //     //velocity += (Vector3d)(maneuver.acceleration * (Vector3d)maneuver.direction * time);
    //     velocity += (Vector3d)maneuver.direction * (maneuver.deltaV.magnitude);
    // }
}

[Serializable]
public struct Cartesian
{
    public Vector3d velocity;
    public Vector3d position;
}

[Serializable]
public struct Keplerian
{
    public double a;
    public double e;
    public double w;
    public double LAN;
    public double i;
    public double meanAnomaly;
    public double eAnomaly;
    public double trueAnomaly;
    public double T;
    public Body centralBody;
}