using System.Collections.Generic;
using UnityEngine;

public class FloatingOrigin : MonoBehaviour
{
    public static FloatingOrigin Instance;

    public int originThreshold;
    [SerializeField]
    private Transform localCamera;
    public Vector3d currentPosition;
    public Vector3d originPosition;
    [SerializeField]
    private Transform scaledCamera;
    public Vector3d currentPositionScaled;
    public Vector3d originPositionScaled;
    [Header("Transforms")]
    public List<Transform> localTransforms;
    public List<Transform> scaledTransforms;

    private void Awake()
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
}
