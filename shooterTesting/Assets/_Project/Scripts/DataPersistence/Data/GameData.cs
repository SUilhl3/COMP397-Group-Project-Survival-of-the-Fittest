using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public Vector3 location;
    public int roundNumber;
    public int money;
    public int ammo;
    public int ammoMax;
    public int ammoTwo;
    public int ammoMaxTwo;
    public bool hasJug;
    public bool hasSpeedCola;
    public bool hasDoubleTap;
    public bool hasQuickRevive;
    public bool hasDeadshot;
    public GameObject firstGun;
    public GameObject secondGun;
    public int lethalCount;
    public int tacticalCount;


    public GameData()
    {
        location = Vector3.zero;
        roundNumber = 1;
        money = 0;
        ammo = 7;
        ammoMax = 28;
        ammoTwo = 0;
        ammoMaxTwo = 0;
        hasJug = false;
        hasSpeedCola = false;
        hasDoubleTap = false;
        hasQuickRevive = false;
        hasDeadshot = false;
        firstGun = null;
        secondGun = null;
        lethalCount = 0;
        tacticalCount = 0;
    }
}
