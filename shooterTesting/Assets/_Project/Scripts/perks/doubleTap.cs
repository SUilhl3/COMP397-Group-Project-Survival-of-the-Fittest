using UnityEngine;
using Platformer397;

public class doubleTap : MonoBehaviour
{
    [SerializeField] private int price = 2000;
    [SerializeField] private int damageIncrease = 2;
    [SerializeField] private PlayerController player;
    [SerializeField] private WeaponManager weaponManager;
    public Weapon weapon1 = null;
    public Weapon weapon2 = null;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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

    public void increaseDamage(Weapon weapon)
    {
        try{weapon.gunDamage = weapon.originalGunDamage * damageIncrease;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
    }
    public void resetDamage(Weapon weapon)
    {
        weapon.gunDamage = weapon.originalGunDamage;
    }
        
    public int getCost()
    {
        return price;
    }
}
