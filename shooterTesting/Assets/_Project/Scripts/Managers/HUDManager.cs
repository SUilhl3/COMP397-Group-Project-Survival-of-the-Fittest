using System;
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

    [Header("Health")]
    [SerializeField]
    private List<GameObject> damage = new List<GameObject>();

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;
    public Sprite greySlot;


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

        if (WeaponManager.Instance.lethalsCount <= 0)
        {
            lethalUI.sprite = greySlot;
        }
        if (WeaponManager.Instance.tacticalsCount <= 0)
        {
            tacticalUI.sprite = greySlot;
        }
    }

    //have to edit this eventually to include more models
    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol:
                return Resources.Load<GameObject>("Pistol1911_Weapon").GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.Rifle:
                return Resources.Load<GameObject>("M16_Weapon").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.Rifle:
                return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

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

    internal void UpdateThrowables()
    {
        lethalAmountUI.text = $"{WeaponManager.Instance.lethalsCount}";
        tacticalAmountUI.text = $"{WeaponManager.Instance.tacticalsCount}";

        switch (WeaponManager.Instance.equippedLethalType)
        {
            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }

        switch (WeaponManager.Instance.equippedTacticalType)
        {
            case Throwable.ThrowableType.Smoke_Grenade:
                tacticalUI.sprite = Resources.Load<GameObject>("Smoke_Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }

    public void HealthShower(float health, int maxHealth)
    {
        if(maxHealth == 100)
        {
            switch (health)
            {
                case var expression when health < 100 && health > 75:
                        damage[0].SetActive(true);
                    break;
                case var expression when health < 75 && health > 50:
                        damage[0].SetActive(true);
                        damage[1].SetActive(true);
                    break;
                case var expression when health < 50 && health > 25:
                        damage[0].SetActive(true);
                        damage[1].SetActive(true);
                        damage[2].SetActive(true);
                    break;
                case var expression when health < 25 && health > 0:
                        damage[0].SetActive(true);
                        damage[1].SetActive(true);
                        damage[2].SetActive(true);
                        damage[3].SetActive(true);
                    break;
                default:
                    foreach(GameObject obj in damage)
                    {
                        obj.SetActive(false);
                    }
                    break;
            }
        }
        else if(maxHealth > 100)
        {
            switch (health)
            {
                case var expression when health < 200 && health > 175:
                        damage[0].SetActive(true);
                    break;
                case var expression when health < 175 && health > 150:
                        damage[0].SetActive(true);
                        damage[1].SetActive(true);
                    break;
                case var expression when health < 150 && health > 125:
                        damage[0].SetActive(true);
                        damage[1].SetActive(true);
                        damage[2].SetActive(true);
                    break;
                case var expression when health < 125 && health > 100:
                        damage[0].SetActive(true);
                        damage[1].SetActive(true);
                        damage[2].SetActive(true);
                        damage[3].SetActive(true);
                    break;
                case var expression when health < 100 && health > 75:
                        damage[0].SetActive(true);
                        damage[1].SetActive(true);
                        damage[2].SetActive(true);
                        damage[3].SetActive(true);
                        damage[4].SetActive(true);
                    break;
                case var expression when health < 75 && health > 50:
                        damage[0].SetActive(true);
                        damage[1].SetActive(true);
                        damage[2].SetActive(true);
                        damage[3].SetActive(true);
                        damage[4].SetActive(true);
                        damage[5].SetActive(true);
                    break;
                case var expression when health < 50 && health > 25:
                        damage[0].SetActive(true);
                        damage[1].SetActive(true);
                        damage[2].SetActive(true);
                        damage[3].SetActive(true);
                        damage[4].SetActive(true);
                        damage[5].SetActive(true);
                        damage[6].SetActive(true);
                    break;
                case var expression when health < 25 && health > 0:
                        damage[0].SetActive(true);
                        damage[1].SetActive(true);
                        damage[2].SetActive(true);
                        damage[3].SetActive(true);
                        damage[4].SetActive(true);
                        damage[5].SetActive(true);
                        damage[6].SetActive(true);
                        damage[7].SetActive(true);
                    break;
                default:
                    foreach (GameObject obj in damage)
                    {
                        obj.SetActive(false);
                    }
                    break;
            }
        }
    }
}
