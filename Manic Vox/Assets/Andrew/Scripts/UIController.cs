using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
    public Text healthText;
    public Text battlePointsText;
    public Text ammoText;
    public Text enemyCountText;
    public GameObject player;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject gun3;

    int enemyCount;

    //public Text ammoText;
    // Use this for initialization
    void Start () {
        

        player = GameObject.FindGameObjectWithTag("Player");
   
    }

    // Update is called once per frame
    void Update () {
        Debug.Log(enemyCount);
        enemyCount = PlayerPrefs.GetInt("enemyCount");
        if (gun1.activeSelf == true)
        {
            ammoText.text = "Ammo : " + gun1.GetComponent<GunRC>().currentAmmo;
        }
        if (gun2.activeSelf == true)
        {
            ammoText.text = "Ammo : " + gun2.GetComponent<GunProj>().currentAmmo;
        }
        if (gun3.activeSelf == true)
        {
            ammoText.text = "Ammo : " + gun3.GetComponent<GunProj>().currentAmmo;
        }

        healthText.text = "Health : " + player.GetComponent<PlayerController>().health;
        battlePointsText.text = "Battle Points : " + PlayerPrefs.GetInt("battlePoints");
        enemyCountText.text = "Enemy Count : " + enemyCount;
       
        if (player.GetComponent<PlayerController>().health <= 0)
        {
            
            SceneManager.LoadScene("EndGame");
        }
    }

}
