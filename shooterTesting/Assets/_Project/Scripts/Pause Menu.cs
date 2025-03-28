using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu Buttons")]
    [SerializeField] private GameObject firstSelected;

    [SerializeField] private GameObject hideUI;
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
    public AudioMixer bgmMixer;


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameIsPause)
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

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
        hideUI.SetActive(false);
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
