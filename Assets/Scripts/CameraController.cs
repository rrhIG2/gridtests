using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Pan Settings")]
    public float panSpeed = 0.1f;

    [Header("Vertical Movement Settings")]
    public float verticalSpeed = 0.1f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 1f;
    public float horizontalRotationSpeed = 1f;

    private Vector2 lastPanPosition;

    void Update()
    {
        HandlePan();
        HandleVertical();
        HandleRotation();
    }

    void HandlePan()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDelta = touch.position - lastPanPosition;

                if (Mathf.Abs(touchDelta.x) > Mathf.Abs(touchDelta.y))
                {
                    Vector3 move = new Vector3(touchDelta.x * panSpeed * Time.deltaTime, 0, 0);
                    transform.Translate(move, Space.World);
                }
                else
                {
                    Vector3 move = new Vector3(0, 0, touchDelta.y * panSpeed * Time.deltaTime);
                    transform.Translate(move, Space.World);
                }

                lastPanPosition = touch.position;
            }
        }
    }

    void HandleVertical()
    {
        if (Input.touchCount == 2)
        {
            Touch touchOne = Input.GetTouch(0);
            Touch touchTwo = Input.GetTouch(1);

            if (touchOne.phase == TouchPhase.Moved && touchTwo.phase == TouchPhase.Moved)
            {
                float averageY = (touchOne.deltaPosition.y + touchTwo.deltaPosition.y) / 2;
                Vector3 verticalMove = new Vector3(0, averageY * verticalSpeed * Time.deltaTime, 0);
                transform.Translate(verticalMove, Space.World);
            }
        }
    }

    void HandleRotation()
    {
        if (Input.touchCount == 3)
        {
            Touch touchOne = Input.GetTouch(0);
            Touch touchTwo = Input.GetTouch(1);
            Touch touchThree = Input.GetTouch(2);

            if (touchOne.phase == TouchPhase.Moved && touchTwo.phase == TouchPhase.Moved && touchThree.phase == TouchPhase.Moved)
            {
                float averageY = (touchOne.deltaPosition.y + touchTwo.deltaPosition.y + touchThree.deltaPosition.y) / 3;
                float averageX = (touchOne.deltaPosition.x + touchTwo.deltaPosition.x + touchThree.deltaPosition.x) / 3;

                // Vertical Rotation (Up/Down)
                if (Mathf.Abs(averageY) > Mathf.Abs(averageX))
                {
                    transform.Rotate(Vector3.right, -averageY * rotationSpeed * Time.deltaTime, Space.World);
                }

                // Horizontal Rotation (Left/Right)
                if (Mathf.Abs(averageX) > Mathf.Abs(averageY))
                {
                    transform.Rotate(Vector3.up, averageX * horizontalRotationSpeed * Time.deltaTime, Space.World);
                }
            }
        }
    }
}

