using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSaving : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;

    private void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked_Save()
    {
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync("GrassyLevel");
        DisableMenuButtons();
    }

    public void OnContinueGameClicked_Save()
    {
        SceneManager.LoadSceneAsync("Placeholder");

        DisableMenuButtons();
    }

    public void ExitApplication()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }
}
