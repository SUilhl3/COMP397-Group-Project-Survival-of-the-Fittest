using UnityEngine;

public class AchievementManager : MonoBehaviour
{

    private bool hasCollectibleOne = false;
    private bool hasCollectibleTwo = false;
    private bool hasCollectibleThree = false;
    private bool hasCollectibleFour = false;
    private bool hasCollectibleFive = false;

    private bool firstCollectible = false;
    private bool everyCollectible = false;

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
}
