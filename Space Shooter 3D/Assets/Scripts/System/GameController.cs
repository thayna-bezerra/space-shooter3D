using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Image healthBarFill;
    public float healthBarChangeTime = 0.5f;

    public GameObject pauseMenu;
    public GameObject deathMenu;

    public PlayerController playerController;

    public int scoreCurrent;
    public int scoreRecord;

    public Text scoreText;
    public Text recordText;

    public Text scoreTextPanel;
    public Text recordTextPanel;

    public BulletController bulletController;

    public static GameController gameController;

    private void Awake()
    {
        gameController = this;

    }

    public void Adicionar()
    {
        scoreCurrent++;
    }

    public void CheckScore()
    {
        if(scoreCurrent > scoreRecord)
        {
            scoreRecord = scoreCurrent;
            PlayerPrefs.SetInt("Score", scoreRecord);
        }
    }

    private void Start()
    {
        scoreCurrent = 0;
        scoreRecord = 0;

        scoreText.enabled = true;
        recordText.enabled = true;

        if (PlayerPrefs.HasKey("Score"))
        {
            scoreRecord = PlayerPrefs.GetInt("Score");
        }
    }

    public void Update()
    {
        //OnFireButtonClicked();
        scoreText.text = scoreCurrent.ToString();
        recordText.text = scoreRecord.ToString();
        //menu
        scoreTextPanel.text = scoreCurrent.ToString();
        recordTextPanel.text = scoreRecord.ToString();
    }

    public void ChangeHealthBar(int maxHeath, int currentHealth)
    {
        if (currentHealth < 0) 
                return;

        if(currentHealth == 0)
        {
            Invoke("OpenDeathMenu", healthBarChangeTime);
        }

        float healthPct = currentHealth / (float)maxHeath;

        StartCoroutine(SmoothHealthBarChange(healthPct));
    }

    private IEnumerator SmoothHealthBarChange (float newFillAmt)
    {
        float elapsed = 0f;
        float oldFillAmt = healthBarFill.fillAmount;

        while(elapsed <= healthBarChangeTime)
        {
            elapsed += Time.deltaTime;
            float currentFillAmt = Mathf.Lerp(oldFillAmt, newFillAmt, elapsed / healthBarChangeTime);

            healthBarFill.fillAmount = currentFillAmt;

            yield return null;
        }
    }

    public void OnFireButtonClicked()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) for desktop
            playerController.FireRockets();
    }

    public void OnMenuBtnClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        SoundController.sounds.click.Play();
    }

    public void OnQuitBtnClicked()
    {
        Application.Quit();
        SoundController.sounds.click.Play();
    }

    public void OnPauseBtnClicked()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        SoundController.sounds.click.Play();
    }

    
    public void OnContinueBtnClicked()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        SoundController.sounds.click.Play();
    }

    public void OnRestartBtnClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SoundController.sounds.click.Play();
    }

    public void OpenDeathMenu()
    {
        Time.timeScale = 0f;
        deathMenu.SetActive(true);
        SoundController.sounds.panelFinal.Play();

        scoreText.enabled = false;
        recordText.enabled = false;
    }
}
