using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissapear : MonoBehaviour {
    public GameObject wall;
    public bool isActiveWall;
    public bool isWaiting = false;
	// Update is called once per frame
	void Update () {
        if (isWaiting)
        {
            return;
        }
		if (isActiveWall == true)
        {
            StartCoroutine(Wait());            
            return;
        }
        else
        {
            StartCoroutine(Wait());
            return;
        }
	}



    IEnumerator Wait()
    {
        isWaiting = true;
        wall.SetActive(!isActiveWall);
        yield return new WaitForSecondsRealtime(Random.Range(1,3));
        isActiveWall = !isActiveWall;
        isWaiting = false;
    }
}
