using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer397;

public class InteractionManager : MonoBehaviour
{


    public static InteractionManager Instance { get; private set; }

    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    [SerializeField] private float rayCastLength = 3.5f;
    [SerializeField] private PlayerController player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        Vector3 rayOrigin = Camera.main.transform.position 
        + Camera.main.transform.forward * rayCastLength 
        - Camera.main.transform.right * .1f; // Moves it .1 units ahead

        Ray ray = new Ray(rayOrigin, Camera.main.transform.forward); 


        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (Physics.Raycast(ray, out hit, 2f) && hit.collider.gameObject.tag == "weapon") //checks if raycast hit is a weapon
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            // Debug.Log(objectHitByRaycast.name);

            if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false) 
            {
                //highlight weapon if raycast is on collider
                hoveredWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;
                int weaponPrice = hoveredWeapon.getCost();
                // Debug.Log(weaponPrice);

                if(Input.GetKeyDown(KeyCode.F)){
                    if (weaponPrice < player.getMoney()) //if you have enough money to buy the gun
                        {
                            //makes a copy of the gun selected and adds said copy to the player instead of the buy
                        
                            GameObject copyOfGun = Instantiate(objectHitByRaycast.gameObject);
                            // copyOfGun.transform.SetParent(WeaponManager.Instance.weaponSlot1.transform);
                            bool duplicate = WeaponManager.Instance.PickUpWeapon(copyOfGun);
                            // Debug.Log(duplicate);
                            if(duplicate == false)
                            {
                                player.decreaseMoney(weaponPrice);
                                // Debug.Log("Purchased " + hoveredWeapon.name);    
                            }
                            else{Debug.Log("You already have this gun! Couldn't purchase"); Destroy(copyOfGun);}
                        }
                    else{Debug.Log("Not enough money");}
                    }
            }

            //Ammo Box
            if (objectHitByRaycast.GetComponent<AmmoBox>())
            {
                hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                hoveredAmmoBox.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                    Destroy(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }
        }
        else{
        try{if(hoveredWeapon == null){throw new System.NullReferenceException("Hovered weapon is null");}
        else if(hoveredWeapon.GetComponent<Outline>().enabled == true){hoveredWeapon.GetComponent<Outline>().enabled = false;}}
        catch(System.NullReferenceException e) 
        {
            //Do this if you want a lot of messages in the console
            // Debug.LogError("Null reference exception caught: " + e);
        }
        }

    }

}
