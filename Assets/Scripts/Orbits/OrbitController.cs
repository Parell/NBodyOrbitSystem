using UnityEngine;

public enum IntegratorType
{
    Euler,
    SemiImplicitEuler,
    RungeKutta4, // Needs error system, only for low time steps
}

[ExecuteInEditMode]
public class OrbitController : MonoBehaviour
{
    public Maneuver maneuver;
    public double plotLength;
    public double time;
    public bool useKeys;
    public int maxTimeScale;
    public int timeScale;
    public double stepSize = 1;
    public int steps;
    public IntegratorType integratorType;
    public OrbitBody referenceFrame;

    private OrbitBody[] orbitBodies;
    public OrbitData[] orbitData;
    public OrbitData[] virtualOrbitData;

    public FloatingOrigin floatingOrigin;

    private void FindBodies()
    {
        orbitBodies = FindObjectsOfType<OrbitBody>();

        orbitData = new OrbitData[orbitBodies.Length];

        for (int i = 0; i < orbitBodies.Length; i++)
        {
            orbitData[i] = new OrbitData(orbitBodies[i].name, i, orbitBodies[i].mass, orbitBodies[i].velocity, orbitBodies[i].position);
        }
    }

    private void Start()
    {
        FindBodies();
    }

    private void Update()
    {
        DrawOrbit(10);

        if (!Application.isPlaying) { return; }

        if (useKeys)
        {
            if (Input.GetKeyUp(KeyCode.Period))
            {
                timeScale /= 2;

                Mathf.Clamp(timeScale, 0, maxTimeScale);
            }
            else if (Input.GetKeyUp(KeyCode.Comma))
            {
                timeScale *= 2;

                if (timeScale == 0)
                {
                    timeScale = 1;
                }

                Mathf.Clamp(timeScale, 0, maxTimeScale);
            }
            else if (Input.GetKeyUp(KeyCode.Slash))
            {
                timeScale = 0;
            }
        }

        time += Time.deltaTime * timeScale;

        for (int i = 0; i < timeScale; i++)
        {
            orbitData = Propagate(orbitData, Time.deltaTime, integratorType);

            for (int j = 0; j < orbitBodies.Length; j++)
            {
                // orbitBodies[j].transform.position = (Vector3)orbitData[j].position;

                orbitBodies[j].velocity = orbitData[j].velocity;
                orbitBodies[j].position = orbitData[j].position;
            }
        }

    }

    private OrbitData[] Propagate(OrbitData[] orbitData, double deltaTime, IntegratorType integratorType)
    {
        for (int i = 0; i < orbitData.Length; i++)
        {
            OrbitData orbitalBody = orbitData[i];
            orbitalBody.Integrate(deltaTime, integratorType, orbitData);
            orbitData[i] = orbitalBody;
        }
        return orbitData;
    }

    private void DrawOrbit(int width)
    {
        orbitBodies = FindObjectsOfType<OrbitBody>();

        virtualOrbitData = new OrbitData[orbitBodies.Length];
        Vector3[][] drawPoints = new Vector3[orbitBodies.Length][];

        int referenceFrameIndex = 0;
        Vector3d referenceBodyInitialPosition = Vector3d.zero;

        for (int i = 0; i < orbitBodies.Length; i++)
        {
            virtualOrbitData[i] = new OrbitData(orbitBodies[i].name, i, orbitBodies[i].mass, orbitBodies[i].velocity, orbitBodies[i].position);
            drawPoints[i] = new Vector3[steps];

            if (referenceFrame != null && orbitBodies[i] == referenceFrame)
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
                virtualOrbitData = Propagate(virtualOrbitData, stepSize, integratorType);

                if ((step * stepSize) * 2 >= maneuver.startTime)
                {
                    if (maneuver.duration >= 0)
                    {
                        virtualOrbitData[1].AddConstantAcceleration(maneuver, stepSize);
                        maneuver.duration -= step * stepSize;
                    }
                    else if (step * stepSize > (maneuver.startTime + maneuver.duration))
                    {
                    }
                }
                plotLength = (step * stepSize) * 2;


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

                drawPoints[i][step] = (Vector3)(nextPosition) / Constant.Scale - (Vector3)floatingOrigin.originPositionScaled;
            }
        }

        for (int bodyIndex = 0; bodyIndex < virtualOrbitData.Length; bodyIndex++)
        {
            var pathColour = Color.white;

            var lineRenderer = orbitBodies[bodyIndex].scaledTransform.GetComponent<LineRenderer>();
            lineRenderer.positionCount = drawPoints[bodyIndex].Length;
            lineRenderer.SetPositions(drawPoints[bodyIndex]);
            lineRenderer.startColor = pathColour;
            lineRenderer.endColor = pathColour;
            lineRenderer.widthMultiplier = width;
        }
    }
}
