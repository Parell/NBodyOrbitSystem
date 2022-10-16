using Unity.Burst;
using Unity.Jobs;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitTrajectory : MonoBehaviour
{
    public int numSteps;
    public float timeStep;
    public OrbitalBody referenceFrame;

    private void FixedUpdate()
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

                drawPoints[i][step] = newPos.ToVector3();
            }
        }

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++)
        {
            if (virtualBodies[bodyIndex].drawTrajectory)
            {
                var pathColour = Color.white;

                for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
                {
                    Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColour);
                }
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
// public struct UpdateAcceleration : IJob
// {
//     public void Execute()
//     {

//     }
// }