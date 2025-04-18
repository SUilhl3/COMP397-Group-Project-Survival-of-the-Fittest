using Platformer397;
using System;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Throwable : MonoBehaviour
{

    [SerializeField] float delay = 3.0f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;
    [SerializeField] private int damage = 10;
    [SerializeField] private int price;

    float countdown;

    bool hasExploded = false;
    public bool hasBeenThrown = false;

    private PlayerController player;

    public enum ThrowableType
    {
        None,
        Grenade,
        Smoke_Grenade

    }

    public ThrowableType throwableType;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if(countdown <= 0f && !hasExploded)
            {
                Explode();
                Destroy(gameObject);
                hasExploded = true;
            }
        }
    }

    public int getPrice()
    {
        return price;
    }

    private void Explode()
    {
        GetThrowableEffect();

        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
            case ThrowableType.Smoke_Grenade:
                SmokeGrenadeEffect();
                break;
        }
    }

    private void SmokeGrenadeEffect()
    {
        //Visual effect
        GameObject smokeEffect = GlobalReferences.Instance.smokeGrenadeEffect;
        Instantiate(smokeEffect, transform.position, transform.rotation);

        // Play Sound
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.GrenadeSound);

        //Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            EnemyNavigation en = objectInRange.GetComponent<EnemyNavigation>();
            if (en != null)
            {
                en.isBlinded = true; 
            }
        }
    }

    private void GrenadeEffect()
    {
        //Visual effect
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Play Sound
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.GrenadeSound);

        //Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach(Collider objectInRange in colliders)
        {
            basicEnemy be = objectInRange.GetComponent<basicEnemy>();
            if (be != null)
            {
                //enemies give 20 money even though it should be 10 because the enemies have 2 colliders on them and this just scans for any colliders, including the trigger
                be.takeDamage(damage);
                // player.addMoney(10);
            }


            //Also apply damage over here
        }
    }
}
