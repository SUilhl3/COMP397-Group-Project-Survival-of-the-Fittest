using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenuController : MonoBehaviour
{

    public static PauseMenuController Instance { get; private set; }
    private PauseMenu menu;
    public GameObject pause;

    private void Awake()
    {
        if(Instance != null && Instance != this)
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
        pause.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        menu = GetComponent<PauseMenu>();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.SetActive(true);
            Pause();
        }
    }

    [Header("Pause Menu Buttons")]
    [SerializeField] private GameObject firstSelected;

    [SerializeField] private GameObject hideUI;
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
    public AudioMixer bgmMixer;


    // Update is called once per frame

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
        hideUI.SetActive(true);
    }

    public void Pause()
    {
        StartCoroutine(SelectFirstChoice());
        Time.timeScale = 0f;
        GameIsPause = true;
        hideUI.SetActive(false);
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
