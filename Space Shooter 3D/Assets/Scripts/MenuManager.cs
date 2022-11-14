using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject panelConfig;

    private void Start()
    {
        panelConfig.SetActive(false);
    }

    public void OnZerar()
    {
        SoundController.sounds.click.Play();
        PlayerPrefs.DeleteAll();
    }

    public void OnQuitBtnClicked()
    {
        SoundController.sounds.click.Play();
        Application.Quit();
    }

    public void OnClose()
    {
        SoundController.sounds.click.Play();
        panelConfig.SetActive(false);
    }

    public void OnConfigure()
    {
        SoundController.sounds.click.Play();
        panelConfig.SetActive(true);
    }

    public void OnStartedBtnClicked()
    {
        SoundController.sounds.click.Play();
        SceneManager.LoadScene("Game");
    }
}
