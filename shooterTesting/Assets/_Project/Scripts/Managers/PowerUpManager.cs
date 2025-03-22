using UnityEngine;
using System.Collections.Generic;
using Platformer397;
public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }


    //store powerup prefabs. Prefabs are objects with powerup scripts on them
    [SerializeField] private List<GameObject> powerups;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject choosePowerup()
    {
        int randomIndex = Random.Range(0, powerups.Count);
        GameObject selectedPowerup = powerups[randomIndex];
        return selectedPowerup;
    }

    public void spawnPowerup(Vector3 spawnPosition)
    {
        GameObject powerupToSpawn = choosePowerup();
        Instantiate(powerupToSpawn, spawnPosition, Quaternion.identity);
    }


}
