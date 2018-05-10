using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKELLDAD : MonoBehaviour {
    float health = 2000;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void takeDamage(float dmg)
    {
        health -= dmg;
    }
}

