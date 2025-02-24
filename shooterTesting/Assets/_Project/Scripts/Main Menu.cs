using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public AudioMixer bgmMixer;

    //private void Start()
    //{
    //    if (!DataPersistenceManager.Instance.HasGameData())
    //    {
    //        continueGameButton.interactable = false;
    //    }
    //}

    public void OnNewGameClicked()
    {
        //DataPersistenceManager.Instance.NewGame();
        //SceneManager.LoadSceneAsync("Main game");
        //DisableMenuButtons();
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
    //private void DisableMenuButtons()
    //{
    //    newGameButton.interactable = false;
    //    continueGameButton.interactable = false;
    //}
}
