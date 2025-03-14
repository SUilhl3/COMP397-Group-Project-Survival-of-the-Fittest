using UnityEngine;
using Platformer397;

public class jug : MonoBehaviour
{
    [SerializeField] private int price = 2500;
    [SerializeField] private PlayerController player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void increaseHealth()
    {
        player.playerMaxHealth = 200;
    }
    public void resetHealth()
    {
        player.playerMaxHealth = 100;
    }
    public int getCost()
    {
        return price;
    }
}
