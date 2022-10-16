using UnityEngine;

public class Billboard : MonoBehaviour
{
    GameObject mainCamera;
    public bool useStaticBillboard;

    void Start()
    {
        mainCamera = GameObject.Find("LocalCamera");
    }

    void LateUpdate()
    {
        if (!useStaticBillboard)
        {
            transform.LookAt(mainCamera.transform);
        }
        else
        {
            transform.rotation = mainCamera.transform.rotation;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}