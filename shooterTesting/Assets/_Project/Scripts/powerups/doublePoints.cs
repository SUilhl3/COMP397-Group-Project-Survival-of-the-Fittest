using UnityEngine;
using Platformer397;

public class doublePoints : MonoBehaviour
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
            player.doublePoints = true;
            player.StartCoroutine(player.doublePointsTimer(22f));
            Destroy(gameObject);
        }
    }
}
