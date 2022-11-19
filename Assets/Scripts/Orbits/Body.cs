using UnityEngine;

[ExecuteAlways]
public class Body : MonoBehaviour
{
    public double mass;
    public Vector3d velocity;
    public Vector3d position;

    private void Update()
    {
        if (Application.isPlaying)
        {
            //transform.position = (Vector3)(position - FloatingOrigin.Instance.originPosition);
            //scaledTransform.position = (Vector3)(position / Constant.Scale - FloatingOrigin.Instance.originPositionScaled);
        }
        else
        {
            //transform.position = scaledTransform.position * Constant.Scale;
            position = (Vector3d)transform.position;
        }
    }
}
