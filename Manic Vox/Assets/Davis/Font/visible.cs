using System.Collections;
using UnityEngine;

public class visible : MonoBehaviour {
    float sec;
	// Use this for initialization
	void Start () {
        sec = 0;
	}
	
	// Update is called once per frame
	void Update () {
        sec = sec + Time.deltaTime;
        if (sec < 0)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
