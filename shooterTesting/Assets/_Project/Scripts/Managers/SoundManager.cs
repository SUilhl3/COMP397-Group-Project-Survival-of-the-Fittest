using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    //working
    //shooting channel
    public AudioSource shootingChannel;
    public AudioClip RifleShootSound;
    public AudioClip PistolShootSound;

    //reloading channel 
    public AudioSource reloadingChannel;
    public AudioClip pistolReloadSound;
    public AudioClip rifleReloadSound;

    public AudioSource throwablesChannel;
    public AudioClip GrenadeSound;

    //not working
    public AudioSource emptyPistolMagazine;


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

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
                shootingChannel.PlayOneShot(PistolShootSound);
                break;
            case WeaponModel.Rifle:
                shootingChannel.PlayOneShot(RifleShootSound);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
            case WeaponModel.Python:
            case WeaponModel.Buzzsub:
                reloadingChannel.PlayOneShot(pistolReloadSound);
                // Debug.Log("Pistol reload sound");
                break;
            case WeaponModel.Rifle:
            case WeaponModel.Mkb16:
            case WeaponModel.M82:
            case WeaponModel.SVDmr:
            case WeaponModel.AUGE:
            case WeaponModel.M249:
            case WeaponModel.Assault57:
            case WeaponModel.SCE5:
                reloadingChannel.PlayOneShot(rifleReloadSound);
                // Debug.Log("Rifle reload sound");
                break;
        }
    }

}
