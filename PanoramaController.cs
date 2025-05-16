using UnityEngine;

public class PanoramaController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.5f;
    private Vector2 lastMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastMousePosition;
            transform.Rotate(Vector3.up, delta.x * rotationSpeed, Space.World);
            transform.Rotate(Vector3.left, delta.y * rotationSpeed, Space.Self);
            lastMousePosition = Input.mousePosition;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
            transform.Rotate(Vector3.up, touchDelta.x * rotationSpeed, Space.World);
            transform.Rotate(Vector3.left, touchDelta.y * rotationSpeed, Space.Self);
        }
    }
}
