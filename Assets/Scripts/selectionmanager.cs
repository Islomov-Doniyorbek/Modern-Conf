using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public LayerMask groundLayer = ~0;  // ~0 = hamma layer
    public LayerMask unitLayer = ~0;

    private Tank selectedTank;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (cam == null) Debug.LogError("Camera.main topilmadi! Kameraga MainCamera tegini qo'ying.");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, unitLayer))
            {
                Debug.Log("Bosildi: " + hit.collider.name);

                Tank tank = hit.collider.GetComponentInParent<Tank>();
                if (tank != null)
                {
                    if (selectedTank != null) selectedTank.SetSelected(false);
                    selectedTank = tank;
                    selectedTank.SetSelected(true);
                    Debug.Log("TANK TANLANDI");
                }
                else
                {
                    Debug.LogWarning("Collider topildi, lekin ustida Tank.cs yo'q!");
                }
            }
            else
            {
                Debug.Log("Hech narsa topilmadi (collider yo'q yoki layer noto'g'ri)");
                if (selectedTank != null) selectedTank.SetSelected(false);
                selectedTank = null;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (selectedTank == null)
            {
                Debug.LogWarning("Avval tankni tanlang (chap tugma)");
                return;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
            {
                Debug.Log("Manzil: " + hit.point);
                selectedTank.MoveTo(hit.point);
            }
            else
            {
                Debug.LogWarning("Zamin topilmadi (zaminda collider yoki layer yo'q)");
            }
        }
    }
}