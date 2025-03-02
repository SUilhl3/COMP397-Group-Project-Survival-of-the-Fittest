using UnityEngine;
using Platformer397;

public class roomChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerController>().currentRoom = gameObject.name;
            Debug.Log("Player is in " + gameObject.name);
        }
    }
}
