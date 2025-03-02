using UnityEngine;
using UnityEngine.AI;

namespace Platformer397
{
    public class EnemyNavigation : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Transform player;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player").transform;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Update()
        {
            agent.destination = player.position;
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                RotateTowardsVelocity();
            }
            else
            {
                RotateTowardsPlayer();
            }
        }

        void RotateTowardsVelocity()
    {
        if (agent.velocity.sqrMagnitude > 0.1f) // Avoid rotating when idle
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0;

        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    }
}
