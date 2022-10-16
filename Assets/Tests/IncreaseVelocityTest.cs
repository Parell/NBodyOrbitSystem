using UnityEngine;

public class IncreaseVelocityTest : MonoBehaviour
{
    private OrbitController orbitController;
    private OrbitalBody orbitalBody;
    public Vector3d force;

    private void Start()
    {
        orbitController = FindObjectOfType<OrbitController>();
        orbitalBody = FindObjectOfType<OrbitalBody>();
    }

    private void FixedUpdate()
    {
        //orbitController.orbitData[orbitalBody.index].velocity += force;

        //force = Vector3d.zero;
    }
}
