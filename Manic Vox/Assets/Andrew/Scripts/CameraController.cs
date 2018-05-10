using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float minimumX = -60f;
    public float maximumX = 60f;
    public float minimumY = -360f;
    public float maximumY = 360f;

    public float sensetivityX = 15f;
    public float sensetivityY = 15f;
    public bool isPaused;

    public Camera cam;

    float rotationY = 0f;
    float rotationX = 0f;

    GameObject player;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        player = GameObject.FindGameObjectWithTag("Player");
        isPaused = false;
    }
	
	// Update is called once per frame
	void Update () {
        
        if(isPaused == true)
        {
            return;
        }
            rotationY += Input.GetAxis("Mouse X") * sensetivityY;
            rotationX += Input.GetAxis("Mouse Y") * sensetivityX;

            rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);

            transform.localEulerAngles = new Vector3(0, rotationY, 0);
            cam.transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0);

            cam.transform.position = player.transform.position + new Vector3(0, .5f, 0);
            player.transform.localRotation = Quaternion.AngleAxis(rotationY, player.transform.up);
        
    }
}
