using UnityEngine;

[ExecuteInEditMode]
public class OrbitalBody : MonoBehaviour
{
    public Cartesian cartesian;
    // public Keplerian keplerian;
    // [HideInInspector]
    // public Transform scaledTransform;

    private void Update()
    {
        transform.position = (Vector3)cartesian.position;
        // scaledTransform.position = (Vector3)cartesian.position / Constant.Scale;
    }

    // [Button(Mode = ButtonMode.DisabledInPlayMode, Spacing = ButtonSpacing.Before)]
    // public void GenerateScaled()
    // {
    //     GameObject.DestroyImmediate(GameObject.Find(gameObject.name + GetInstanceID()));

    //     GameObject scaledObject = new GameObject(gameObject.name + GetInstanceID());
    //     scaledObject.transform.parent = GameObject.Find("Scaled").transform;
    //     scaledObject.transform.position = transform.position / Constant.Scale;
    //     scaledObject.transform.localScale = Vector3.one / Constant.Scale;
    //     scaledObject.transform.rotation = transform.rotation;
    //     scaledObject.layer = 6;
    //     scaledTransform = scaledObject.transform;
    // }
}
