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

    private void Update()
    {
        if(activeWeaponSlot == weaponSlot1 && !weaponSlot1.activeSelf) 
        {
            weaponSlot1.SetActive(true);
            weaponSlot2.SetActive(false);
        }
        else if(activeWeaponSlot == weaponSlot2 && !weaponSlot2.activeSelf)
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

            pickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
            pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);
            pickedUpWeapon.transform.localScale = Vector3.one;

            weapon.isActiveWeapon = true;
            weapon.animator.enabled = true;
        }
        
        // //if the player has no gun in hand, pick up gun
        else{
            // Debug.Log("No gun in hand");
            pickedUpWeapon.transform.SetParent(weaponSlot1.transform);
            Weapon weapon = pickedUpWeapon.GetComponent<Weapon>();

            pickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
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
