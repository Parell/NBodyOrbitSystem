using UnityEngine;

public enum Integrator
{
    None,
    RK4,
    RKDP45
}

[ExecuteInEditMode]
public class OrbitPropagator : MonoBehaviour
{
    public double time;
    [Tooltip("Intermediate integration steps are taken to ensure that the error never exceeds this value")]
    public double tolerance;
    [Tooltip("The number of seconds propagated")]
    public float stepSize;
    [Tooltip("The number of steps propagated")]
    public int steps;
    [SerializeField] private float timeScale;
    public Integrator integrator;
    public OrbitalBody referenceFrame;
    [Space]
    public Maneuver maneuver;
    public OrbitalBody[] orbitalBodies;
    public OrbitData[] orbitData;
    public OrbitData[] virtualOrbitData;

    private void Start()
    {
        orbitalBodies = FindObjectsOfType<OrbitalBody>();

        orbitData = new OrbitData[orbitalBodies.Length];

        for (int i = 0; i < orbitalBodies.Length; i++)
        {
            orbitData[i] = new OrbitData(i, orbitalBodies[i].mass, orbitalBodies[i].velocity, orbitalBodies[i].position);
        }
    }

    private void Update()
    {
        DrawOrbit();

        if (!Application.isPlaying) { return; }

        time += timeScale * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!Application.isPlaying) { return; }

        orbitData = Propagate(orbitData, Time.fixedDeltaTime * timeScale);

        for (int j = 0; j < orbitData.Length; j++)
        {
            orbitalBodies[j].velocity = orbitData[j].velocity;
            orbitalBodies[j].position = orbitData[j].position;
        }
    }

    private OrbitData[] Propagate(OrbitData[] orbitData, float time)
    {
        for (int j = 0; j < orbitData.Length; j++)
        {
            OrbitData orbitalBody = orbitData[j];
            orbitalBody.CalculateGravity(orbitData);
            orbitalBody.IntagrateAcceleration(time, integrator);
            orbitData[j] = orbitalBody;
        }
        return orbitData;
    }

    private void DrawOrbit()
    {
        orbitalBodies = FindObjectsOfType<OrbitalBody>();

        virtualOrbitData = new OrbitData[orbitalBodies.Length];
        Vector3[][] drawPoints = new Vector3[orbitalBodies.Length][];

        int referenceFrameIndex = 0;
        Vector3d referenceBodyInitialPosition = Vector3d.zero;

        for (int i = 0; i < orbitalBodies.Length; i++)
        {
            virtualOrbitData[i] = new OrbitData(i, orbitalBodies[i].mass, orbitalBodies[i].velocity, orbitalBodies[i].position);
            drawPoints[i] = new Vector3[steps];

            if (referenceFrame != null && orbitalBodies[i] == referenceFrame)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualOrbitData[i].position;
            }
        }

        for (int step = 0; step < steps; step++)
        {
            Vector3d referenceBodyPosition = (referenceFrame != null) ? virtualOrbitData[referenceFrameIndex].position : Vector3d.zero;

            for (int i = 0; i < virtualOrbitData.Length; i++)
            {
                virtualOrbitData = Propagate(virtualOrbitData, stepSize);

                if (step * stepSize >= maneuver.startTime)
                {
                    if (maneuver.altitudeLock == "prograde")
                    {
                        maneuver.direction = ((Vector3)virtualOrbitData[i].velocity).normalized;
                    }

                    if (maneuver.duration > 0)
                    {
                        virtualOrbitData[0].AddConstantAcceleration(maneuver, stepSize);
                        maneuver.duration -= step * stepSize;
                    }
                }
                else if (step * stepSize > (maneuver.startTime + maneuver.duration))
                {
                }

                Vector3d nextPosition = virtualOrbitData[i].position;
                if (referenceFrame != null)
                {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    nextPosition -= referenceFrameOffset;
                }
                if (referenceFrame != null && i == referenceFrameIndex)
                {
                    nextPosition = referenceBodyInitialPosition;
                }

                drawPoints[i][step] = (Vector3)(nextPosition) / Constant.Scale;
            }
        }

        for (int bodyIndex = 0; bodyIndex < virtualOrbitData.Length; bodyIndex++)
        {
            var pathColour = Color.white;

            for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
            {
                Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColour);
            }
        }
    }
}
