using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemySpawner;
    private Transform player;
    public float spawnRadius = 5f;
    public string spawnersRoom; //just going to manually assign these for now

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public void SpawnEnem()
    {
        // Debug.Log("Spawn function");
        Vector3 spawnPos = enemySpawner.position;
        UnityEngine.AI.NavMeshHit hit;
        
        // Find the closest valid position on the NavMesh
        if (UnityEngine.AI.NavMesh.SamplePosition(spawnPos, out hit, spawnRadius, UnityEngine.AI.NavMesh.AllAreas))
        {
            GameObject enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity); 
            enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(player.position); // Set the destination of the enemy to the player
        }
        else
        {
            Debug.LogWarning("No valid NavMesh position found near spawn point!");
        }
    }

}
