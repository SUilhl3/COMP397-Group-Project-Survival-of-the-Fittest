using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    [SerializeField] private int weaponCost;

    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //Spread
    public float spreadIntensity;

    // Bullet

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;
    public int gunDamage;

    public GameObject muzzleEffect;
    internal Animator animator;

    // Loading
    public float reloadTime;
    public int magazineSize, bulletsLeft, bulletReserve;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    //cooldown for audio 
    float cooldownTime = .5f;
    float lastTimeExecuted = -Mathf.Infinity;

    //I think these will be based off the guns we have 
    public enum WeaponModel
    {
        Pistol,
        Rifle,
        Assault57,
        Buzzsub,
        M249
    }

    public WeaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;

            // Empty Magazine Sound
            if (bulletsLeft == 0 && isShooting && Time.time - lastTimeExecuted >= cooldownTime)
            {
                lastTimeExecuted = Time.time;
                SoundManager.Instance.emptyPistolMagazine.Play();
            }


            if (currentShootingMode == ShootingMode.Auto)
            {
                // Holding down left mouse button
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                // Clicking left mouse button once
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false && bulletReserve > 0)
            {
                Reload();
            }
            if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            {
                //Reload();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }
        }

    }

    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("Recoil");

        //for now, we will use the rifle or pistol sounds for any new guns until we get more sounds set up, not a priority
        if(thisWeaponModel == WeaponModel.Assault57 || thisWeaponModel == WeaponModel.M249){SoundManager.Instance.PlayShootingSound(WeaponModel.Rifle);}
        else if(thisWeaponModel == WeaponModel.Buzzsub){SoundManager.Instance.PlayShootingSound(WeaponModel.Pistol);}
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;


        //Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().damage = gunDamage;

        //point the bullet to face shooting direction
        bullet.transform.forward = shootingDirection;

        //shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        // destroy the bullet after some time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        //checking if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // burst mode
        if(currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) // we already shoot once before this
        {
            burstBulletsLeft--;
            Invoke("FIreWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        animator.SetTrigger("Reload");

        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        int requiredAmount = magazineSize - bulletsLeft; //bullet reserve keeps the bullets relative to the gun rather than having a supply of general ammo
        if(bulletReserve > requiredAmount) 
        { 
            bulletsLeft = magazineSize; 
            bulletReserve -= requiredAmount;
        }
        else
        {
            bulletsLeft = bulletReserve + bulletsLeft;
            bulletReserve -= bulletReserve;   
        }

        isReloading = false;
    }

    public int getCost(){return weaponCost;}
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            // Hitting something
            targetPoint = hit.point;
        }
        else
        {
            // Shooting the air
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // Returning the shooting direction and spread
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }



}
