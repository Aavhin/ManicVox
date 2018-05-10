using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //loads level
    public void LoadLevel(string level)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel(level);
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
