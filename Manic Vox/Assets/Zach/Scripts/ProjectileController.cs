using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    public float speed = 10f;
    public int damage = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0, 0, speed * Time.deltaTime);
        StartCoroutine(Wait());
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);           
        }
        
        


    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
