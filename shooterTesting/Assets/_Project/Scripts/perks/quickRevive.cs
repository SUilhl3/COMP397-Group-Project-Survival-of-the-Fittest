using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using Platformer397;
using UnityEngine.UI;
using System.Collections.Generic;

public class quickRevive : MonoBehaviour
{
    [SerializeField] private int price = 1500;
    [SerializeField] private float reviveTime = 10000f; //time to revive once downed, currrently 10 seconds
    [SerializeField] private float gracePeriod = 3000; //time before the zombies are resumed 
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject downedUi;
    
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public async void downed()
    {
        //do downed logic
        player.moveSpeed = 50f; //make them slow
        //have the enemies be added to a list in game manager and have them stop moving for a bit

        downedUi.SetActive(true);
        EnemyManager.Instance.stopAllEnemies();

        await Task.Delay((int)(reviveTime));
        player.moveSpeed = player.originalSpeed;
        downedUi.SetActive(false);
        player.loseAllPerks();
        player.playerHealth = 100;
        await Task.Delay((int)(gracePeriod));
        EnemyManager.Instance.continueAllEnemies();
    }

    public int getCost()
    {
        return price;
    }
    
}
