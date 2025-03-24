using UnityEngine;
using Platformer397;
public class nukes : MonoBehaviour
{
    private PlayerController player;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Nuke");
            EnemyManager.Instance.killAllEnemies();
            player.addMoney(400);
            Destroy(gameObject);
        }
    }
}
