using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Platformer397;

public class PauseMenu : MonoBehaviour
{

    [Header("Pause Menu Buttons")]
    [SerializeField] private GameObject firstSelected;

    [SerializeField] private InputReader input;

    [SerializeField] private GameObject hideUI;
    public bool pauseGame = false;
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
    public AudioMixer bgmMixer;


    public void Start()
    {
        input.EnablePlayerActions();
    }

    private void OnEnable()
    {
        input.PauseGame += OnGamePause;
    }

    private void OnDisable()
    {
        input.PauseGame -= OnGamePause;
    }

    // Update is called once per frame
    private void Update()
    {
        if (pauseGame && !GameIsPause)
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }

    private void OnGamePause(bool gamePaused)
    {
        pauseGame = gamePaused;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
        hideUI.SetActive(false);
        pauseGame = false;
    }

    public void Pause()
    {
        StartCoroutine(SelectFirstChoice());
        Time.timeScale = 0f;
        GameIsPause = true;
        hideUI.SetActive(true);
        pauseMenuUI.SetActive(true);
    }
    public void SetVolume(float volume)
    {
        bgmMixer.SetFloat("volume", volume);
    }

    public void LoadMenu()
    {
        //SceneManager.LoadScene("Main Menu");
        //Time.timeScale = 1f;
        Debug.Log("Going to main menu...");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
    }
}
