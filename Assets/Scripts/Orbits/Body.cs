using UnityEngine;

// [ExecuteAlways]
public class Body : MonoBehaviour
{
    public double mass;
    public Vector3d velocity;
    public Vector3d position;
    [Space]
    public bool calculateKeplerian;
    public Keplerian keplerian;
    [HideInInspector]
    public Transform scaledTransform;

    private void Start()
    {
        // GenerateScaled();
    }

    [ContextMenu("Generate Scaled")]
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

        scaledObject.AddComponent<LineRenderer>();
    }

    private void Update()
    {
        GetKeplerian(keplerian.centralBody);

        if (Application.isPlaying)
        {
            transform.position = (Vector3)(position - FloatingOrigin.Instance.originPosition);
            scaledTransform.position = (Vector3)(position / Constant.Scale - FloatingOrigin.Instance.originPositionScaled);
        }
        else /* if (scaledTransform != null) */
        {
            transform.position = scaledTransform.position * Constant.Scale;
            position = (Vector3d)transform.position;
        }
    }

    public void GetKeplerian(Body centralBody)
    {
        if (centralBody == null) { return; }
        if (calculateKeplerian == false) { return; }

        double mu = centralBody.mass * Constant.G;

        Vector3d vVec = centralBody.velocity - velocity;
        Vector3d rVec = centralBody.position - position;

        Vector3d h = Vector3d.Cross(vVec, rVec);

        Vector3d eVec = (Vector3d.Cross(vVec, h) / mu) - (position / position.magnitude);

        keplerian.e = eVec.magnitude;

        Vector3d n = new Vector3d(-h.x, h.y, 0);

        if (Vector3d.Dot(rVec, vVec) >= 0)
        {
            keplerian.trueAnomaly = Mathd.Acos(Vector3d.Dot(eVec, rVec) / (eVec.magnitude * rVec.magnitude));
        }
        else
        {
            keplerian.trueAnomaly = (2 * Mathd.PI) - Mathd.Acos(Vector3d.Dot(eVec, rVec) / (eVec.magnitude * rVec.magnitude));
        }

        keplerian.i = Mathd.Acos(h.y / h.magnitude) * (180 / Mathd.PI);

        keplerian.eAnomaly = 2 * Mathd.Acos(Mathd.Tan(keplerian.trueAnomaly / 2) / Mathd.Sqrt((1 + eVec.magnitude) / (1 - eVec.magnitude)));

        if (n.x >= 0)
        {
            keplerian.LAN = Mathd.Acos(n.y / n.magnitude);
        }
        else
        {
            keplerian.LAN = (2 * Mathd.PI) - Mathd.Acos(n.y / n.magnitude);
        }

        if (eVec.z >= 0)
        {
            keplerian.w = Mathd.Acos(Vector3d.Dot(n, eVec) / n.magnitude * eVec.magnitude);
        }
        else
        {
            keplerian.w = (2 * Mathd.PI) - Mathd.Acos(Vector3d.Dot(n, eVec) / n.magnitude * eVec.magnitude);
        }

        keplerian.meanAnomaly = keplerian.eAnomaly - (eVec.magnitude * Mathd.Sin(keplerian.eAnomaly));

        keplerian.a = 1 / ((2 / rVec.magnitude) - (Mathd.Pow(vVec.magnitude, 2) / mu));

        keplerian.eAnomaly *= (180 / Mathd.PI);
        keplerian.meanAnomaly *= (180 / Mathd.PI);
        keplerian.trueAnomaly *= (180 / Mathd.PI);
        keplerian.LAN *= (180 / Mathd.PI);
        keplerian.w *= (180 / Mathd.PI);

        keplerian.T = Mathd.Sqrt((4 * (Mathd.PI * Mathd.PI)) / mu * (keplerian.a * keplerian.a * keplerian.a));
    }

    // public void GetDistance()
    // {

    // }

    // public void SetCartesianState()
    // {

    // }

    // public void SetKeplerianState()
    // {

    // }

    // public void GetVelocityAtTime()
    // {

    // }

    // public void GetPositionAtTime()
    // {

    // }
}
