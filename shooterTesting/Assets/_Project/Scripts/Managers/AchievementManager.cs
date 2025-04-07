using UnityEngine;

public class AchievementManager : MonoBehaviour, IDataPersistence
{

    public  bool hasCollectibleOne = false;
    public  bool hasCollectibleTwo = false;
    public  bool hasCollectibleThree = false;
    public  bool hasCollectibleFour = false;
    public  bool hasCollectibleFive = false;

    public  bool firstCollectible = false;
    public  bool everyCollectible = false;

    public static AchievementManager Instance { get; private set; }
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasCollectibleOne || hasCollectibleTwo || hasCollectibleThree || hasCollectibleFour || hasCollectibleFive)
        {
            firstCollectible = true;
        }
        if (hasCollectibleOne && hasCollectibleTwo && hasCollectibleThree && hasCollectibleFour && hasCollectibleFive)
        {
            everyCollectible = true;
        }
    }

    public void GetCollectible(string name)
    {
        switch (name)
        {
            case "CollectibleOne":
                hasCollectibleOne = true;
                break;
            case "CollectibleTwo":
                hasCollectibleTwo = true;
                break;
            case "CollectibleThree":
                hasCollectibleThree = true;
                break;
            case "CollectibleFour":
                hasCollectibleFour = true;
                break;
            case "CollectibleFive":
                hasCollectibleFive = true;
                break;
            default:
                hasCollectibleOne = false;
                hasCollectibleTwo =false;
                hasCollectibleThree =false;
                hasCollectibleFour =false;
                hasCollectibleFive =false;
                break;
        }
    }

    public void LoadData(GameData data)
    {
        this.firstCollectible = data.achievementOne;
        this.everyCollectible = data.achievementTwo;
    }

    public void SaveData(GameData data)
    {
        data.achievementOne = this.firstCollectible;
        data.achievementTwo = this.everyCollectible;
    }
}
