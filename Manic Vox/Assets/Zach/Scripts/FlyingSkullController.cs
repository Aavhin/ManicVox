using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSkullController : MonoBehaviour {

    public float maxDist = 5f;
    public float lookRadius = 16f;
    public float stoppingDistance = 10f;
    public float fireRate = 1.3f;
    public float speed = 3f;
    public GameObject fireBallPrefab;
    public float health = 20;

    float timer = 0f;
    bool isDead = false;
    string state = "wander";
    Vector3 newDest;
    GameObject player;
    GameObject fireBall;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        state = "wander";
        newDest = WanderDestination();
	}
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
            state = "die";
        timer += Time.deltaTime;

        float dist = Vector3.Distance(transform.position, player.transform.position);

        switch (state)
        {
            case "wander":
                Debug.Log("Skull FSM - WANDER");
                Quaternion rotation = Quaternion.LookRotation(newDest - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
                if (timer > 1f)
                {
                    newDest = WanderDestination();
                    timer = 0f;
                }
                if (dist <= lookRadius && !isDead)
                {
                    state = "pursue";
                }
                transform.position = Vector3.Lerp(transform.position, newDest, speed * Time.deltaTime);
                break;
            case "pursue":
                Debug.Log("Skull FSM - PURSUE");
                LookAtPlayer();
                Fire();
                transform.position = Vector3.Lerp(transform.position, player.transform.position, (speed / 2) * Time.deltaTime);
                if (dist <= stoppingDistance && !isDead)
                    state = "attack";
                break;
            case "attack":
                Debug.Log("Skull FSM - ATTACK");
                LookAtPlayer();
                Fire();
                if (dist >= stoppingDistance && dist <= lookRadius && !isDead)
                    state = "pursue";
                if (dist <= stoppingDistance / 2)
                    state = "flee";
                break;
            case "flee":
                //move the enemy away from player
                Debug.Log("Skull FSM - FLEE");
                Transform start = transform;
                transform.rotation = Quaternion.LookRotation(transform.position - player.transform.position);
                Vector3 fleeTo = transform.position + transform.forward * 3f;
                transform.position = Vector3.Lerp(transform.position, fleeTo, speed * Time.deltaTime);
                Fire();
                if (dist >= stoppingDistance && !isDead)
                    state = "pursue";
                break;
            case "die":
                if (!isDead)
                {
                    PlayerPrefs.SetInt("enemyCount", PlayerPrefs.GetInt("enemyCount") - 1);
                    PlayerPrefs.SetInt("battlePoints", PlayerPrefs.GetInt("battlePoints") + 1);
                    isDead = true;
                    Destroy(gameObject);
                }
                     
                break;
        }
        
        
	}

    void Fire()
    {
        if (timer >= fireRate)
        {
            Debug.Log("Skull FSM - FIRE");
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward);
            if (Physics.Raycast(transform.position, transform.forward, out hit) && !isDead)
            {
                Debug.Log("Skull FSM - CAST");
                if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("Skull FSM - FIREBALL");
                    StartCoroutine(AttackAnim());
                    fireBall = Instantiate(fireBallPrefab) as GameObject;
                    fireBall.transform.position = transform.position;
                    fireBall.transform.rotation = transform.rotation;
                }
            }
            timer = 0f;
        }
    }

    Vector3 WanderDestination()
    {
        Vector3 randomDir = Random.insideUnitSphere * maxDist;
        randomDir += transform.position;
        return randomDir;
    }

    void LookAtPlayer()
    {
        if (!isDead)
        {
            Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerProj")
        {
            int dmg = other.gameObject.GetComponent<Bullet>().damage;
            health -= dmg;
            Destroy(other.gameObject);
        }
    }

    public void takeDamage(float dmg)
    {
        health -= dmg;
    }

    IEnumerator AttackAnim()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
