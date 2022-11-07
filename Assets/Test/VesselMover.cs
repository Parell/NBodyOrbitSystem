using System.Collections;
using UnityEngine;

public class VesselMover : MonoBehaviour
{
    public Hashtable torques;
    public Hashtable forces;
    VesselInputController vesselInputs;
    public new Rigidbody rigidbody;

    void Start()
    {
        vesselInputs = GetComponent<VesselInputController>();
        rigidbody = GetComponent<Rigidbody>();

        torques = new Hashtable();
        forces = new Hashtable();
    }

    void Update()
    {
        if (!vesselInputs.controllable) return;

        HandleRoll();
        HandlePitch();
        HandleYaw();
        HandleMainThruster();
    }

    void FixedUpdate()
    {
        HandleTorques();
        HandleForces();
    }

    void HandleRoll()
    {
        if (vesselInputs.RollRight())
        {
            // for (int i = 0; i < rollThrusters.right.Length; i++)
            // {
            //     rollThrusters.right[i].StartThruster();
            // }
            // for (int i = 0; i < rollThrusters.left.Length; i++)
            // {
            //     rollThrusters.left[i].StopThruster();
            // }
        }
        else if (vesselInputs.RollLeft())
        {
            // for (int i = 0; i < rollThrusters.right.Length; i++)
            // {
            //     rollThrusters.right[i].StopThruster();
            // }
            // for (int i = 0; i < rollThrusters.left.Length; i++)
            // {
            //     rollThrusters.left[i].StartThruster();
            // }
        }
        else
        {
            // for (int i = 0; i < rollThrusters.right.Length; i++)
            // {
            //     rollThrusters.right[i].StopThruster();
            // }
            // for (int i = 0; i < rollThrusters.left.Length; i++)
            // {
            //     rollThrusters.left[i].StopThruster();
            // }
        }
    }

    void HandlePitch()
    {
        if (vesselInputs.PitchUp())
        {
            // for (int i = 0; i < pitchThrusters.up.Length; i++)
            // {
            //     pitchThrusters.up[i].StartThruster();
            // }
            // for (int i = 0; i < pitchThrusters.down.Length; i++)
            // {
            //     pitchThrusters.down[i].StopThruster();
            // }
        }
        else if (vesselInputs.PitchDown())
        {
            // for (int i = 0; i < pitchThrusters.up.Length; i++)
            // {
            //     pitchThrusters.up[i].StopThruster();
            // }
            // for (int i = 0; i < pitchThrusters.down.Length; i++)
            // {
            //     pitchThrusters.down[i].StartThruster();
            // }
        }
        else
        {
            // for (int i = 0; i < pitchThrusters.up.Length; i++)
            // {
            //     pitchThrusters.up[i].StopThruster();
            // }
            // for (int i = 0; i < pitchThrusters.down.Length; i++)
            // {
            //     pitchThrusters.down[i].StopThruster();
            // }
        }
    }

    void HandleYaw()
    {
        if (vesselInputs.YawRight())
        {
            // for (int i = 0; i < yawThrusters.right.Length; i++)
            // {
            //     yawThrusters.right[i].StartThruster();
            // }
            // for (int i = 0; i < yawThrusters.left.Length; i++)
            // {
            //     yawThrusters.left[i].StopThruster();
            // }
        }
        else if (vesselInputs.YawLeft())
        {
            // for (int i = 0; i < yawThrusters.right.Length; i++)
            // {
            //     yawThrusters.right[i].StopThruster();
            // }
            // for (int i = 0; i < yawThrusters.left.Length; i++)
            // {
            //     yawThrusters.left[i].StartThruster();
            // }
        }
        else
        {
            // for (int i = 0; i < yawThrusters.right.Length; i++)
            // {
            //     yawThrusters.right[i].StopThruster();
            // }
            // for (int i = 0; i < yawThrusters.left.Length; i++)
            // {
            //     yawThrusters.left[i].StopThruster();
            // }
        }
    }

    void HandleMainThruster()
    {
        // if (MainThruster())
        // {
        //     for (int i = 0; i < mainThrusters.main.Length; i++)
        //     {
        //         mainThrusters.main[i].StartThruster();
        //     }
        // }
        // else
        // {
        //     for (int i = 0; i < mainThrusters.main.Length; i++)
        //     {
        //         mainThrusters.main[i].StopThruster();
        //     }
        // }
    }

    public void AddTorque(string identifier, Vector3 torque)
    {
        if (torques.ContainsKey(identifier)) return;
        torques.Add(identifier, torque);
    }

    public void RemoveTorque(string identifier)
    {
        if (!torques.ContainsKey(identifier)) return;
        torques.Remove(identifier);
    }

    public void AddForce(string identifier, Vector3 force)
    {
        if (forces.ContainsKey(identifier)) return;
        forces.Add(identifier, force);
    }

    public void RemoveForce(string identifier)
    {
        if (!forces.ContainsKey(identifier)) return;
        forces.Remove(identifier);
    }

    void HandleTorques()
    {
        foreach (DictionaryEntry entry in torques)
        {
            Vector3 torque = (Vector3)entry.Value;
            rigidbody.AddRelativeTorque(torque.normalized);
        }
    }

    void HandleForces()
    {
        foreach (DictionaryEntry entry in forces)
        {
            Vector3 force = (Vector3)entry.Value;
            rigidbody.AddRelativeForce(force);
        }
    }
}
