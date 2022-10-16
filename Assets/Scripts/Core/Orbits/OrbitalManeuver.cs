using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public enum SteeringMode
{
    Fixed,
    Prograde,
    Retrograde
};

[ExecuteInEditMode]
public class OrbitalManeuver : MonoBehaviour
{
    public List<Maneuver> maneuvers;
    public OrbitalBody target;
    public SteeringMode steeringMode;
    [Space]
    public int numSteps = 1000;
    public float timeStep = 0.1f;
    public OrbitalBody centralBody;
    public OrbitalBody thisOrbitalBody;

    private void Update()
    {
        DrawOrbits();
    }

    void DrawOrbits()
    {
            thisOrbitalBody = GetComponent<OrbitalBody>();
            OrbitalBody[] bodies = FindObjectsOfType<OrbitalBody>();
            var virtualBodies = new VirtualBody[bodies.Length];
            var drawPoints = new Vector3[bodies.Length][];
            int referenceFrameIndex = 0;
            Vector3d referenceBodyInitialPosition = Vector3d.zero;

            // Initialize virtual bodies (don't want to move the actual bodies)
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                virtualBodies[i] = new VirtualBody(bodies[i]);
                drawPoints[i] = new Vector3[numSteps];

                if (centralBody != null && bodies[i] == centralBody)
                {
                    referenceFrameIndex = i;
                    referenceBodyInitialPosition = virtualBodies[i].position;
                }
            }


            // Simulate
            for (int step = 0; step < numSteps; step++)
            {
                // Maneuver
                for (int i = 0; i < maneuvers.Count; i++)
                {
                    if (step == (int)(maneuvers[i].startTime / timeStep))
                    {
                        virtualBodies[0].velocity += maneuvers[i].deltaV;
                    }
                }

                Vector3d referenceBodyPosition = (centralBody != null) ? virtualBodies[referenceFrameIndex].position : Vector3d.zero;
                // Update velocities

                for (int i = 0; i < virtualBodies.Length; i++)
                {
                    virtualBodies[i].velocity += CalculateAcceleration(i, virtualBodies) * timeStep;
                }
                // Update positions
                for (int i = 0; i < virtualBodies.Length; i++)
                {
                    Vector3d newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
                    virtualBodies[i].position = newPos;
                    if (centralBody != null)
                    {
                        var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                        newPos -= referenceFrameOffset;
                    }
                    if (centralBody != null && i == referenceFrameIndex)
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
                    var pathColour = Color.red; //

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

[System.Serializable]
public class Maneuver
{
    public Vector3d deltaV;
    public int startTime;
    public float burnTime;
    public bool done;

    public Maneuver(Vector3d deltaV, int startTime, int burnTime)
    {
        this.deltaV = deltaV;
        this.startTime = startTime;
        this.burnTime = burnTime;
    }
}

// public class VirtualBody
// {
//     public Vector3d position;
//     public Vector3d velocity;
//     public double mass;
//     public bool drawTrajectory;

//     public VirtualBody(OrbitalBody body)
//     {
//         position = (Vector3d)body.transform.position;
//         velocity = body.velocity;
//         mass = body.mass;
//         drawTrajectory = body.drawTrajectory;
//     }
// }
