using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Endandovermenu : MonoBehaviour
{
    public AudioMixer bgmMixer;
    public void GoToMainMenu()
    {
        //SceneManager.LoadScene("Main Menu");
        Debug.Log("Entered main menu screen!");
    }
    public void ExitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        bgmMixer.SetFloat("volume",volume);
    }
}
