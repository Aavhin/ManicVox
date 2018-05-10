using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideButtons : MonoBehaviour {


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > 10)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
	}
}
