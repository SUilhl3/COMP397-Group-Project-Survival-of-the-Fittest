using System;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Throwable : MonoBehaviour
{

    [SerializeField] float delay = 3.0f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;

    float countdown;

    bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType
    {
        None,
        Grenade,
        Smoke_Grenade

    }

    public ThrowableType throwableType;

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
                // Apply blindness to enemies
            }

            //Also apply damage over here
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

            //Also apply damage over here
        }
    }
}
