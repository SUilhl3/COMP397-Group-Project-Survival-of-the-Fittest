using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using Platformer397;

public class Bullet : MonoBehaviour
{
    public int damage;
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
            basicEnemy enemy = objectWeHit.gameObject.GetComponent<basicEnemy>();
            player.addMoney(10);
            enemy.takeDamage(damage);
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
