using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    [Header("Movement")]
    public float panSpeed = 0.02f;   // sezgirlik, o'zingizga moslang

    [Header("Zoom")]
    public float mouseZoomSpeed = 1f;
    public float touchZoomSpeed = 0.02f;
    public float minHeight = 5f;
    public float maxHeight = 35f;

    [Header("Map Bounds (ixtiyoriy)")]
    public bool useBounds = false;
    public Vector2 minXZ = new Vector2(-50, -50);
    public Vector2 maxXZ = new Vector2(50, 50);

    private Vector3 lastMousePosition;
    private bool dragging;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            dragging = false;   // touch bo'lsa sichqoncha mantiqini o'chiramiz
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }

        ClampPosition();
    }

    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            dragging = true;
        }

        if (Input.GetMouseButtonUp(0))
            dragging = false;

        if (dragging && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;
            Pan(delta);
        }

        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
            Zoom(-scroll * mouseZoomSpeed);
    }

    private void HandleTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
                Pan(t.deltaPosition);
        }
        else if (Input.touchCount >= 2)
        {
            Touch a = Input.GetTouch(0);
            Touch b = Input.GetTouch(1);

            Vector2 aPrev = a.position - a.deltaPosition;
            Vector2 bPrev = b.position - b.deltaPosition;

            float prevDist = (aPrev - bPrev).magnitude;
            float currDist = (a.position - b.position).magnitude;

            // barmoqlar yaqinlashsa -> uzoqlashadi (balandlik oshadi)
            Zoom((prevDist - currDist) * touchZoomSpeed);
        }
    }

    private void Pan(Vector2 delta)
    {
        // Faqat XZ tekisligida harakat
        Vector3 right = transform.right;
        Vector3 forward = transform.forward;
        right.y = 0;
        forward.y = 0;
        right.Normalize();
        forward.Normalize();

        // balandlik qancha katta bo'lsa, shuncha tez suriladi
        float heightFactor = transform.position.y * panSpeed;

        Vector3 move = (-right * delta.x - forward * delta.y) * heightFactor;
        transform.position += move;
    }

    private void Zoom(float amount)
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y + amount, minHeight, maxHeight);
        transform.position = pos;
    }

    private void ClampPosition()
    {
        if (!useBounds) return;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minXZ.x, maxXZ.x);
        pos.z = Mathf.Clamp(pos.z, minXZ.y, maxXZ.y);
        transform.position = pos;
    }
}