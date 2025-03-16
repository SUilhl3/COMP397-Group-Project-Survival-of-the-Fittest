using UnityEngine;
using Platformer397;
using UnityEngine.AI;
using Unity.AI.Navigation;
public class buyBarriers : MonoBehaviour
{
    [SerializeField] private int price; //to change in editor as different doors might cost different amounts

    public void purchased()
    {
        Debug.Log("You bought the door");
        Destroy(gameObject);
    }
    public int getCost() {return price;}
}
