using UnityEngine;
using Platformer397;

public class doubleTap : MonoBehaviour
{
    [SerializeField] private int price = 2000;
    [SerializeField] private int damageIncrease = 2;
    [SerializeField] private WeaponManager weaponManager;
    public Weapon weapon1 = null;
    public Weapon weapon2 = null;
    [SerializeField] private float fireRateIncrease = .85f;

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

    public void increaseDamage()
    {
        try{weapon1.gunDamage = weapon1.originalGunDamage * damageIncrease;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        try{weapon2.gunDamage = weapon2.originalGunDamage * damageIncrease;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
    }
    public void resetEverything()
    {
        try{
            weapon1.gunDamage = weapon1.originalGunDamage;
            weapon1.shootingDelay = weapon1.originalShootingDelay;
            }catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        try{
            weapon2.gunDamage = weapon2.originalGunDamage;
            weapon2.shootingDelay = weapon2.originalShootingDelay;
            }catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        weapon1 = null;
        weapon2 = null;
    }
    public void increaseFireRate()
    {
        try{weapon1.shootingDelay = weapon1.originalShootingDelay * fireRateIncrease;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        try{weapon2.shootingDelay = weapon2.originalShootingDelay * fireRateIncrease;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
    }
        
    public int getCost()
    {
        return price;
    }
}
