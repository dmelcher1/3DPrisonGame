using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu, ui;

    private bool paused;

    private void Start()
    {
        paused = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("pause"))
        {
            if (paused)
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
        pauseMenu.SetActive(false);
        ui.SetActive(true);
        Time.timeScale = 1f;
        paused = false;
    }

    public void Pause()
    {
        ui.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void ResumeGame()
    {
        Resume();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
