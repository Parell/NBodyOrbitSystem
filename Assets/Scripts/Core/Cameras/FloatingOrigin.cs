using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FloatingOrigin : MonoBehaviour
{
    public static FloatingOrigin Instance;

    public int originThreshold;
    // public bool UpdateParticles = true;
    // public bool UpdateTrailRenderers = true;
    // public bool UpdateLineRenderers = true;
    [Header("Local")]
    [SerializeField]
    private Transform localCamera;
    public Vector3d currentPosition;
    public Vector3d originPosition;
    [Header("Scaled")]
    [SerializeField]
    private Transform scaledCamera;
    public Vector3d currentPositionScaled;
    public Vector3d originPositionScaled;
    [Header("Transforms")]
    public List<Transform> localTransforms;
    public List<Transform> scaledTransforms;

    private ParticleSystem.Particle[] parts = null;
    private VisualEffect[] visualEffect = null;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        currentPosition = (Vector3d)localCamera.position + originPosition;
        currentPositionScaled = (Vector3d)scaledCamera.position + originPositionScaled;

        if (localCamera.position.magnitude > originThreshold)
        {
            MoveOrigin(localCamera.position);

            // if (UpdateParticles)
            //     MoveParticles(localCamera.position);

            // if (UpdateTrailRenderers)
            //     MoveTrailRenderers(localCamera.position);

            // if (UpdateLineRenderers)
            //     MoveLineRenderers(localCamera.position);
        }

        if (scaledCamera.position.magnitude > originThreshold)
        {
            MoveOriginScaled(scaledCamera.position);
        }
    }

    private void MoveOrigin(Vector3 delta)
    {
        foreach (Transform target in localTransforms)
        {
            target.position -= delta;
        }

        originPosition += (Vector3d)delta;
    }

    private void MoveOriginScaled(Vector3 delta)
    {
        foreach (Transform target in scaledTransforms)
        {
            target.position -= delta;
        }

        originPositionScaled += (Vector3d)delta;
    }

    public void RegisterTransform(Transform target)
    {
        localTransforms.Add(target);
    }

    public void RegisterTransformScaled(Transform target)
    {
        scaledTransforms.Add(target);
    }

    // private void MoveTrailRenderers(Vector3 offset)
    // {
    //     var trails = FindObjectsOfType<TrailRenderer>() as TrailRenderer[];
    //     foreach (var trail in trails)
    //     {
    //         Vector3[] positions = new Vector3[trail.positionCount];

    //         int positionCount = trail.GetPositions(positions);
    //         for (int i = 0; i < positionCount; ++i)
    //             positions[i] -= offset;

    //         trail.SetPositions(positions);
    //     }
    // }

    // private void MoveLineRenderers(Vector3 offset)
    // {
    //     var lines = FindObjectsOfType<LineRenderer>() as LineRenderer[];
    //     foreach (var line in lines)
    //     {
    //         Vector3[] positions = new Vector3[line.positionCount];

    //         int positionCount = line.GetPositions(positions);
    //         for (int i = 0; i < positionCount; ++i)
    //             positions[i] -= offset;

    //         line.SetPositions(positions);
    //     }
    // }

    // private void MoveParticles(Vector3 offset)
    // {
    //     var particles = FindObjectsOfType<ParticleSystem>() as ParticleSystem[];
    //     foreach (ParticleSystem system in particles)
    //     {
    //         if (system.main.simulationSpace != ParticleSystemSimulationSpace.World)
    //             continue;

    //         int particlesNeeded = system.main.maxParticles;

    //         if (particlesNeeded <= 0)
    //             continue;

    //         bool wasPaused = system.isPaused;
    //         bool wasPlaying = system.isPlaying;

    //         if (!wasPaused)
    //             system.Pause();

    //         // ensure a sufficiently large array in which to store the particles
    //         if (parts == null || parts.Length < particlesNeeded)
    //         {
    //             parts = new ParticleSystem.Particle[particlesNeeded];
    //         }

    //         // now get the particles
    //         int num = system.GetParticles(parts);

    //         for (int i = 0; i < num; i++)
    //         {
    //             parts[i].position -= offset;
    //         }

    //         system.SetParticles(parts, num);

    //         if (wasPlaying)
    //             system.Play();
    //     }

    //     var particles2 = FindObjectsOfType<VisualEffect>() as VisualEffect[];
    //     foreach (VisualEffect system in particles2)
    //     {


    //         int particlesNeeded = system.aliveParticleCount;

    //         if (particlesNeeded <= 0)
    //             continue;

    //         bool wasPaused = !system.isActiveAndEnabled;
    //         bool wasPlaying = system.isActiveAndEnabled;

    //         if (!wasPaused)
    //             system.Stop();


    //     }
    // }
}
