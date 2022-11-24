using System;
using UnityEngine;

public enum IntegratorType
{
    Euler,
    SemiImplicitEuler,
    RungeKutta4,
}

[ExecuteAlways]
public class Propagator : MonoBehaviour
{
    public static Propagator Instance;

    public IntegratorType integratorType;
    public double time;
    public int timeScale = 1;
    public float fixedDeltaTime = 0.001f;
    public float plotLength;
    public float stepSize;
    public int steps;
    public Body referenceFrame;
    public float width;

    public Body[] bodies;
    public BodyData[] bodyData;
    public BodyData[] virtualBodyData;
    
    public FloatingOrigin floatingOrigin;

    private TimeSpan timeSpan;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.fixedDeltaTime = fixedDeltaTime;

        bodies = FindObjectsOfType<Body>();

        bodyData = new BodyData[bodies.Length];

        for (int i = 0; i < bodyData.Length; i++)
        {
            bodyData[i] = new BodyData(i, bodies[i].mass, bodies[i].velocity, bodies[i].position);
        }
    }

    private void Update()
    {
        UpdateOrbit();
    }

    private void FixedUpdate()
    {
        timeSpan = TimeSpan.FromSeconds(time);

        for (int i = 0; i < timeScale; i++)
        {
            time += Time.fixedDeltaTime;

            bodyData = Propagate(bodyData, Time.fixedDeltaTime, integratorType);

            for (int j = 0; j < bodies.Length; j++)
            {
                //bodies[j].transform.position = (Vector3)bodyData[j].position;

                bodies[j].velocity = bodyData[j].velocity;
                bodies[j].position = bodyData[j].position;
            }
        }
    }

    private void UpdateOrbit()
    {
        bodies = FindObjectsOfType<Body>();

        virtualBodyData = new BodyData[bodies.Length];
        Vector3[][] drawPoints = new Vector3[bodies.Length][];

        int referenceFrameIndex = 0;
        Vector3d referenceBodyInitialPosition = Vector3d.zero;

        for (int i = 0; i < bodies.Length; i++)
        {
            virtualBodyData[i] = new BodyData(i, bodies[i].mass, bodies[i].velocity, bodies[i].position);
            drawPoints[i] = new Vector3[steps];

            if (referenceFrame != null && bodies[i] == referenceFrame)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualBodyData[i].position;
            }
        }

        for (int step = 0; step < steps; step++)
        {
            Vector3d referenceBodyPosition = (referenceFrame != null) ? virtualBodyData[referenceFrameIndex].position : Vector3d.zero;

            for (int i = 0; i < virtualBodyData.Length; i++)
            {
                virtualBodyData = Propagate(virtualBodyData, stepSize, integratorType);

                // if ((step * stepSize) * 2 >= maneuver.startTime)
                // {
                //     if (maneuver.duration >= 0)
                //     {
                //         virtualBodyData[1].AddConstantAcceleration(maneuver, stepSize);
                //         maneuver.duration -= step * stepSize;
                //     }
                //     else if (step * stepSize > (maneuver.startTime + maneuver.duration))
                //     {
                //     }
                // }
                plotLength = (step * stepSize) * 2;

                Vector3d nextPosition = virtualBodyData[i].position;
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

        for (int bodyIndex = 0; bodyIndex < virtualBodyData.Length; bodyIndex++)
        {
            var pathColour = Color.white;

            var lineRenderer = bodies[bodyIndex].scaledTransform.GetComponent<LineRenderer>();
            lineRenderer.positionCount = drawPoints[bodyIndex].Length;
            lineRenderer.SetPositions(drawPoints[bodyIndex]);
            lineRenderer.startColor = pathColour;
            lineRenderer.endColor = pathColour;
            lineRenderer.widthMultiplier = width;
        }
    }

    private BodyData[] Propagate(BodyData[] bodyData, float deltaTime, IntegratorType integratorType)
    {
        for (int i = 0; i < bodyData.Length; i++)
        {
            BodyData newBodyData = bodyData[i];
            newBodyData.Integrator(bodyData, deltaTime, integratorType);
            bodyData[i] = newBodyData;
        }

        return bodyData;
    }
}
