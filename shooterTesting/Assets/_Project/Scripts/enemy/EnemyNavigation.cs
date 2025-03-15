using UnityEngine;
using UnityEngine.AI;

namespace Platformer397
{
    public class EnemyNavigation : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Transform player;

        [SerializeField] private float timer;
        public bool isBlinded = false;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player").transform;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Update()
        {
            // If not blinded work as normal
            if (!isBlinded)
            {

                // Starts timer for blindness effect
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
            if (isBlinded)
            {
                timer += Time.deltaTime;
                if (timer > 5.0f)
                {
                    // Turns blindness back to false once timer is up
                    isBlinded = false;
                    timer = 0;
                }
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
