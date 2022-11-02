using Buttons;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitalBody : MonoBehaviour
{
    public double mass;
    public Vector3d velocity;
    public Vector3d position;
    [Tooltip("Semi-major axis")]
    public double a;
    [Tooltip("Eccentricity")]
    public double e;
    [Tooltip("Argument of periapsis")]
    public double w;
    [Tooltip("Longitude of ascending node")]
    public double LAN;
    [Tooltip("Inclination")]
    public double i;
    [Tooltip("Mean anomaly")]
    public double M;
    [Tooltip("Eccentricity anomaly")]
    public double E;
    [Tooltip("True anomaly")]
    public double v;
    [Tooltip("Orbital period")]
    public double T;
    public OrbitalBody parent;

    [HideInInspector]
    public Transform scaledTransform;

    private void Update()
    {
        CartesianToKeplerian(parent);

        if (Application.isPlaying)
        {
            transform.position = (Vector3)(position - FloatingOrigin.Instance.originPosition);
            scaledTransform.position = (Vector3)(position / Constant.Scale - FloatingOrigin.Instance.originPositionScaled);

        }
        else
        {
            transform.position = scaledTransform.position * Constant.Scale;
            position = (Vector3d)transform.position;
        }
    }

    [Button(Mode = ButtonMode.DisabledInPlayMode, Spacing = ButtonSpacing.Before)]
    public void GenerateScaled()
    {
        GameObject.DestroyImmediate(GameObject.Find(gameObject.name + "_Scaled"));

        GameObject scaledObject = new GameObject(gameObject.name + "_Scaled");
        scaledObject.transform.parent = GameObject.Find("Scaled").transform;
        scaledObject.transform.position = transform.position / Constant.Scale;
        scaledObject.transform.localScale = Vector3.one / Constant.Scale;
        scaledObject.transform.rotation = transform.rotation;
        scaledObject.layer = 6;
        scaledTransform = scaledObject.transform;
    }

    public void CartesianToKeplerian(OrbitalBody parent)
    {
        if (parent == null) { return; }

        var mu = parent.mass * Constant.G;


        var r_vec = position;
        var v_vec = velocity;

        var hVec = Vector3d.Cross(position, velocity);
        var eVec = (Vector3d.Cross(velocity, hVec) / mu) - (position / position.magnitude);

        var h = hVec.magnitude;
        e = eVec.magnitude;

        var n = new Vector3d(-hVec.x, hVec.y, 0);

        i = Mathd.Acos(hVec.z / h) * (180 / Mathd.PI);

        if (eVec.z >= 0)
        {
            w = Mathd.Acos(Vector3d.Dot(n, eVec) / n.magnitude * e);
        }
        else if (eVec.z < 0)
        {
            w = (2 * Mathd.PI) - Mathd.Acos(Vector3d.Dot(n, eVec) / n.magnitude * e);
        }

        if (n.x >= 0)
        {
            LAN = Mathd.Acos(n.y / n.magnitude);
        }
        else if (n.x < 0)
        {
            LAN = (2 * Mathd.PI) - Mathd.Acos(n.y / n.magnitude);
        }

        if (h >= 0)
        {
            v = Mathd.Acos(Vector3d.Dot(eVec, position) / (e * position.magnitude));
        }
        else if (h < 0)
        {
            v = (2 * Mathd.PI) - Mathd.Acos(Vector3d.Dot(eVec, position) / (e * position.magnitude));
        }

        E = Mathd.Atan2(Mathd.Tan(v / 2), Mathd.Sqrt((1 + e) / (1 - e)));

        M = E - e * Mathd.Sin(E);

        E = E * (180 / Mathd.PI);
        M = M * (180 / Mathd.PI);
        v = v * (180 / Mathd.PI);
        LAN = LAN * (180 / Mathd.PI);
        w = w * (180 / Mathd.PI);

        a = 1 / ((2 / position.magnitude) - (Mathd.Pow(velocity.magnitude, 2) / mu));

        var r = Vector3d.Distance(position, parent.position);

        T = Mathd.Sqrt((4 * (Mathd.PI * Mathd.PI)) / mu * (a * a * a));
    }
}
