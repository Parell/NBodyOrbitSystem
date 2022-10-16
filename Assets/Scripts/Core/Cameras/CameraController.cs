using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minSpeed = 0.1f;
    [SerializeField] private float maxSpeed = 10000000;
    [SerializeField] private float scrollPower = 10;

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 40), speed.ToString());
    }

    private void Update()
    {
        FreeCamera();
    }

    private void FreeCamera()
    {
        if (Input.GetMouseButton(1))
        {
            transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            speed += speed * Time.deltaTime * scrollPower;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            speed -= speed * Time.deltaTime * scrollPower;
        }

        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += transform.up * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= transform.up * Time.deltaTime * speed;
        }
    }
}
