using UnityEngine;

[System.Serializable]
public struct OrbitData
{
    public Cartesian cartesian;
    // public Keplerian keplerian;
    public Vector3d nextAcceleration;
    public int index;

    public OrbitData(Cartesian cartesian, /* Keplerian keplerian, */ int index)
    {
        this.cartesian = cartesian;
        // this.keplerian = keplerian;
        this.nextAcceleration = Vector3d.zero;
        this.index = index;
    }

    public void CalculateGravitationalAcceleration(OrbitData[] orbitData)
    {
        Vector3d acceleration = Vector3d.zero;
        for (int i = 0; i < orbitData.Length; i++)
        {
            if (i == index) { continue; }

            Vector3d r = (orbitData[i].cartesian.position - cartesian.position);

            acceleration += (r * (Constant.G * orbitData[i].cartesian.mass)) / (r.magnitude * r.magnitude * r.magnitude);
        }
        nextAcceleration = acceleration;
    }

    public void AddForce(float time, Integrator integrator)
    {
        if (integrator == Integrator.None)
        {
            //double acceleration = nextAcceleration.magnitude / cartesian.mass;
            cartesian.velocity += nextAcceleration * time;
            cartesian.position += cartesian.velocity * time;
        }
        else if (integrator == Integrator.RK4)
        {
            
        }
        else if (integrator == Integrator.RKDP45)
        {
        }
    }

    // public Vector3d ApplyManeuver(ManeuverData maneuverData, float time)
    // {
    //     Vector3d velocity = new Vector3d();
    //     velocity += maneuverData.acceleration * time * (Vector3d)cartesian.velocity.normalized;
    //     return velocity;
    // }

    // public void CartesianToKeplerian(Cartesian cartesian, Cartesian centralCartesian)
    // {
    //     double mu = centralCartesian.mass * Constant.G;

    //     Vector3d h = Vector3d.Cross(cartesian.position, cartesian.velocity);

    //     Vector3d e = (Vector3d.Cross(cartesian.velocity, h) / mu) - (cartesian.position / cartesian.position.magnitude);

    //     keplerian.e = e.magnitude;

    //     Vector3d n = new Vector3d(-h.x, h.y, 0);

    //     if (Vector3d.Dot(cartesian.position, cartesian.velocity) >= 0)
    //     {
    //         keplerian.trueAnomaly = Mathd.Acos(Vector3d.Dot(e, cartesian.position) / (e.magnitude * cartesian.position.magnitude));
    //     }
    //     else
    //     {
    //         keplerian.trueAnomaly = (2 * Mathd.PI) - Mathd.Acos(Vector3d.Dot(e, cartesian.position) / (e.magnitude * cartesian.position.magnitude));
    //     }

    //     keplerian.i = Mathd.Acos(h.y / h.magnitude);

    //     keplerian.eAnomaly = 2 * Mathd.Acos(Mathd.Tan(keplerian.trueAnomaly / 2) / Mathd.Sqrt((1 + keplerian.e) / (1 - keplerian.e)));

    //     if (n.x >= 0)
    //     {
    //         keplerian.LAN = Mathd.Acos(n.y / n.magnitude);
    //     }
    //     else
    //     {
    //         keplerian.LAN = (2 * Mathd.PI) - Mathd.Acos(n.y / n.magnitude);
    //     }

    //     if (e.z >= 0)
    //     {
    //         keplerian.w = Mathd.Acos(Vector3d.Dot(n, e) / n.magnitude * e.magnitude);
    //     }
    //     else
    //     {
    //         keplerian.w = (2 * Mathd.PI) - Mathd.Acos(Vector3d.Dot(n, e) / n.magnitude * e.magnitude);
    //     }

    //     keplerian.meanAnomaly = keplerian.eAnomaly - (keplerian.e * Mathd.Sin(keplerian.eAnomaly));

    //     keplerian.a = 1 / ((2 / cartesian.position.magnitude) - (Mathd.Pow(cartesian.velocity.magnitude, 2) / mu));
    // }
}

[System.Serializable]
public struct Cartesian
{
    public double mass;
    public Vector3d velocity;
    public Vector3d position;
}

// [System.Serializable]
// public struct Keplerian
// {
//     public double a;
//     public double e;
//     public double i;
//     public double LAN;
//     public double w;
//     public double trueAnomaly;
//     public double eAnomaly;
//     public double meanAnomaly;
// }
