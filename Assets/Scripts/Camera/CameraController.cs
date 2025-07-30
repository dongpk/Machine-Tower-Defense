using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private bool canControll;
    [SerializeField] private Vector3 levelCenterPoint;
    [SerializeField] private float maxDistanceFromCenter;

    [Header("Movement Details")]
    [SerializeField] private float moveSpeed = 120f;
    private float mouseMovementSpeed = 5f;
    [SerializeField] private float edgeMovementSpeed = 50f;
    [SerializeField] private float edgeTreshHold = 10f;
    private float screenWidth;
    private float screenHeight;



    [Header("Rotation Details")]
    [SerializeField] private Transform focusPoint;
    [SerializeField] private float maxFocusDistance = 20f;
    [SerializeField] private float rotationSpeed = 200f;

    [Space]
    private float pitch;
    [SerializeField] private float minPitch = 5f;
    [SerializeField] private float maxPitch = 80f;

    [Header("Zoom Details")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 21f;

    private float smoothTime = 0.05f;
    private Vector3 movementVelocity = Vector3.zero;
    private Vector3 zoomVelocity = Vector3.zero;
    private Vector3 edgeMovementVelocity = Vector3.zero;
    private Vector3 mouseMovementVelocity = Vector3.zero;
    private Vector3 lastMousePosition;

    private void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canControll)
        {
            return; // Exit early if the camera is not controllable
        }
        HandleRotation();
        HandleZoom();
        HandleEdgeMovement();
        HandleMouseMovement();
        HandleMovement();

        focusPoint.position = transform.position + (transform.forward * GetFocusPointDistance());
    }
    public bool EnableCameraControl(bool enable) => canControll = enable;
    public float AdjustPitchValue(float value) => pitch = value;


    private void HandleZoom()
    {
        float scrool = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoomDirection = transform.forward * scrool * zoomSpeed;
        Vector3 targetPosition = transform.position + zoomDirection;

        if (transform.position.y < minZoom && scrool > 0)
        {
            return;
        }
        if (transform.position.y > maxZoom && scrool < 0)
        {
            return;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref zoomVelocity, smoothTime);
    }
    private float GetFocusPointDistance()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxFocusDistance))
        {
            return hit.distance;
        }
        return maxFocusDistance;
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1))//right mouse button
        {
            // Get the input from the user
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            pitch = Mathf.Clamp(pitch - verticalRotation, minPitch, maxPitch);
            transform.RotateAround(focusPoint.position, Vector3.up, horizontalRotation);


            transform.RotateAround(focusPoint.position, transform.right, pitch - transform.eulerAngles.x);
            transform.LookAt(focusPoint);
        }

    }

    private void HandleMovement()
    {
        Vector3 targetPosition = transform.position;
        // Get the input from the user
        float vInput = Input.GetAxisRaw("Vertical");
        float hInput = Input.GetAxisRaw("Horizontal");

        if (vInput == 0 && hInput == 0)
        {
            return; // No movement input, exit early
        }


        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        if (vInput > 0)
        {
            targetPosition += flatForward * moveSpeed * Time.deltaTime;
        }
        if (vInput < 0)
        {
            targetPosition -= flatForward * moveSpeed * Time.deltaTime;
        }
        if (hInput > 0)
        {
            targetPosition += transform.right * moveSpeed * Time.deltaTime;
        }
        if (hInput < 0)
        {
            targetPosition -= transform.right * moveSpeed * Time.deltaTime;
        }

        if (Vector3.Distance(levelCenterPoint, targetPosition) > maxDistanceFromCenter)
        {
            targetPosition = levelCenterPoint + (targetPosition - levelCenterPoint).normalized * maxDistanceFromCenter;

        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref movementVelocity, smoothTime);
    }

    private void HandleMouseMovement()
    {
        if (Input.GetMouseButtonDown(2)) // Middle mouse button
        {
            lastMousePosition = Input.mousePosition;

        }
        if (Input.GetMouseButton(2)) // Middle mouse button
        {
            Vector3 positionDifference = Input.mousePosition - lastMousePosition;
            Vector3 moveRight = transform.right * (-positionDifference.x) * mouseMovementSpeed * Time.deltaTime;
            Vector3 moveForward = transform.forward * (-positionDifference.y) * mouseMovementSpeed * Time.deltaTime;

            moveRight.y = 0; // Keep the movement on the horizontal plane
            moveForward.y = 0; // Keep the movement on the horizontal plane

            Vector3 movement = moveRight + moveForward;
            Vector3 targetPosition = transform.position + movement;


            if (Vector3.Distance(levelCenterPoint, targetPosition) > maxDistanceFromCenter)
            {
                targetPosition = levelCenterPoint + (targetPosition - levelCenterPoint).normalized * maxDistanceFromCenter;

            }

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref mouseMovementVelocity, smoothTime);
            lastMousePosition = Input.mousePosition;
        }
    }
    private void HandleEdgeMovement()
    {
        Vector3 targetPosition = transform.position;
        Vector3 mousePosition = Input.mousePosition;

        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        if (mousePosition.x > screenWidth - edgeTreshHold)
        {
            targetPosition += transform.right * edgeMovementSpeed * Time.deltaTime;
        }
        if (mousePosition.x < edgeTreshHold)
        {
            targetPosition -= transform.right * edgeMovementSpeed * Time.deltaTime;
        }
        if (mousePosition.y > screenHeight - edgeTreshHold)
        {
            targetPosition += flatForward * edgeMovementSpeed * Time.deltaTime;
        }
        if (mousePosition.y < edgeTreshHold)
        {
            targetPosition -= flatForward * edgeMovementSpeed * Time.deltaTime;
        }
        if (Vector3.Distance(levelCenterPoint, targetPosition) > maxDistanceFromCenter)
        {
            targetPosition = levelCenterPoint + (targetPosition - levelCenterPoint).normalized * maxDistanceFromCenter;

        }
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref edgeMovementVelocity, smoothTime);
    }
}
