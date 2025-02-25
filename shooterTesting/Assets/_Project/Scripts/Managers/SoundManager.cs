using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource shootingChannel;

    public AudioClip RifleShootSound;
    public AudioClip PistolShootSound;

    public AudioSource reloadingRifleSound;
    public AudioSource reloadingSoundPistol;

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
                reloadingSoundPistol.Play();
                break;
            case WeaponModel.Rifle:
                reloadingRifleSound.Play();
                break;
        }
    }

}
