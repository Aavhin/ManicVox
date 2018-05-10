using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public int SpawnTime;
    public GameObject monster;
    public Transform loc;
    private bool iswaiting = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (iswaiting)
        {
            return;
        }
        if(!iswaiting)
        {
            StartCoroutine(Spawn());
        }
	}

    IEnumerator Spawn()
    {
        Instantiate(monster,loc.position,monster.transform.rotation);
        iswaiting = true;
        yield return new WaitForSeconds(SpawnTime);
        iswaiting = false;
    }
}
