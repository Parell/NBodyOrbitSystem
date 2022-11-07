using Buttons;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitBody : MonoBehaviour
{
    public double mass = 1;
    public Vector3d velocity;
    public Vector3d position;
    public KeplerianElements keplerian;
    public Transform scaledTransform;

    [Button(Mode = ButtonMode.DisabledInPlayMode, Spacing = ButtonSpacing.Before)]
    public void GenerateScaled()
    {
        GameObject.DestroyImmediate(GameObject.Find(gameObject.name + "_Scaled"));

        GameObject scaledObject = new GameObject(gameObject.name + "_Scaled");
        scaledObject.transform.parent = GameObject.Find("Scaled").transform;
        scaledObject.transform.position = transform.position / Constant.Scale;
        scaledObject.transform.localScale = Vector3.one / Constant.Scale;
        scaledObject.transform.rotation = transform.rotation;
        scaledObject.layer = 0;
        scaledTransform = scaledObject.transform;
    }

    private void Update()
    {
        CartesianToKeplerian(keplerian.parent);

        // KE = (Constant.G * keplerian.parent.mass * mass) / (2 * keplerian.SMA);
        // PE = (-Constant.G * keplerian.parent.mass * mass) / (keplerian.SMA);
        // E = keplerian.KE + keplerian.PE;

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

    public void CartesianToKeplerian(OrbitBody parent)
    {
        if (parent == null) { return; }

        var mu = parent.mass * Constant.G;

        var r_vec = position;
        var v_vec = velocity;

        var hVec = Vector3d.Cross(position, velocity);
        var eVec = (Vector3d.Cross(velocity, hVec) / mu) - (position / position.magnitude);

        var h = hVec.magnitude;
        keplerian.e = eVec.magnitude;

        var n = new Vector3d(-hVec.x, hVec.y, 0);

        keplerian.i = Mathd.Acos(hVec.z / h) * (180 / Mathd.PI);

        if (eVec.z >= 0)
        {
            keplerian.w = Mathd.Acos(Vector3d.Dot(n, eVec) / n.magnitude * keplerian.e);
        }
        else if (eVec.z < 0)
        {
            keplerian.w = (2 * Mathd.PI) - Mathd.Acos(Vector3d.Dot(n, eVec) / n.magnitude * keplerian.e);
        }

        if (n.x >= 0)
        {
            keplerian.LAN = Mathd.Acos(n.y / n.magnitude);
        }
        else if (n.x < 0)
        {
            keplerian.LAN = (2 * Mathd.PI) - Mathd.Acos(n.y / n.magnitude);
        }

        if (h >= 0)
        {
            keplerian.v = Mathd.Acos(Vector3d.Dot(eVec, position) / (keplerian.e * position.magnitude));
        }
        else if (h < 0)
        {
            keplerian.v = (2 * Mathd.PI) - Mathd.Acos(Vector3d.Dot(eVec, position) / (keplerian.e * position.magnitude));
        }

        keplerian.E = Mathd.Atan2(Mathd.Tan(keplerian.v / 2), Mathd.Sqrt((1 + keplerian.e) / (1 - keplerian.e)));

        keplerian.M = keplerian.E - keplerian.e * Mathd.Sin(keplerian.E);

        keplerian.E *= (180 / Mathd.PI);
        keplerian.M *= (180 / Mathd.PI);
        keplerian.v *= (180 / Mathd.PI);
        keplerian.LAN *= (180 / Mathd.PI);
        keplerian.w *= (180 / Mathd.PI);

        keplerian.SMA = 1 / ((2 / position.magnitude) - (Mathd.Pow(velocity.magnitude, 2) / mu));

        keplerian.T = Mathd.Sqrt((4 * (Mathd.PI * Mathd.PI)) / mu * (keplerian.SMA * keplerian.SMA * keplerian.SMA));
    }
}
