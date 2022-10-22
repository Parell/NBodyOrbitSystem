using Buttons;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitalBody : MonoBehaviour
{
    public double mass;
    public Vector3d velocity;
    //public KeplerianElements keplerian;
    public bool drawTrajectory = true;
    [HideInInspector]
    public GameObject scaledObject;

#if (UNITY_EDITOR)
    private void Update()
    {
        if (EditorApplication.isPlaying) return;
        transform.position = scaledObject.transform.position * Constants.Scale;
    }

    [Button(Mode = ButtonMode.DisabledInPlayMode, Spacing = ButtonSpacing.Before)]
    public void GenerateScaled()
    {
        if (EditorApplication.isPlaying) return;

        var parent = GameObject.Find("Scaled");
        GameObject.DestroyImmediate(GameObject.Find(gameObject.name + "_Scaled"));

        scaledObject = new GameObject(gameObject.name + "_Scaled");
        scaledObject.transform.parent = parent.transform;
        scaledObject.transform.position = transform.position / Constants.Scale;
        scaledObject.layer = 6;
        scaledObject.transform.localScale = Vector3.one / Constants.Scale;
    }
#endif

    /*     private void Update()
        {
            //keplerian.ToKeplerian(position, velocity);
        } */
}
