using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Tank : MonoBehaviour
{
    public GameObject selectionCircle;
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        if (selectionCircle != null)
            selectionCircle.SetActive(selected);
    }

    public void MoveTo(Vector3 target)
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogError("Tank NavMesh ustida emas! NavMesh Bake qilinmagan yoki tank zamindan uzoqda.");
            return;
        }
        bool ok = agent.SetDestination(target);
        Debug.Log("SetDestination: " + ok);
    }
}