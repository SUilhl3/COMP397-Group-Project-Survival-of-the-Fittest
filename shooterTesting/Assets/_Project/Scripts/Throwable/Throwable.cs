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
                hasExploded = true;
            }
        }
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
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
            if (objectInRange.gameObject.CompareTag("enemy"))
            {
                //Reaches in to navigation script and turns blindness to true for each enemy caught in radius
                EnemyNavigation enemy = objectInRange.gameObject.GetComponent<EnemyNavigation>();
                enemy.isBlinded = true;
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
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if(rb!= null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
            if (objectInRange.gameObject.CompareTag("enemy"))
            {
                //For each enemy in radius, causes damage to enemy and adds money to player
                basicEnemy enemy = objectInRange.gameObject.GetComponent<basicEnemy>();
                player.addMoney(50);
                enemy.takeDamage(damage);
            }

            //Also apply damage over here
        }
    }
}
