using UnityEngine;

public class OrbitalBody : MonoBehaviour
{
    public double mass;
    public Vector3d velocity;
    public Vector3d position;
    public KeplerianElements keplerianElements;
    public bool drawTrajectory;
    public GameObject scaledObject;

    [ContextMenu("Generate scaled")]
    private void GenerateScaled()
    {
        var parent = GameObject.Find("Scaled");
        GameObject.DestroyImmediate(GameObject.Find(gameObject.name + "_Scaled"));

        scaledObject = new GameObject(gameObject.name + "_Scaled");
        scaledObject.transform.parent = parent.transform;
        scaledObject.layer = 6;
    }

    private void Update()
    {
        keplerianElements.ToKeplerian(position, velocity);
    }
}
