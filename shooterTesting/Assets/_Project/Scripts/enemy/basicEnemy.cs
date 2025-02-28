using UnityEngine;
using Platformer397;

public class basicEnemy : MonoBehaviour
{
    //want to make the hp relative to the round late when rounds are implemented
    [SerializeField] private int hp = 150;
    PlayerController player;
    EnemySpawner enemSpawn;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        enemSpawn = GameObject.FindWithTag("enemSpawnPoint").GetComponent<EnemySpawner>();
    }

    void Update(){if(hp <= 0){player.addMoney(70);enemSpawn.SpawnEnem();Destroy(gameObject);}}

    //deal with enemy taking damage
    public void takeDamage(int amount)
    {
        hp-=amount;
    }

    //deal damage function

    //deal with animations

    //deal with ui if we want that

    //deal with attack range
}
