using UnityEngine;

public class maxAmmo : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Maximum ammunition");
            Weapon weapon1 = WeaponManager.Instance.weaponSlot1.transform.GetChild(0).GetComponent<Weapon>(); 
            Weapon weapon2 = WeaponManager.Instance.weaponSlot2.transform.GetChild(0).GetComponent<Weapon>();
            weapon1.maxAmmo();
            weapon2.maxAmmo();
            Destroy(gameObject);
        }

    }
}
