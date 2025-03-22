using UnityEngine;
using System.Collections.Generic;
using System;
using Platformer397;

public class basicEnemy : MonoBehaviour
{
    //want to make the hp relative to the round late when rounds are implemented
    [SerializeField] private int hp = 150;
    private PlayerController player;
    // [SerializeField] private EnemyManager enemyManager;
    public float timeInside = 0f; //time inside trigger
    public float requiredTimeInside = 0.5f; //how many seconds before player gets slapped
    public bool playerInside = false;
    static System.Random random = new System.Random();

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        // enemyManager = GameObject.FindWithTag("enemyManager").GetComponent<EnemyManager>();
    }

    void Update()
    {
        if(hp <= 0)
        {
            player.addMoney(70);
            if(random.NextDouble() < .05)
            {
                Debug.Log("Spawn power up");
                PowerUpManager.Instance.spawnPowerup(gameObject.transform.position);
            }
            EnemyManager.Instance.EnemyDied(gameObject);
            Destroy(gameObject);}}

    //deal with enemy taking damage
    public void takeDamage(int amount)
    {
        hp-=amount;
    }

    //deal damage function
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player triggers it
        {
            playerInside = true;
            timeInside = 0f; // Reset timer when the player enters
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerInside && other.CompareTag("Player"))
        {
            timeInside += Time.deltaTime;

            if (timeInside >= requiredTimeInside)
            {
                damagePlayer();
                timeInside = 0f; // Reset timer when the player takes damage
            }
        }
    }

    private void OntTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            timeInside = 0f; // Reset timer when the player leaves
        }
    }

    private void damagePlayer()
    {
        player.takeDamage(45);
    }

    //deal with animations

    //deal with ui if we want that

    //deal with attack range
}
