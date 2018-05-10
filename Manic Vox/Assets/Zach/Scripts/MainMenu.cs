using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    GameObject audioSource;
    AudioSource audio;
    public Canvas tut;
    public Canvas credits;
    public Canvas main;

    public GameObject pointsDisplay;
    int bp;

	// Use this for initialization
	void Start () {
        bp = PlayerPrefs.GetInt("battlePoints");
        Debug.Log("BATLLEPOINTS: " + bp.ToString());
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        audioSource = GameObject.FindGameObjectWithTag("Enemy");
        audio = audioSource.GetComponent<AudioSource>();
        credits.gameObject.SetActive(false);
        tut.gameObject.SetActive(false);

        //pointsDisplay = GameObject.FindGameObjectWithTag("laser");
        
        
        PlayerPrefs.DeleteAll();
    }
	
	// Update is called once per frame
	void Update () {
        pointsDisplay.GetComponent<TextMeshProUGUI>().text = "Battle Points: " + bp.ToString();
	}

    public void StartButton()
    {
        audio.Play();
        SceneManager.LoadScene("MapGenTiles");
    }

    public void ReturnButton()
    {
        audio.Play();
        SceneManager.LoadScene("Menu");
    }

    public void TutorialButton()
    {
        tut.gameObject.SetActive(true);
        main.gameObject.SetActive(false);
        audio.Play();
    }
    public void CreditsButton()
    {
        credits.gameObject.SetActive(true);
        main.gameObject.SetActive(false);
        audio.Play();
    }
    public void BackButton()
    {
        main.gameObject.SetActive(true);
        credits.gameObject.SetActive(false);
        tut.gameObject.SetActive(false);
        audio.Play();
    }

    public void QuitButton()
    {
        audio.Play();
        Application.Quit();
    }
}
