using UnityEngine;
using UnityEngine.AI;
public class AgentMovement : MonoBehaviour
{
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetRandomDestination();
    }
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            SetRandomDestination();
    }
    void SetRandomDestination()
    {
        Vector3 randomPos = new Vector3(Random.Range(-20f, 20f), 0,
        Random.Range(-20f, 20f));
        agent.SetDestination(randomPos);
    }
}