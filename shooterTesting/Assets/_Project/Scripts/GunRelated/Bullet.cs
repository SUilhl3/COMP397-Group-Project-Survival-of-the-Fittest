using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using Platformer397;
using System;
public class Bullet : MonoBehaviour
{
    public int damage;
    public float headShotMultiplier;
    private PlayerController player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Target"))
        {
            Target target = objectWeHit.gameObject.GetComponent<Target>();
            target.takeDamage(damage);
            // print("hit " + objectWeHit.gameObject.name + " !");

            CreateBulletImpactEffect(objectWeHit);

            Destroy(gameObject);
        }

        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            // print("hit a wall");

            CreateBulletImpactEffect(objectWeHit);

            Destroy(gameObject);
        }

        if(objectWeHit.gameObject.CompareTag("enemy"))
        {
            bool head = false;
            if(objectWeHit.gameObject.name == "head"){
                head = true;
                basicEnemy enemy = objectWeHit.gameObject.transform.parent.GetComponent<basicEnemy>();
                enemy.takeDamage((int)Math.Floor(damage*headShotMultiplier));
                Debug.Log("Headshot");
            }
            else if(head == false){
                basicEnemy enemy = objectWeHit.gameObject.GetComponent<basicEnemy>();
                enemy.takeDamage(damage);
                Debug.Log("body shot");
                }
            player.addMoney(10);
        }
    }

    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }

}
