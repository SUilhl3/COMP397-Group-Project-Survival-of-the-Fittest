using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public AudioMixer soundMixer;
    public Button newGameButton;
    public Button continueGameButton;

    [SerializeField]
    private List<GameObject> achievements = new List<GameObject>();

    private void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
        if(achievements.Count > 0)
        {
            AchievementsAppear();
        }


    }

    public void ToTutorial()
    {
        SceneManager.LoadSceneAsync("Map 1");
        DisableMenuButtons();
    }
    public void OnNewGameClicked()
    {
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync("Map");
        DisableMenuButtons();
    }
    public void OnContinueGameClicked()
    {
        //SceneManager.LoadSceneAsync(SceneManagement.GetInstance().sceneToLoad);
        Debug.Log("Game loaded the last known level saved...");
    }


    public void ExitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void SetMasterVolume(float level)
    {
        //soundMixer.SetFloat("MasterVol", masterVol);
        soundMixer.SetFloat("MasterVol", Mathf.Log10(level) * 20f);
    }

    public void SetVolume(float level)
    {
        //soundMixer.SetFloat("BGMVol", volume);
        soundMixer.SetFloat("BGMVol", Mathf.Log10(level) * 20f);
    }

    public void SetSFXVol(float level)
    {
        //soundMixer.SetFloat("SFXVol", sFXVol);
        soundMixer.SetFloat("SFXVol", Mathf.Log10(level) * 20f);
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    public void AchievementsAppear()
    {
        if (AchievementManager.Instance.firstCollectible)
        {
            achievements[0].gameObject.SetActive(true);
        }
        else { achievements[0].SetActive(false); }
        if (AchievementManager.Instance.everyCollectible)
        {
            achievements[1].SetActive(true);
        }
        else { achievements[1].SetActive(false); }
    }
}
