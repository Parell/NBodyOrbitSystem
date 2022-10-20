using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform focus = default;

    [SerializeField, Range(1f, 200)]
    float distance = 5f;

    [SerializeField, Min(0f)]
    float focusRadius = 5f;

    [SerializeField, Range(0f, 1f)]
    float focusCentering = 0.5f;

    [SerializeField, Range(1f, 360f)]
    float rotationSpeed = 90f;

    [SerializeField, Range(-89f, 89f)]
    float minVerticalAngle = -45f, maxVerticalAngle = 45f;

    [SerializeField, Min(0f)]
    float alignDelay = 5f;

    [SerializeField, Range(0f, 90f)]
    float alignSmoothRange = 45f;

    [SerializeField]
    LayerMask obstructionMask = -1;

    Camera regularCamera;

    Vector3 focusPoint, previousFocusPoint;

    Vector2 orbitAngles = new Vector2(45f, 0f);

    float lastManualRotationTime;

    Vector3 CameraHalfExtends
    {
        get
        {
            Vector3 halfExtends;
            halfExtends.y =
                regularCamera.nearClipPlane *
                Mathf.Tan(0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);
            halfExtends.x = halfExtends.y * regularCamera.aspect;
            halfExtends.z = 0f;
            return halfExtends;
        }
    }

    void OnValidate()
    {
        if (maxVerticalAngle < minVerticalAngle)
        {
            maxVerticalAngle = minVerticalAngle;
        }
    }

    void Awake()
    {
        regularCamera = GetComponent<Camera>();
        focusPoint = focus.position;
        transform.localRotation = Quaternion.Euler(orbitAngles);
    }

    void LateUpdate()
    {
        UpdateFocusPoint();
        Quaternion lookRotation;
        if (ManualRotation() || AutomaticRotation())
        {
            ConstrainAngles();
            lookRotation = Quaternion.Euler(orbitAngles);
        }
        else
        {
            lookRotation = transform.localRotation;
        }

        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance;

        Vector3 rectOffset = lookDirection * regularCamera.nearClipPlane;
        Vector3 rectPosition = lookPosition + rectOffset;
        Vector3 castFrom = focus.position;
        Vector3 castLine = rectPosition - castFrom;
        float castDistance = castLine.magnitude;
        Vector3 castDirection = castLine / castDistance;

        if (Physics.BoxCast(
            castFrom, CameraHalfExtends, castDirection, out RaycastHit hit,
            lookRotation, castDistance, obstructionMask
        ))
        {
            rectPosition = castFrom + castDirection * hit.distance;
            lookPosition = rectPosition - rectOffset;
        }

        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    void UpdateFocusPoint()
    {
        previousFocusPoint = focusPoint;
        Vector3 targetPoint = focus.position;
        if (focusRadius > 0f)
        {
            float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;
            if (distance > 0.01f && focusCentering > 0f)
            {
                t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
            }
            if (distance > focusRadius)
            {
                t = Mathf.Min(t, focusRadius / distance);
            }
            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
        }
        else
        {
            focusPoint = targetPoint;
        }
    }

    bool ManualRotation()
    {
        Vector2 input = new Vector2(
            -Input.GetAxis("Mouse Y"),
            Input.GetAxis("Mouse X")
        );
        const float e = 0.001f;
        if (input.x < -e || input.x > e || input.y < -e || input.y > e)
        {
            orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
            lastManualRotationTime = Time.unscaledTime;
            return true;
        }
        return false;
    }

    bool AutomaticRotation()
    {
        if (Time.unscaledTime - lastManualRotationTime < alignDelay)
        {
            return false;
        }

        Vector2 movement = new Vector2(
            focusPoint.x - previousFocusPoint.x,
            focusPoint.z - previousFocusPoint.z
        );
        float movementDeltaSqr = movement.sqrMagnitude;
        if (movementDeltaSqr < 0.0001f)
        {
            return false;
        }

        float headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
        float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(orbitAngles.y, headingAngle));
        float rotationChange =
            rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
        if (deltaAbs < alignSmoothRange)
        {
            rotationChange *= deltaAbs / alignSmoothRange;
        }
        else if (180f - deltaAbs < alignSmoothRange)
        {
            rotationChange *= (180f - deltaAbs) / alignSmoothRange;
        }
        orbitAngles.y =
            Mathf.MoveTowardsAngle(orbitAngles.y, headingAngle, rotationChange);
        return true;
    }

    void ConstrainAngles()
    {
        orbitAngles.x =
            Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

        if (orbitAngles.y < 0f)
        {
            orbitAngles.y += 360f;
        }
        else if (orbitAngles.y >= 360f)
        {
            orbitAngles.y -= 360f;
        }
    }

    static float GetAngle(Vector2 direction)
    {
        float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        return direction.x < 0f ? 360f - angle : angle;
    }
    // [SerializeField] private float speed;
    // [SerializeField] private float minSpeed = 0.1f;
    // [SerializeField] private float maxSpeed = 10000000;
    // [SerializeField] private float scrollPower = 10;
    // [Space]
    // [SerializeField]
    // private float _yawLimit = 45f;

    // [SerializeField]
    // private float _pitchLimit = 45;

    // public float YawLimit { get { return _yawLimit; } }
    // public float PitchLimit { get { return _pitchLimit; } }

    // private void OnGUI()
    // {
    //     GUI.Label(new Rect(10, 10, 200, 40), speed.ToString());
    // }

    // private void Update()
    // {
    //     FreeCamera();
    // }

    // private void FreeCamera()
    // {
    //     if (Input.GetMouseButton(1))
    //     {
    //         transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
    //     }

    //     if (Input.GetAxis("Mouse ScrollWheel") > 0)
    //     {
    //         speed += speed * Time.deltaTime * scrollPower;
    //     }
    //     else if (Input.GetAxis("Mouse ScrollWheel") < 0)
    //     {
    //         speed -= speed * Time.deltaTime * scrollPower;
    //     }

    //     speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

    //     if (Input.GetKey(KeyCode.W))
    //     {
    //         transform.position += transform.forward * Time.deltaTime * speed;
    //     }
    //     if (Input.GetKey(KeyCode.S))
    //     {
    //         transform.position -= transform.forward * Time.deltaTime * speed;
    //     }
    //     if (Input.GetKey(KeyCode.A))
    //     {
    //         transform.position -= transform.right * Time.deltaTime * speed;
    //     }
    //     if (Input.GetKey(KeyCode.D))
    //     {
    //         transform.position += transform.right * Time.deltaTime * speed;
    //     }
    //     if (Input.GetKey(KeyCode.E))
    //     {
    //         transform.position += transform.up * Time.deltaTime * speed;
    //     }
    //     if (Input.GetKey(KeyCode.Q))
    //     {
    //         transform.position -= transform.up * Time.deltaTime * speed;
    //     }
    // }
}
