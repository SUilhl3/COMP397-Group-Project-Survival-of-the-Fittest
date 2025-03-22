using UnityEngine;
using System.Collections.Generic;
public class MysteryBox : MonoBehaviour
{
    [SerializeField] private int price;
    public Weapon weapon1 = null;
    public Weapon weapon2 = null;
    public bool inUse = false;
    [SerializeField] private WeaponManager weaponManager;

    [Header("List of weapon prefabs")]
    [SerializeField] private List<GameObject> weaponPrefabs;

    void Awake()
    {
        weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
    }

    public void requestWeapons()
    {
        //If weapon slot 1 is not empty, then get the weapon in slot 1 and change damage
        if(weaponManager.weaponSlot1.transform.childCount > 0)
        {
            weapon1 = weaponManager.weaponSlot1.transform.GetChild(0).GetComponent<Weapon>();
            // Debug.Log("Weapon 1: " + weapon1.name);
        }
        if(weaponManager.weaponSlot2.transform.childCount > 0)
        {
            weapon2 = weaponManager.weaponSlot2.transform.GetChild(0).GetComponent<Weapon>();
            // Debug.Log("Weapon 2: " + weapon2.name);
        }
    }

    public GameObject getRandomWeapon()
    {
        int randomIndex = Random.Range(0, weaponPrefabs.Count);
        bool dupe = weaponManager.boxCheck(weaponPrefabs[randomIndex]);
        while(dupe){randomIndex = Random.Range(0,weaponPrefabs.Count); dupe = weaponManager.boxCheck(weaponPrefabs[randomIndex]);}
        return weaponPrefabs[randomIndex];
    }
    public int getCost() {return price;}
}
