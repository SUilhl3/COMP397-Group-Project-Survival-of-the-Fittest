using UnityEngine;
using System.Collections.Generic;
using System.Collections;
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
        GameObject powerup = Instantiate(powerupToSpawn, spawnPosition, Quaternion.identity);
        StartCoroutine(powerupTimer(powerup, 15f));
    }

    private IEnumerator powerupTimer(GameObject powerUp, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(powerUp);
    }


}
