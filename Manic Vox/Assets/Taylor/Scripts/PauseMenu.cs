using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool gamePaused = false;
    public GameObject PauseUI;
    public GameObject Camera;
    // Update is called once per frame
    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("Camera");
    }
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }else
            {
                Pause();
            }
        }
	}

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        Camera.GetComponent<CameraController>().isPaused = false;

    }

    void Pause()
    {
        Camera.GetComponent<CameraController>().isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoSpider()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SpiderQueen");
    }

    public void GoSkele()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SkullDaddy");
    }
}
