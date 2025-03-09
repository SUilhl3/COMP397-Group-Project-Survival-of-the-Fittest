using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class WeaponManager : MonoBehaviour
{

    public static WeaponManager Instance { get; private set; }
    public GameObject weaponSlot1;
    public GameObject weaponSlot2;

    public GameObject activeWeaponSlot;

    [Header("Throwables General")]
    public float throwForce = 10f;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0f;
    public float forceMultiplierLimit = 2f;

    [Header("Lethals")]
    public int maxLethals = 2;
    public int lethalsCount = 0;
    public Throwable.ThrowableType equippedLethalType;
    public GameObject grenadePrefab;

    [Header("Tacticals")]
    public int maxTacticals = 2;
    public int tacticalsCount = 0;
    public Throwable.ThrowableType equippedTacticalType;
    public GameObject smokeGrenadePrefab;

    // [Header("Ammo")]
    // public int totalRifleAmmo = 0;
    // public int totalPistolAmmo = 0;

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
        activeWeaponSlot = weaponSlot1;
    }

    public void Start()
    {
        equippedLethalType = Throwable.ThrowableType.None;
        equippedTacticalType = Throwable.ThrowableType.None;
    }

    private void Update()
    {
        if(activeWeaponSlot == weaponSlot1) 
        {
            weaponSlot1.SetActive(true);
            weaponSlot2.SetActive(false);
        }
        else if(activeWeaponSlot == weaponSlot2)
        {
            weaponSlot1.SetActive(false);
            weaponSlot2.SetActive(true);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        if (Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.T))
        {
            forceMultiplier += Time.deltaTime;
            if(forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        if (Input.GetKeyUp(KeyCode.G))
        {

            if(lethalsCount > 0)
            {
                ThrowLethal();
            }
            forceMultiplier = 0;
        }

        if (Input.GetKeyUp(KeyCode.T))
        {

            if (tacticalsCount > 0)
            {
                ThrowTacticals();
            }
            forceMultiplier = 0;
        }

    }



    public bool PickUpWeapon(GameObject pickedUpWeapon)
{
    // Debug.Log("Pick up weapon");
    bool isDuplicate = false; 

    try
    {
        if (pickedUpWeapon.GetInstanceID() == weaponSlot1.transform.GetChild(0).GetInstanceID() ||
         pickedUpWeapon.GetInstanceID() == weaponSlot2.transform.GetChild(0).GetInstanceID())
        {
            Debug.Log("Duplicate weapon, cannot buy");
            isDuplicate = true; // Mark as duplicate
        }
    }
    catch (IndexOutOfRangeException e)
    {
        Debug.LogError("Error: Out of bounds object - " + e.Message);
    }
    catch (NullReferenceException e)
    {
        Debug.LogError("Error: Null reference - " + e.Message);
    }
    catch (Exception e) // Catch any other unexpected errors
    {
        Debug.LogError("An unexpected error occurred: " + e.Message);
    }
    finally
    {
        // If it's NOT a duplicate, add the weapon
        if (!isDuplicate)
        {
            AddWeaponIntoActiveSlot(pickedUpWeapon);
        }
    }
    return isDuplicate;
}


    private void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {
        // Debug.Log("Add weapon");
        pickedUpWeapon.GetComponent<Collider>().enabled = false;
        // //if there is a weapon in the active and non-active slot, drop the current weapon for the new one
        if(activeWeaponSlot.transform.childCount > 0 && weaponSlot1.transform.childCount > 0 && weaponSlot2.transform.childCount > 0){
            Debug.Log("Both are full");
            Destroy(activeWeaponSlot.transform.GetChild(0).gameObject);
            }

        // //if there is a weapon in the primary slot but not the secondary, put the current weapon in second slot and take the new gun as active weapon
        if(activeWeaponSlot.transform.childCount > 0 && weaponSlot2.transform.childCount == 0)
        {
            // Debug.Log("Second slot free");
            //change the active weapon to secondary slot
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
            currentWeapon.transform.SetParent(weaponSlot2.transform, false);
            currentWeapon.transform.localScale = Vector3.one;

            //pickup and assign the weapon to the active slot
            pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);

            Weapon weapon = pickedUpWeapon.GetComponent<Weapon>();

            pickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x + 0.2f, weapon.spawnPosition.y, weapon.spawnPosition.z);
            pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);
            pickedUpWeapon.transform.localScale = Vector3.one;

            weapon.isActiveWeapon = true;
            weapon.animator.enabled = true;
        }
        else if(activeWeaponSlot.transform.childCount > 0 && weaponSlot1.transform.childCount == 0)
        {
            // Debug.Log("First slot free");
            //change the active weapon to primary slot
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
            currentWeapon.transform.SetParent(weaponSlot1.transform, false);
            currentWeapon.transform.localScale = Vector3.one;

            //pickup and assign the weapon to the active slot
            pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);

            Weapon weapon = pickedUpWeapon.GetComponent<Weapon>();

            pickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x + 0.2f, weapon.spawnPosition.y, weapon.spawnPosition.z);
            pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);
            pickedUpWeapon.transform.localScale = Vector3.one;

            weapon.isActiveWeapon = true;
            weapon.animator.enabled = true;
        }
        
        // //if the player has no gun in hand, pick up gun
        else{
            // Debug.Log("No gun in hand");
            pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform);
            Weapon weapon = pickedUpWeapon.GetComponent<Weapon>();

            pickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x + 0.2f, weapon.spawnPosition.y, weapon.spawnPosition.z);
            pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);
            pickedUpWeapon.transform.localScale = Vector3.one;

            weapon.isActiveWeapon = true;
            weapon.animator.enabled = true;
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        if(slotNumber == 0)
        {
            activeWeaponSlot = weaponSlot1;
        }
        else if(slotNumber == 1)
        {
            activeWeaponSlot = weaponSlot2;
        }

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }

    #region || -- Throwables -- ||
    public void PickupThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                PickupThrowableAsLethal(Throwable.ThrowableType.Grenade);
                break;
            case Throwable.ThrowableType.Smoke_Grenade:
                PickupThrowableAsTactical(Throwable.ThrowableType.Smoke_Grenade);
                break;
        }
    }

    private void PickupThrowableAsTactical(Throwable.ThrowableType tactical)
    {
        if (equippedTacticalType == tactical || equippedTacticalType == Throwable.ThrowableType.None)
        {
            equippedTacticalType = tactical;
            if (tacticalsCount < maxTacticals)
            {
                tacticalsCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUDManager.Instance.UpdateThrowables();
            }
            else
            {
                print("tactical limit reached");
            }
        }
        else
        {
            // Cannot pickup different tacticals
            //option to swap tacticals
        }
    }

    private void PickupThrowableAsLethal(Throwable.ThrowableType lethal)
    {
        if(equippedLethalType == lethal || equippedLethalType == Throwable.ThrowableType.None)
        {
            equippedLethalType = lethal;
            if(lethalsCount < maxLethals)
            {
                lethalsCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUDManager.Instance.UpdateThrowables();
            }
            else
            {
                print("Lethals limit reached");
            }
        }
    }


    private void ThrowLethal()
    {
        GameObject lethalPrefab = GetThrowablePrefab(equippedLethalType);

        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        lethalsCount -= 1;

        if(lethalsCount <= 0)
        {
            equippedLethalType = Throwable.ThrowableType.None;
        }

        HUDManager.Instance.UpdateThrowables();
    }

    private void ThrowTacticals()
    {
        GameObject tacticalPrefab = GetThrowablePrefab(equippedTacticalType);

        GameObject throwable = Instantiate(tacticalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        tacticalsCount -= 1;

        if (tacticalsCount <= 0)
        {
            equippedLethalType = Throwable.ThrowableType.None;
        }

        HUDManager.Instance.UpdateThrowables();
    }

    private GameObject GetThrowablePrefab(Throwable.ThrowableType throwableType)
    {
        switch (throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                return grenadePrefab;
            case Throwable.ThrowableType.Smoke_Grenade:
                return smokeGrenadePrefab;
        }
        return new();
    }
    #endregion

    // internal void PickupAmmo(AmmoBox ammo)
    // {
    //     switch (ammo.ammoType)
    //     {
    //         case AmmoBox.AmmoType.PistolAmmo:
    //             totalPistolAmmo += ammo.ammoAmount;
    //             break;
    //         case AmmoBox.AmmoType.RifleAmmo:
    //             totalRifleAmmo += ammo.ammoAmount;
    //             break;
    //     }
    // }

    // internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel thisWeaponModel)
    // {
    //     switch (thisWeaponModel)
    //     {
    //         case Weapon.WeaponModel.Rifle:
    //             totalRifleAmmo -= bulletsToDecrease;
    //             break;
    //         case Weapon.WeaponModel.Pistol:
    //             totalPistolAmmo -= bulletsToDecrease;
    //             break;
    //     }
    // }

    // public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    // {
    //     switch (thisWeaponModel)
    //     {
    //         case Weapon.WeaponModel.Rifle:
    //             return totalRifleAmmo;

    //         case Weapon.WeaponModel.Pistol:
    //             return totalPistolAmmo;

    //         default:
    //             return 0;
    //     }
    // }
}
