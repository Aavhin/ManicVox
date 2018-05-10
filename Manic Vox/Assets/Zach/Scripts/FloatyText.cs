using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyText : MonoBehaviour {

    public float beatTimer = .9f;
    public float speed = 1f;

    Quaternion startRot;
    Quaternion newDest;
    Vector3 startScale;
    Vector3 newScale;
    [SerializeField]
    bool isStart = true;
    float timer;

	// Use this for initialization
	void Start () {
        startRot = transform.rotation;
        newDest = startRot;
        newDest.z = startRot.z + Random.Range(-.2f, .2f);

        startScale = transform.localScale;
        newScale = transform.localScale * Random.Range(0.8f, 1.3f);
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        //transform.Rotate(newDest);
        //transform.Translate(newDest1);
        transform.rotation = Quaternion.Slerp(transform.rotation, newDest, Time.deltaTime * speed);
        transform.localScale = Vector3.Slerp(transform.localScale,newScale,Time.deltaTime * speed);
        if (timer > beatTimer && !isStart)
        {
            newDest = startRot;
            newScale = startScale;
            //StartCoroutine(Rotate(newDest));
            timer = 0f;
            isStart = true;
        }
        if (timer > beatTimer && isStart)
        {
            newDest = startRot;
            newDest.z = startRot.z + Random.Range(-.2f, .2f);
            newScale = transform.localScale * Random.Range(0.8f, 1.3f);
            //StartCoroutine(Rotate(newDest));
            timer = 0f;
            isStart = false;
        }
    }
}
