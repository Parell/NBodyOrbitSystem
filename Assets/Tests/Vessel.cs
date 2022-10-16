using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Celestial,
    Asteroid,
    Station,
    Vessel,
}

public class Vessel : MonoBehaviour
{
    public ObjectType objectType;
    // only stats that matter
    public float deltaV;
    public float acceleration;
}
