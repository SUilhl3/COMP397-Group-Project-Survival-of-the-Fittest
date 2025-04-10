using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public bool firstStep = false;
    public bool secondStep = false;
    public bool thirdStep = false;
    public bool fourthStep = false;

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
        
    }

    public string Tutorial()
    {
        if (firstStep && !secondStep)
        {
            return "Please shoot by pressing the shoot button you see";
        }
        if(secondStep && !thirdStep)
        {
            return "Perfect, interact with your surroundings by pressing the interact button. Try buying a perk from the machine!";
        }
        if (thirdStep)
        {
            return "You have completed the tutorial! Feel free to continue interacting";
        }
        return "Welcome to the tutorial! Please use the controls on the screen to move";
    }

}
