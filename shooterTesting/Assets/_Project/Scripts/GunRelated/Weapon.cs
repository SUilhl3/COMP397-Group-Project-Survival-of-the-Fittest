using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Platformer397;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    [SerializeField] private int weaponCost;
    public bool mysteryWeapon = false;
    public bool instaKill = false;
    public static int counter = 0;

    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //Spread
    public float spreadIntensity;
    public float originalSpread;

    // Bullet

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 30f;
    public int gunDamage;
    public float headShotMultiplier;
    public float originalHSM;
    public int originalGunDamage;
    public float originalShootingDelay;

    public GameObject muzzleEffect;
    internal Animator animator;

    // Loading
    public float reloadTime, originalReloadTime;
    public int magazineSize, bulletsLeft, bulletReserve, originalBulletReserve;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    //cooldown for audio 
    float cooldownTime = .5f;
    float lastTimeExecuted = -Mathf.Infinity;

    static System.Random random = new System.Random();
    public double bigDamageChance = 0.0;


    //Interaction
    [SerializeField] private InputReader input;

    private bool attackPress = false;
    private bool reloadPress = false;

    //I think these will be based off the guns we have 
    public enum WeaponModel
    {
        Pistol,
        Rifle,
        Assault57,
        Buzzsub,
        M249,
        AUGE,
        M82,
        Mkb16,
        Python,
        SVDmr,
        SCE5,
        skorp6,
        tac,
        Sturmgewehr46,
        mp5k,
        tar
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
        originalGunDamage = gunDamage;
        originalHSM = headShotMultiplier;
        originalReloadTime = reloadTime;
        originalShootingDelay = shootingDelay;  
        originalSpread = spreadIntensity;
        originalBulletReserve = bulletReserve; 
    }

    private void Start()
    {
        input.EnablePlayerActions();
    }
    private void OnEnable()
    {
        input.Attack += HandleAttack;
        input.Reload += HandleReload;
    }

    private void HandleReload(bool reload)
    {
        reloadPress = reload;
        Debug.Log("Pressed Reload");
    }

    private void HandleAttack(bool attack)
    {
        attackPress = attack;
    }

    private void OnDisable()
    {
        input.Attack -= HandleAttack;
        input.Reload -= HandleReload;
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
                isShooting = attackPress;
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                // Clicking left mouse button once
                isShooting = attackPress;
            }

            if (reloadPress && bulletsLeft < magazineSize && isReloading == false && bulletReserve > 0)
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

    public void mysteryBoxWeapon(){if(mysteryWeapon){weaponCost = 0;}}

    public bool bigDamage(double chance)
    {
        if(random.NextDouble() < chance) {return true;}
        return false;
    }

    public bool fullAmmo()
    {
        if(bulletReserve < originalBulletReserve || 
        bulletReserve == originalBulletReserve && bulletsLeft < magazineSize) {return false;}
        return true;
    }
    public void maxAmmo()
    {
        bulletsLeft = magazineSize;
        bulletReserve = originalBulletReserve;
    }

    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("Recoil");
        //for now, we will use the rifle or pistol sounds for any new guns until we get more sounds set up, not a priority
        if(thisWeaponModel == WeaponModel.Assault57 || thisWeaponModel == WeaponModel.M249 || thisWeaponModel == WeaponModel.AUGE || thisWeaponModel == WeaponModel.M82 
        || thisWeaponModel == WeaponModel.Mkb16 || thisWeaponModel == WeaponModel.SVDmr || thisWeaponModel == WeaponModel.SCE5 || thisWeaponModel == WeaponModel.Sturmgewehr46
        || thisWeaponModel == WeaponModel.tar)
        {SoundManager.Instance.PlayShootingSound(WeaponModel.Rifle);}
        else if(thisWeaponModel == WeaponModel.Buzzsub || thisWeaponModel == WeaponModel.Python || thisWeaponModel == WeaponModel.skorp6 || thisWeaponModel == WeaponModel.tac
        || thisWeaponModel == WeaponModel.mp5k)
        {SoundManager.Instance.PlayShootingSound(WeaponModel.Pistol);}
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        // Debug.DrawRay(bulletSpawn.position, shootingDirection * 100f, Color.blue, 1f);

        //Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        if(instaKill)
        {
            bullet.GetComponent<Bullet>().damage = 999999999;
        }
        else{
            bullet.GetComponent<Bullet>().damage = gunDamage;
        }
        if(bigDamage(bigDamageChance)){bullet.GetComponent<Bullet>().headShotMultiplier = headShotMultiplier*50;Debug.Log("BIG DAMAGE");}
        else{bullet.GetComponent<Bullet>().headShotMultiplier = headShotMultiplier;}

        //point the bullet to face shooting direction
        bullet.transform.forward = shootingDirection;

        //shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection.normalized * bulletVelocity, ForceMode.Impulse);
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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f,0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit, 10000f))
        {
            // Hitting something
            targetPoint = hit.point;
        }
        else
        {
            // Shooting the air
            targetPoint = ray.GetPoint(100);
            // targetPoint = new Vector3(1f, 0f,0f);
        }

        Debug.DrawRay(bulletSpawn.position, (targetPoint - bulletSpawn.position).normalized * 1000f, Color.green, 1f);
        Vector3 direction = (targetPoint - bulletSpawn.position).normalized;
        // Debug.Log(direction);

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        Vector3 spread = (Camera.main.transform.right*x) + (Camera.main.transform.up*y);
        // Returning the shooting direction and spread
        return (direction + spread).normalized;
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    public IEnumerator instaKillTimer(float delay)
    {
        counter++;
        yield return new WaitForSeconds(delay);
        if(instaKill == true && counter == 1)
        {
            instaKill = false;
            counter = 0;
            Debug.Log("InstaKill Over");
        }
        else if(instaKill == true && counter > 1)
        {
            counter--;
        }
    }


}
