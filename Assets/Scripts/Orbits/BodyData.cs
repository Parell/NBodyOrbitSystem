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
            for (int i = 0; i < bodyData.Length; i++)
            {
                if (i == index) { continue; }

                Vector3d k1, k2, k3, k4;

                k1 = a(bodyData, position, 0);
                // k2 = a(bodyData, )
            }
        }
    }

    private Vector3d a(BodyData[] bodyData, Vector3d position, double deltaTime)
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

    // private Vector3d V(int i, BodyData[] bodyData, Vector3d vector, double deltaTime)
    // {
    //     //return velocity + Gravity(vector, bodyData[i].mass);
    // }

    // private Vector3d A(int i, BodyData[] bodyData, Vector3d vector, double deltaTime)
    // {
    //     //return Gravity(vector, bodyData[i].mass) + Gravity(vector + V(i, bodyData, bodyData[i].position, deltaTime), bodyData[i].mass);
    // }
}
