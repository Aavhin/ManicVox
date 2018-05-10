using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyCamera : MonoBehaviour {

    public float beatTimer = 2f;
    public float speed = 10f;

    float timer = 0f;
    Vector3 startPos;
    Quaternion startRot;
    Vector3 newDest;
    bool isStart = false;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
        startRot = transform.rotation;
        newDest = startPos;
        newDest.z =  newDest.z + Random.Range(10f,70f);
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, newDest, speed * Time.deltaTime);
        if (timer > beatTimer && !isStart)
        {
            newDest = startPos;
            timer = 0f;
            isStart = true;
        }
        if (timer > beatTimer && isStart)
        {
            newDest = startPos;
            newDest.z = startPos.z + Random.Range(10f, 70f);
            timer = 0f;
            isStart = false;
        }
	}
}
