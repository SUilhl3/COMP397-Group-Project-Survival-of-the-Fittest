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
    public AudioMixer bgmMixer;
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
        AchievementsAppear();
    }

    public void OnNewGameClicked()
    {
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync("map");
        DisableMenuButtons();
        Debug.Log("Game has started...");
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
    public void SetVolume(float volume)
    {
        bgmMixer.SetFloat("volume", volume);
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
