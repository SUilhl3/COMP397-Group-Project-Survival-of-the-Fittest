using UnityEngine;
using Platformer397;

public class deadshot : MonoBehaviour
{
    [SerializeField] private int price = 1500;
    [SerializeField] private float HSMIncrease = 1.5f; //increase headshot multiplier by 50%
    [SerializeField] private float spreadReduceAmount = .5f; //reduce spread by 50%
    public Weapon weapon1 = null;
    public Weapon weapon2 = null;
    [SerializeField] private double deadlyShotChance = 0.1; //going to be used later to have a small change to drastically raise HSM for the bullet shot

    public void requestWeapons()
    {
        //If weapon slot 1 is not empty, then get the weapon in slot 1 and change damage
        if(WeaponManager.Instance.weaponSlot1.transform.childCount > 0)
        {
            weapon1 = WeaponManager.Instance.weaponSlot1.transform.GetChild(0).GetComponent<Weapon>();
            // Debug.Log("Weapon 1: " + weapon1.name);
        }
        if(WeaponManager.Instance.weaponSlot2.transform.childCount > 0)
        {
            weapon2 = WeaponManager.Instance.weaponSlot2.transform.GetChild(0).GetComponent<Weapon>();
            // Debug.Log("Weapon 2: " + weapon2.name);
        }
    }

    public void increaseHSM()
    {
        try{weapon1.headShotMultiplier = weapon1.originalHSM * HSMIncrease;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        try{weapon2.headShotMultiplier = weapon2.originalHSM * HSMIncrease;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}

        //for chance headshot multiplier
        try{weapon1.bigDamageChance = deadlyShotChance;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        try{weapon2.bigDamageChance = deadlyShotChance;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
    }

    public void reduceSpread()
    {
        try{weapon1.spreadIntensity = weapon1.originalSpread * spreadReduceAmount;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        try{weapon2.spreadIntensity = weapon2.originalSpread * spreadReduceAmount;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
    }

    public void resetEverything()
    {
        try{weapon1.headShotMultiplier = weapon1.originalHSM;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        try{weapon2.headShotMultiplier = weapon2.originalHSM;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}

        try{weapon1.spreadIntensity = weapon1.originalSpread;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        try{weapon2.spreadIntensity = weapon2.originalSpread;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}

        try{weapon1.bigDamageChance = 0.0;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        try{weapon2.bigDamageChance = 0.0;}
        catch(System.NullReferenceException e){/*Debug.LogError("Weapon is null");*/}
        weapon1 = null;
        weapon2 = null;
    }


    public int getCost(){return price;}
}
