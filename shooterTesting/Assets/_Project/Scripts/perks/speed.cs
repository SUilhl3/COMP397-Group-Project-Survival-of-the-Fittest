using UnityEngine;
using Platformer397;

public class speed : MonoBehaviour
{
    [SerializeField] private int price = 3000;
    [SerializeField] private float speedIncrease = 1.05f;
    [SerializeField] private PlayerController player;
    [SerializeField] private WeaponManager weaponManager;
    public Weapon weapon1 = null;
    public Weapon weapon2 = null;
    private float reloadSpeedIncrease = .5f;

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
    public void increaseMoveSpeed()
    {
        player.moveSpeed *= speedIncrease;
    }
    public void increaseReloadSpeed()
    {
        try{weapon1.reloadTime = weapon1.originalReloadTime * reloadSpeedIncrease;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        try{weapon2.reloadTime = weapon2.originalReloadTime * reloadSpeedIncrease;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
    }
    public void resetEverything()
    {
        player.moveSpeed = player.originalSpeed;
        try{weapon1.reloadTime = weapon1.originalReloadTime;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        try{weapon2.reloadTime = weapon2.originalReloadTime;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        weapon1 = null;
        weapon2 = null;
    }
    public int getCost()
    {
        return price;
    }
}
