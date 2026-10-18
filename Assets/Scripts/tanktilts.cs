using UnityEngine;

public class TankTilt : MonoBehaviour
{
    public LayerMask groundLayer = ~0;   // Terrain layer'i
    public float rayHeight = 3f;         // nur qancha balandlikdan otiladi
    public float rayLength = 10f;
    public float tiltSpeed = 6f;         // moslashish tezligi
    public float maxTilt = 40f;          // maksimal qiyalik burchagi

    void Update()
    {
        Vector3 origin = transform.position + Vector3.up * rayHeight;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayLength, groundLayer))
        {
            // Tankning oldinga yo'nalishini qiyalik tekisligiga proyeksiya qilamiz
            Vector3 forward = Vector3.ProjectOnPlane(transform.parent.forward, hit.normal).normalized;
            Quaternion target = Quaternion.LookRotation(forward, hit.normal);

            // Haddan tashqari qiyshayib ketmasligi uchun cheklov
            Quaternion flat = Quaternion.LookRotation(transform.parent.forward, Vector3.up);
            target = Quaternion.RotateTowards(flat, target, maxTilt);

            transform.rotation = Quaternion.Slerp(transform.rotation, target, tiltSpeed * Time.deltaTime);
        }
    }
}