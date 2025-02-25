using UnityEngine;

public class Target : MonoBehaviour
{
    public int hp = 150;

    void Update(){if(hp <= 0){Destroy(gameObject);}}

    public void takeDamage(int amount)
    {
        hp-=amount;
    }
}
