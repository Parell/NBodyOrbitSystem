using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaledCamera : MonoBehaviour
{
    [SerializeField]
    private Camera localCamera;
    [SerializeField]
    private float unscaledFarClipPlane = 1e14f;
    [SerializeField]
    private float nearClipOffset = 1f;
    private Camera thisCamera;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        transform.position = (FloatingOrigin.Instance.currentPosition.ToVector3() / Constant.Scale) - FloatingOrigin.Instance.originPositionScaled.ToVector3();
        transform.rotation = localCamera.transform.rotation;

        thisCamera.nearClipPlane = localCamera.farClipPlane * nearClipOffset / Constant.Scale;
        thisCamera.farClipPlane = unscaledFarClipPlane / Constant.Scale;
    }
}
