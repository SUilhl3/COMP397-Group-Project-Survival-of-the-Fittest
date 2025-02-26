using UnityEngine;

public class basicEnemy : MonoBehaviour
{
    //want to make the hp relative to the round late when rounds are implemented
    [SerializeField] private int hp = 150;

    void Update(){if(hp <= 0){Destroy(gameObject);}}

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
