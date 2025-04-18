using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Endandovermenu : MonoBehaviour
{
    public AudioMixer soundMixer;
    public void GoToMainMenu()
    {
        //SceneManager.LoadScene("Main Menu");
        Debug.Log("Entered main menu screen!");
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


}
