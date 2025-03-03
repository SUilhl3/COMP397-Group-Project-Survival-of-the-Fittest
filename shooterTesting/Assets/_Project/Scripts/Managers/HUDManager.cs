using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    //Stuff that is commented out is to be worked on later because it currently just clones sprites a bunch
    public static HUDManager Instance { get; private set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;

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
    }

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            // totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            // ammoTypeUI.sprite = GetAmmoSprite(model);

            // activeWeaponUI.sprite = GetWeaponSprite(model);
        }
        if (unActiveWeapon)
        {
            // unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
        }
        else if (!activeWeapon && !unActiveWeapon)
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }
    }

    //have to edit this eventually to include more models
    // private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    // {
    //     switch (model)
    //     {
    //         case Weapon.WeaponModel.Pistol:
    //             return Instantiate(Resources.Load<GameObject>("Pistol1911_Weapon")).GetComponent<SpriteRenderer>().sprite;
                
    //         case Weapon.WeaponModel.Rifle:
    //             return Instantiate(Resources.Load<GameObject>("M16_Weapon")).GetComponent<SpriteRenderer>().sprite;

    //         default:
    //             return null;
    //     }
    // }

    // private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    // {
    //     switch (model)
    //     {
    //         case Weapon.WeaponModel.Pistol:
    //             return Instantiate(Resources.Load<GameObject>("Pistol_Ammo")).GetComponent<SpriteRenderer>().sprite;

    //         case Weapon.WeaponModel.Rifle:
    //             return Instantiate(Resources.Load<GameObject>("Rifle_Ammo")).GetComponent <SpriteRenderer>().sprite;

    //         default:
    //             return null;
    //     }
    // }

    private GameObject GetUnActiveWeaponSlot()
    {
        // foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        // {
        //     if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
        //     {
        //         return weaponSlot;
        //     }
        // }
        if(WeaponManager.Instance.activeWeaponSlot == WeaponManager.Instance.weaponSlot1){return WeaponManager.Instance.weaponSlot1;}
        else{return WeaponManager.Instance.weaponSlot2;}
    }

}
