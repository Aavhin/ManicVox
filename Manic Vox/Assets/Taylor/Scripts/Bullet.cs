using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour {

    public float speed = 10.0f;
    public int damage = 5;
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }



    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject)
        {
            Destroy(gameObject);
        }
    }

   
}