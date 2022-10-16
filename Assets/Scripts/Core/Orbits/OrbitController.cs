using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    private OrbitalBody[] orbitalBodies;
    private NativeArray<OrbitData> orbitData;

    private void Start()
    {
        orbitalBodies = new OrbitalBody[0];
        orbitalBodies = FindObjectsOfType<OrbitalBody>();

        orbitData = new NativeArray<OrbitData>(orbitalBodies.Length, Allocator.Persistent);

        for (int i = 0; i < orbitalBodies.Length; i++)
        {
            orbitData[i] = new OrbitData(i, orbitalBodies[i].mass, orbitalBodies[i].velocity, (Vector3d)orbitalBodies[i].transform.position);
        }
    }

    private void FixedUpdate()
    {
        UpdateSystem(Time.fixedDeltaTime);

        for (int i = 0; i < orbitData.Length; i++)
        {
            DrawBody(i);
        }
    }

    private void DrawBody(int i)
    {
        orbitalBodies[i].transform.position = (Vector3)(orbitData[i].position - FloatingOrigin.Instance.originPosition);
        orbitalBodies[i].scaledObject.transform.position = (Vector3)((orbitData[i].position / Constants.Scale) - FloatingOrigin.Instance.originPositionScaled);

        orbitalBodies[i].velocity = orbitData[i].velocity;
        orbitalBodies[i].position = orbitData[i].position;
    }

    private void UpdateSystem(float time)
    {
        NativeArray<JobHandle> jobHandles = new NativeArray<JobHandle>(TimeController.Instance.timeScale, Allocator.TempJob);

        var job = new UpdateSystemBurstJob()
        {
            time = Time.deltaTime,
            orbitData = orbitData,
        };
        for (int i = 0; i < TimeController.Instance.timeScale; i++)
        {
            if (i == 0)
            {
                jobHandles[i] = job.Schedule(new JobHandle());
            }
            else
            {
                jobHandles[i] = job.Schedule(jobHandles[i - 1]);
            }
        }
        JobHandle.CompleteAll(jobHandles);
        jobHandles.Dispose();
    }
}

[BurstCompile(CompileSynchronously = false)]
public struct UpdateSystemBurstJob : IJob
{
    [NativeDisableParallelForRestriction]
    public NativeArray<OrbitData> orbitData;
    public float time;

    public void Execute()
    {
        for (int i = 0; i < orbitData.Length; i++)
        {
            var orbitalBody = orbitData[i];
            orbitalBody.CalculateForces(orbitData);
            orbitData[i] = orbitalBody;
        }

        for (int i = 0; i < orbitData.Length; i++)
        {
            var orbitalBody = orbitData[i];
            orbitalBody.ApplyForces(time);
            orbitData[i] = orbitalBody;
        }
    }
}
