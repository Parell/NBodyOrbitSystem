using UnityEngine;

[System.Serializable]
public struct OrbitData
{
    public string name;
    public int index;
    public double mass;
    public Vector3d velocity;
    public Vector3d position;
    public Vector3d acceleration;

    public OrbitData(string name, int index, double mass, Vector3d velocity, Vector3d position)
    {
        this.name = name;
        this.index = index;
        this.mass = mass;
        this.velocity = velocity;
        this.position = position;
        this.acceleration = Vector3d.zero;
    }

    public Vector3d Gravity(Vector3d otherPosition, double otherMass)
    {
        var newAcceleration = Vector3d.zero;

        Vector3d r = (otherPosition - position);
        newAcceleration += (r * (Constant.G * otherMass)) / (r.magnitude * r.magnitude * r.magnitude);

        return newAcceleration;
    }

    public void Integrate(double deltaTime, IntegratorType integrator, OrbitData[] orbitData)
    {
        if (integrator == IntegratorType.Euler)
        {
            for (int i = 0; i < orbitData.Length; i++)
            {
                if (i == index) { continue; }

                acceleration += Gravity(orbitData[i].position, orbitData[i].mass);

                position += deltaTime * velocity;
                velocity += deltaTime * acceleration;

                acceleration = Vector3d.zero;
            }
        }
        else if (integrator == IntegratorType.SemiImplicitEuler)
        {
            for (int i = 0; i < orbitData.Length; i++)
            {
                if (i == index) { continue; }

                acceleration += Gravity(orbitData[i].position, orbitData[i].mass);

                position += deltaTime * (velocity + acceleration * deltaTime);
                velocity += deltaTime * acceleration;

                acceleration = Vector3d.zero;
            }
        }
        else if (integrator == IntegratorType.RungeKutta4)
        {
            for (int i = 0; i < orbitData.Length; i++)
            {
                if (i == index) { continue; }

                Vector3d k1, k2, k3, k4;

                k1 = deltaTime * A(i, orbitData, orbitData[i].velocity, 0);
                k2 = deltaTime * A(i, orbitData, orbitData[i].velocity + (deltaTime / 2) * k1, (deltaTime / 2));
                k3 = deltaTime * A(i, orbitData, orbitData[i].velocity + (deltaTime / 2) * k2, (deltaTime / 2));
                k4 = deltaTime * A(i, orbitData, orbitData[i].velocity + deltaTime * k3, deltaTime);

                velocity += (deltaTime / 6) * (k1 + (2 * k2) + (2 * k3) + k4);

                k1 = deltaTime * V(i, orbitData, orbitData[i].position, 0);
                k2 = deltaTime * V(i, orbitData, orbitData[i].position + (deltaTime / 2) * k1, (deltaTime / 2));
                k3 = deltaTime * V(i, orbitData, orbitData[i].position + (deltaTime / 2) * k2, (deltaTime / 2));
                k4 = deltaTime * V(i, orbitData, orbitData[i].position + deltaTime * k3, deltaTime);

                position += (deltaTime / 6) * (k1 + (2 * k2) + (2 * k3) + k4);
            }
        }
    }

    private Vector3d V(int i, OrbitData[] orbitData, Vector3d vector, double deltaTime)
    {
        return velocity + Gravity(vector, orbitData[i].mass) * deltaTime;
    }

    private Vector3d A(int i, OrbitData[] orbitData, Vector3d vector, double deltaTime)
    {
        return Gravity(vector, orbitData[i].mass) + Gravity(vector + V(i, orbitData, orbitData[i].position, deltaTime) * deltaTime, orbitData[i].mass) * deltaTime;
    }

    // Create history of points

    public void AddConstantAcceleration(Maneuver maneuver, double time)
    {
        maneuver.direction = (maneuver.deltaV + (Vector3)velocity.normalized).normalized;
        Debug.DrawLine((Vector3)position, (Vector3)position + maneuver.direction * maneuver.deltaV.magnitude * 10, Color.red);
        //velocity += (Vector3d)(maneuver.acceleration * (Vector3d)maneuver.direction * time);
        velocity += (Vector3d)maneuver.direction * (maneuver.deltaV.magnitude);
    }
}

[System.Serializable]
public struct KeplerianElements
{
    public double SMA;
    public double e;
    public double w;
    public double LAN;
    public double i;
    public double M;
    public double E;
    public double v;
    public double T;
    public OrbitBody parent;
}
