using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using Platformer397;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private EnemySpawner[] allSpawners; //holds the spawners available
    [SerializeField] private EnemySpawner enemSpawn; //the selected spawner
    private float changeInterval = 5f; //the time it takes to spawn an enemy

    [SerializeField] private int round = 0; //the current round
    [SerializeField] private int enemyStartingAmount; //the amount of enemies in the round
    [SerializeField] private int enemiesLeft; //the amount of enemies left in the round
    [SerializeField] private int enemiesSpawned;
    [SerializeField] private float roundModifier;
    private PlayerController player;
    private string playerRoom;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerRoom = player.currentRoom;
    }
    void Start()
    {
        IncreaseRound();

        InvokeRepeating("AssignRandomSpawner", 0f, changeInterval);
    }

    public void Update()
    {
        if(player.currentRoom != playerRoom)
        {
            playerRoom = player.currentRoom;
        }
    }

    public void EnemyDied()
    {
        enemiesLeft--;
    }
    async void AssignRandomSpawner()
    {
        //if the round is 5, stop the spawners from spawning
        if(round == 5)
        {
            CancelInvoke("AssignRandomSpawner");
        }
        //if there are no enemies left in the round, increase the round
        if(enemiesLeft <= 0)
        {
            IncreaseRound();
            enemiesSpawned = 0;
            await Task.Delay(15000); //should wait for 15 seconds before next round commences
        }
        //if there are still enemies to spawn, spawn them
        if(enemiesSpawned < enemyStartingAmount)
        {
            allSpawners = FindObjectsOfType<EnemySpawner>().Where(spawner => spawner.spawnersRoom == playerRoom).ToArray();
            //randomly choose a spawner for the enemy to spawn at
            enemSpawn = allSpawners[Random.Range(0, allSpawners.Length)];
            enemSpawn.SpawnEnem();
            enemiesSpawned++;
        } 
    }

    void IncreaseRound()
    {
        round++;

        if(round < 10){
            switch(round)
        {
            case 1:
                roundModifier = .25f;
                break;
            case 2:
                roundModifier = .3f;
                break;
            case 3:
                roundModifier = .5f;
                break;
            case 4:
                roundModifier = .7f;
                break;
            case 5:
                roundModifier = .9f;
                break;
            default:
                roundModifier = 1f;
                break;
        }

        enemyStartingAmount = Mathf.RoundToInt((24 + (0.5f * 6 * Mathf.Max(1, round / 5)))*roundModifier);
        }

        else if(round >= 10)
        {
            enemyStartingAmount = Mathf.RoundToInt(24f + (0.5f * 6f * (round / 5f) * round * 0.15f));
        }

        enemiesLeft = enemyStartingAmount;
        Debug.Log("Round: " + round + " Enemy Amount: " + enemyStartingAmount);
    }
}
