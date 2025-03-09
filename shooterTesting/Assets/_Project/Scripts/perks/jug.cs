using UnityEngine;
using Platformer397;

public class jug : MonoBehaviour
{
    [SerializeField] private int price = 2500;
    [SerializeField] private int hpIncAmount = 100; //serializefield in case we want to change it for balancing purposes
    [SerializeField] private PlayerController player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void increaseHealth()
    {
        player.maxHPChange(hpIncAmount);
    }
    public void resetHealth()
    {
        player.maxHPChange(-hpIncAmount);
    }
    public int getCost()
    {
        return price;
    }
}
