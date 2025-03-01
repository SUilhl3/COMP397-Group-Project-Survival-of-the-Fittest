using UnityEngine;
using Platformer397;

public class basicEnemy : MonoBehaviour
{
    //want to make the hp relative to the round late when rounds are implemented
    [SerializeField] private int hp = 150;
    private PlayerController player;
    [SerializeField] private EnemyManager enemyManager;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        enemyManager = GameObject.FindWithTag("enemyManager").GetComponent<EnemyManager>();
    }

    void Update(){if(hp <= 0){player.addMoney(70);enemyManager.EnemyDied();Destroy(gameObject);}}

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
