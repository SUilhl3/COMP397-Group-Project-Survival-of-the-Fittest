using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer397;

public class InteractionManager : MonoBehaviour
{


    public static InteractionManager Instance { get; private set; }

    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    public Throwable hoveredThrowable = null;
    [SerializeField] private float rayCastLength = 3.5f;
    [SerializeField] private PlayerController player;

    //perk initialization for disabling the outline
    public jug jugger = null;
    public doubleTap dbTap = null;
    public speed speedCola = null;
    public quickRevive quick = null;
    public deadshot dShot = null;
    //barrier init for disabling the outline
    public buyBarriers barrier;


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
        if (Physics.Raycast(ray, out hit, 3f)) //checks if raycast hit is a weapon
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
                                if(player.checkPerk("double-tap"))
                                {
                                    dbTap.requestWeapons();
                                    dbTap.increaseDamage();
                                    dbTap.increaseFireRate();
                                }   
                                if(player.checkPerk("speed"))
                                {
                                    speedCola.requestWeapons();
                                    speedCola.increaseMoveSpeed();
                                    speedCola.increaseReloadSpeed();
                                }
                                if(player.checkPerk("deadshot"))
                                {
                                    dShot.requestWeapons();
                                    dShot.increaseHSM();
                                }
                            }
                            else{Debug.Log("You already have this gun! Couldn't purchase"); Destroy(copyOfGun);}
                        }
                    else{Debug.Log("Not enough money");}
                    }
            }

            else if(objectHitByRaycast.GetComponent<buyBarriers>())
            {
                barrier = objectHitByRaycast.GetComponent<buyBarriers>();
                barrier.GetComponent<Outline>().enabled = true;
                int barrierCost = barrier.getCost();
                if(Input.GetKeyDown(KeyCode.F))
                {
                    if(player.getMoney() >= barrierCost) {barrier.purchased();player.decreaseMoney(barrierCost);}
                    else{Debug.Log("Not enough money");}
                }
            }
            
            switch(objectHitByRaycast.gameObject.name) //checks if raycast hit is a jug, speedcola, doubletap, quickrevive, or mysterybox
            {
                case "jug": //if hovering over jug
                    if(player.checkPerk("jug")){/*Debug.Log("Already have jug");*/break;} //checks if player already has jug and if so, breaks out of the switch
                    jugger = objectHitByRaycast.gameObject.GetComponent<jug>(); //assigning the components of the jug to perk
                    jugger.GetComponent<Outline>().enabled = true;
                    int jugPrice = jugger.getCost();
                    if(Input.GetKeyDown(KeyCode.F)) //if want to buy
                    {
                        if(player.getMoney() >= jugPrice) //check if player has enough money
                        {
                            //buy perk
                            player.decreaseMoney(jugPrice); 
                            jugger.increaseHealth();//goes to perk then player, dont know if simplifying it is better. Seems kinda hard coding
                            player.addPerk("jug"); //checks off a bool to check if the player tries to buy it again
                            Debug.Log("Purchased Jug");
                        }
                        else{Debug.Log("Not enough money");}
                    }
                    break;
                case "speed":
                    if(player.checkPerk("speed")){/*Debug.Log("Already have speed");*/break;}
                    speedCola = objectHitByRaycast.gameObject.GetComponent<speed>();
                    speedCola.GetComponent<Outline>().enabled = true;
                    int speedPrice = speedCola.getCost();
                    if(Input.GetKeyDown(KeyCode.F))
                    {
                        if(player.getMoney() >= speedPrice)
                        {
                            //buy perk
                            player.decreaseMoney(speedPrice);
                            speedCola.requestWeapons();
                            speedCola.increaseMoveSpeed();
                            speedCola.increaseReloadSpeed();
                            player.addPerk("speed");
                            Debug.Log("Purchased Speed Cola");
                        }
                        else{Debug.Log("Not enough money");}
                    }
                    break;
                case "double-tap":
                    if(player.checkPerk("double-tap")){/*Debug.Log("Already have double-tap");*/break;}
                    dbTap = objectHitByRaycast.gameObject.GetComponent<doubleTap>();
                    dbTap.GetComponent<Outline>().enabled = true;
                    int dbTapPrice = dbTap.getCost();
                    if(Input.GetKeyDown(KeyCode.F))
                    {
                        if(player.getMoney() >= dbTapPrice)
                        {
                            //buy perk
                            player.decreaseMoney(dbTapPrice);
                            dbTap.requestWeapons(); //get the current weapons in the weapon slots
                            dbTap.increaseDamage(); //increase the damage of the weapons
                            dbTap.increaseFireRate(); //increase the fire rate of the weapons
                            player.addPerk("double-tap");
                            Debug.Log("Purchased Double-tap");
                        }
                        else{Debug.Log("Not enough money");}
                    }
                    break;
                case "quick-revive":
                    if(player.checkPerk("quick-revive")){/*Debug.Log("Already have quick-revive");*/break;}
                    quick = objectHitByRaycast.gameObject.GetComponent<quickRevive>();
                    quick.GetComponent<Outline>().enabled = true;
                    int quickPrice = quick.getCost();
                    if(Input.GetKeyDown(KeyCode.F))
                    {
                        if(player.getMoney() >= quickPrice)
                        {
                            player.decreaseMoney(quickPrice);
                            player.addPerk("quick-revive");
                        }
                        else{Debug.Log("Not enough money");}
                    }
                    break;

                case "deadshot":
                    if(player.checkPerk("deadshot")){/*Debug.Log("Already have deadshot");*/break;}
                    dShot = objectHitByRaycast.gameObject.GetComponent<deadshot>();
                    dShot.GetComponent<Outline>().enabled = true;
                    int dShotPrice = dShot.getCost();
                    if(Input.GetKeyDown(KeyCode.F))
                    {
                        if(player.getMoney() >= dShotPrice)
                        {
                            player.decreaseMoney(dShotPrice);
                            dShot.requestWeapons();
                            dShot.increaseHSM();
                            dShot.reduceSpread();
                            player.addPerk("deadshot");
                        }
                        else{Debug.Log("Not enough money");}
                    }
                    break;

                // case "MysteryBox":
                //     if(Input.GetKeyDown(KeyCode.F))
                //     {
                //         if(player.getMoney() >= objectHitByRaycast.GetComponent<MysteryBox>().getCost())
                //         {
                //             player.decreaseMoney(objectHitByRaycast.GetComponent<MysteryBox>().getCost());
                //             objectHitByRaycast.GetComponent<MysteryBox>().randomWeapon();
                //         }
                //         else{Debug.Log("Not enough money");}
                //     }
                //     break;
            }

            

            //Ammo Box
            // if (objectHitByRaycast.GetComponent<AmmoBox>())
            // {
            //     hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
            //     hoveredAmmoBox.GetComponent<Outline>().enabled = true;

            //     if (Input.GetKeyDown(KeyCode.F))
            //     {
            //         // WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
            //         Destroy(objectHitByRaycast.gameObject);
            //     }
            // }
            // else
            // {
            //     if (hoveredAmmoBox)
            //     {
            //         hoveredAmmoBox.GetComponent<Outline>().enabled = false;
            //     }
            // }

            // Throwable
            if (objectHitByRaycast.GetComponent<Throwable>())
            {
                 hoveredThrowable = objectHitByRaycast.gameObject.GetComponent<Throwable>();
                 hoveredThrowable.GetComponent<Outline>().enabled = true;

                 if (Input.GetKeyDown(KeyCode.F))
                 {
                     WeaponManager.Instance.PickupThrowable(hoveredThrowable);
                 }
             }
             else
             {
                 if (hoveredThrowable)
                 {
                     hoveredThrowable.GetComponent<Outline>().enabled = false;
                 }
             }
        }

        else
        {
           try
            {
                try //disable the outline of the weapon when not hovering over it
                {
                    if (hoveredWeapon == null) {throw new System.NullReferenceException("Hovered weapon is null");}
                    else if (hoveredWeapon.GetComponent<Outline>().enabled == true) {hoveredWeapon.GetComponent<Outline>().enabled = false;}}
                catch (System.NullReferenceException ex) {/*Debug.LogError(ex.Message);*/}

                try //disable the outline of jug when not hovering over them
                {
                    if (jugger == null) {throw new System.NullReferenceException("Jug is null");}
                    else if (jugger.GetComponent<Outline>().enabled == true) {jugger.GetComponent<Outline>().enabled = false;}}
                catch (System.NullReferenceException ex) {/*Debug.LogError(ex.Message);*/}

                // Disable the outline of the rest of the perks when made:
                try
                {
                    if (dbTap == null) {throw new System.NullReferenceException("Double-tap is null");}
                    else if (dbTap.GetComponent<Outline>().enabled == true) {dbTap.GetComponent<Outline>().enabled = false;}
                }catch (System.NullReferenceException ex) {/*Debug.LogError(ex.Message);*/}
                try
                {
                    if (speedCola == null) {throw new System.NullReferenceException("Speed Cola is null");}
                    else if (speedCola.GetComponent<Outline>().enabled == true) {speedCola.GetComponent<Outline>().enabled = false;}
                }catch (System.NullReferenceException ex) {/*Debug.LogError(ex.Message);*/}
                try
                {
                    if (quick == null) {throw new System.NullReferenceException("Quick-revive is null");}
                    else if (quick.GetComponent<Outline>().enabled == true) {quick.GetComponent<Outline>().enabled = false;}
                }catch (System.NullReferenceException ex) {/*Debug.LogError(ex.Message);*/}
                try
                {
                    if (dShot == null) {throw new System.NullReferenceException("Deadshot is null");}
                    else if (dShot.GetComponent<Outline>().enabled == true) {dShot.GetComponent<Outline>().enabled = false;}
                }catch (System.NullReferenceException ex) {/*Debug.LogError(ex.Message);*/}
                try
                {
                    if (barrier == null) {throw new System.NullReferenceException("Barrier is null");}
                    else if (barrier.GetComponent<Outline>().enabled == true) {barrier.GetComponent<Outline>().enabled = false;}
                }catch (System.NullReferenceException ex) {/*Debug.LogError(ex.Message);*/}
        
            }catch (System.Exception ex) {/*Debug.LogError("An unexpected error occurred: " + ex.Message);*/}
        }


    } //end of update 
    // christ that's a long update

}
