using UnityEngine;
using Platformer397;

public class hazard : MonoBehaviour
{
    [SerializeField] private float damageTime = 0f;
    [SerializeField] private float timeTillDamage = .66f;
    [SerializeField] private int damage = 24;
    private PlayerController player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(damageTime >= timeTillDamage)
        {
            player.takeDamage(damage);
            damageTime = 0f;
        }
    }
    //could make the damageTime increase by more upon first entering so that it is more punishing
    // private void OnTriggerEnter(Collider other)
    // {
    //     if(other.tag == "Player")
    //     {
    //         damageTime += Time.deltaTime;
    //     }
    // }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            damageTime += Time.deltaTime;   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        damageTime = 0;
    }
}
