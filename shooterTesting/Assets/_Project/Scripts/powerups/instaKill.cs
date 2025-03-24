using UnityEngine;
using Platformer397;
using System.Collections;

public class instaKill : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private Weapon weapon1 = null;
    [SerializeField] private Weapon weapon2 = null;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player.instakill = true;
            player.StartCoroutine(player.instakillTimer(22f));
            Destroy(gameObject);
        }
    }
}
