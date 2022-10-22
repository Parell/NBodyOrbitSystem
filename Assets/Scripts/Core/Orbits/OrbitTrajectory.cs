using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitTrajectory : MonoBehaviour
{
    public static OrbitTrajectory Instance;

    public int numSteps;
    public float timeStep;
    public float width = 10f;
    public OrbitalBody referenceFrame;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        DrawOrbits();
    }

    private void DrawOrbits()
    {
        OrbitalBody[] orbitalBodies = FindObjectsOfType<OrbitalBody>();
        var virtualBodies = new VirtualBody[orbitalBodies.Length];

        var drawPoints = new Vector3[orbitalBodies.Length][];
        int referenceFrameIndex = 0;
        Vector3d referenceBodyInitialPosition = Vector3d.zero;

        // Initialize virtual bodies (don't want to move the actual bodies)
        for (int i = 0; i < virtualBodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(orbitalBodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            if (referenceFrame != null && orbitalBodies[i] == referenceFrame)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualBodies[i].position;
            }
        }

        // Simulate
        for (int step = 0; step < numSteps; step++)
        {
            Vector3d referenceBodyPosition = (referenceFrame != null) ? virtualBodies[referenceFrameIndex].position : Vector3d.zero;

            for (int i = 0; i < virtualBodies.Length; i++)
            {
                virtualBodies[i].velocity += CalculateAcceleration(i, virtualBodies) * timeStep;

                Vector3d newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
                virtualBodies[i].position = newPos;
                if (referenceFrame != null)
                {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    newPos -= referenceFrameOffset;
                }
                if (referenceFrame != null && i == referenceFrameIndex)
                {
                    newPos = referenceBodyInitialPosition;
                }

                drawPoints[i][step] = (Vector3)(newPos - FloatingOrigin.Instance.originPositionScaled);
            }
        }

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++)
        {
            if (virtualBodies[bodyIndex].drawTrajectory)
            {
                var pathColour = Color.white;

                var lineRenderer = orbitalBodies[bodyIndex].scaledObject.GetComponent<LineRenderer>();
                lineRenderer.positionCount = drawPoints[bodyIndex].Length;
                lineRenderer.SetPositions(drawPoints[bodyIndex]);
                lineRenderer.startColor = pathColour;
                lineRenderer.endColor = pathColour;
                lineRenderer.widthMultiplier = width;

                // for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
                // {
                //     Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColour);
                // }
            }
        }
    }

    public Vector3d CalculateAcceleration(int i, VirtualBody[] virtualBodies)
    {
        Vector3d acceleration = Vector3d.zero;
        for (int j = 0; j < virtualBodies.Length; j++)
        {
            if (i == j)
            {
                continue;
            }
            Vector3d forceDir = (virtualBodies[j].position - virtualBodies[i].position).normalized;
            double sqrDst = (virtualBodies[j].position - virtualBodies[i].position).sqrMagnitude;
            acceleration += forceDir * Constants.G * virtualBodies[j].mass / sqrDst;
        }
        return acceleration;
    }
}

public class VirtualBody
{
    public Vector3d position;
    public Vector3d velocity;
    public double mass;
    public bool drawTrajectory;

    public VirtualBody(OrbitalBody body)
    {
        position = (Vector3d)body.transform.position;
        velocity = body.velocity;
        mass = body.mass;
        drawTrajectory = body.drawTrajectory;
    }
}

// [BurstCompile(CompileSynchronously = false)]
// public struct UpdateAccelerationJob : IJobParallelFor
// {
//     public NativeArray<Vector3d> position;
//     public NativeArray<Vector3d> velocity;
//     public NativeArray<double> mass;
//     public float timeStep;
//     public int bodyLength;
//     public Vector3d newPos;
//     public int bodyIndex;
//     public int step;

//     public void Execute(int i)
//     {
//         //Vector3d referenceBodyPosition = (referenceFrame != null) ? position[referenceFrameIndex] : Vector3d.zero;

//         for (int k = 0; k < bodyLength; k++)
//         {
//             Vector3d acceleration = Vector3d.zero;

//             for (int j = 0; j < bodyLength; j++)
//             {
//                 if (k == j)
//                 {
//                     continue;
//                 }
//                 Vector3d forceDir = (position[j] - position[k]).normalized;
//                 double sqrDst = (position[j] - position[k]).sqrMagnitude;
//                 acceleration += forceDir * Constants.G * mass[j] / sqrDst;
//             }

//             velocity[k] += acceleration * timeStep;

//             newPos = position[k] + velocity[k] * timeStep;
//             position[k] = newPos;

//             bodyIndex = k;
//             step = i;

//             /*             if (referenceFrame != null)
//                         {
//                             Vector3d referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
//                             newPos -= referenceFrameOffset;
//                         }
//                         if (referenceFrame != null && k == referenceFrameIndex)
//                         {
//                             newPos = referenceBodyInitialPosition;
//                         } */

//         }
//     }
// }
