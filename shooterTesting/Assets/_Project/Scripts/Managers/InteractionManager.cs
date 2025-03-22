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
    public MysteryBox box = null;
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
                GameObject obj = objectHitByRaycast.gameObject;
                hoveredWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;
                int weaponPrice = hoveredWeapon.getCost();

                if(Input.GetKeyDown(KeyCode.F) && hoveredWeapon.mysteryWeapon == false)
                {
                    //if you don't have the gun already, check if you can afford the gun
                    if (weaponPrice < player.getMoney() && WeaponManager.Instance.boxCheck(obj) == false)
                    {
                        //makes a copy of the gun selected and adds said copy to the player instead of the buy
                        
                        GameObject copyOfGun = Instantiate(obj);
                            
                        // copyOfGun.transform.SetParent(WeaponManager.Instance.weaponSlot1.transform);

                        //will add gun to slot if its not a dupe
                        bool duplicate = WeaponManager.Instance.PickUpWeapon(copyOfGun);
                        // Debug.Log(duplicate);

                        //if you could buy the gun, then take money from player
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
                        //if you had the gun already, dont take money from the player and destroy the remaining copy of the gun you hovered
                        else{Debug.Log("You already have this gun! Couldn't purchase"); Destroy(copyOfGun);}
                    }

                    //else if you have the gun you are hovering, and you have enough to purchase ammo...
                    else if(weaponPrice / 2 < player.getMoney() && WeaponManager.Instance.boxCheck(obj))
                    {
                        Weapon weapon1 = WeaponManager.Instance.weaponSlot1.transform.GetChild(0).GetComponent<Weapon>(); 
                        Weapon weapon2 = WeaponManager.Instance.weaponSlot2.transform.GetChild(0).GetComponent<Weapon>();
                        //if it is the same gun as your weapon slot 1 or 2, max their ammo respectively if they are not completely full on ammo
                        if(obj.name + "(Clone)" == weapon1.name && !weapon1.fullAmmo()) 
                        {
                            weapon1.maxAmmo();
                            Debug.Log("Bought ammo");
                            player.decreaseMoney(weaponPrice / 2); 
                        }
                        else if(obj.name + "(Clone)" == weapon2.name && !weapon2.fullAmmo()) 
                        {
                            weapon2.maxAmmo();
                            Debug.Log("Bought ammo");
                            player.decreaseMoney(weaponPrice / 2); 
                        }
                        else{Debug.Log("Full ammo already");} //if they are completely full on ammo, don't take their money
                    }
                    else{Debug.Log("Not enough money");} //if you don't have enough money for the thing you were going to buy
                }
                //if you are looking at a mystery box weapon
                else if(Input.GetKeyDown(KeyCode.F) && hoveredWeapon.mysteryWeapon)
                {
                    //dont want to dupe it and dont need to check if dupe since the mystery box rerolls dupes automatically
                    WeaponManager.Instance.AddWeaponIntoActiveSlot(objectHitByRaycast.gameObject);
                    box.inUse = false;
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

                case "MysteryBox":
                    box = objectHitByRaycast.gameObject.GetComponent<MysteryBox>();
                    box.GetComponent<Outline>().enabled = true;
                    int mysteryPrice = box.getCost();
                    if(Input.GetKeyDown(KeyCode.F) && box.inUse == false)
                    {
                        if(player.getMoney() >= mysteryPrice)
                        {
                            box.inUse = true;
                            player.decreaseMoney(mysteryPrice);
                            GameObject mysteryWeapon = Instantiate(box.getRandomWeapon(), box.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                            mysteryWeapon.GetComponent<Collider>().enabled = true;
                            mysteryWeapon.transform.localScale *= 2f;
                            Weapon tempWeapon = mysteryWeapon.GetComponent<Weapon>();
                            tempWeapon.mysteryWeapon = true;
                            tempWeapon.mysteryBoxWeapon();
                            StartCoroutine(DestroyMysteryWeapon(mysteryWeapon, 5f));
                        }
                        else{Debug.Log("Not enough money");}
                    }
                    break;
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
                try
                {
                    if (box == null) {throw new System.NullReferenceException("Box is null");}
                    else if (box.GetComponent<Outline>().enabled == true) {box.GetComponent<Outline>().enabled = false;}
                }catch (System.NullReferenceException ex) {/*Debug.LogError(ex.Message);*/}
        
            }catch (System.Exception ex) {/*Debug.LogError("An unexpected error occurred: " + ex.Message);*/}
        }


    } //end of update 
    // christ that's a long update

    private IEnumerator DestroyMysteryWeapon(GameObject weapon, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(weapon != null && weapon.GetComponent<Collider>().enabled == true)
        {
            Destroy(weapon);
            box.inUse = false;
            // Debug.Log("Mystery weapon destroyed");
        }
    }
}
