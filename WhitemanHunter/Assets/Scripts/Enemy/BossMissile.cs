
using UnityEngine.AI;

public class BossMissile : Bullet
{
    NavMeshAgent agent;

    private void Awake()
    {
        agent = transform.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.SetDestination(GameManager.Instance.Player.transform.position);
    }
}
