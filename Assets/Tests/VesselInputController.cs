using UnityEngine;

public class VesselInputController : MonoBehaviour
{
    public bool controllable;
    public Vector3 angularInputs;
    public Vector3 linearInputs;
    public float sensitivity = 1;

    private void Update()
    {
        if (!controllable) return;
    }

    #region Inputs
    public bool RollLeft()
    {
        if (Input.GetKey(KeyCode.Q) || angularInputs.z < -sensitivity) return true; return false;
    }

    public bool RollRight()
    {
        if (Input.GetKey(KeyCode.E) || angularInputs.z > sensitivity) return true; return false;
    }

    public bool PitchUp()
    {
        if (Input.GetKey(KeyCode.S) || angularInputs.x > sensitivity) return true; return false;
    }

    public bool PitchDown()
    {
        if (Input.GetKey(KeyCode.W) || angularInputs.x < -sensitivity) return true; return false;
    }

    public bool YawLeft()
    {
        if (Input.GetKey(KeyCode.A) || angularInputs.y < -sensitivity) return true; return false;
    }

    public bool YawRight()
    {
        if (Input.GetKey(KeyCode.D) || angularInputs.y > sensitivity) return true; return false;
    }

    public bool MainThruster()
    {
        if (Input.GetKey(KeyCode.Space)) return true; return false;
    }
    #endregion
}
