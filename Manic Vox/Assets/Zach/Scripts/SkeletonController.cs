using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour {

    public float lookRadius = 13f;
    public float stoppingDistance = 7f;
    public float fireRate = 2f;
    public float speed = 2f;
    public GameObject arrowPrefab;
    public float health = 50;
    public AudioClip shootSound;
    public AudioClip deathSound;
    public AudioClip boneSound;

    string state;
    float maxDist = 5f;
    float timer = 0f;
    bool isDead = false;
    bool isBoneSound = false;
    NavMeshAgent agent;
    GameObject player;
    GameObject arrow;
    Transform skeleton;
    Transform firePoint;
    AudioSource[] audio;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(WanderDestination());
        player = GameObject.FindGameObjectWithTag("Player");
        state = "wander";
        skeleton = transform.GetChild(0);
        firePoint = transform.GetChild(1);

        gameObject.AddComponent(typeof(AudioSource));
        gameObject.AddComponent(typeof(AudioSource));
        gameObject.AddComponent(typeof(AudioSource));
        audio = GetComponents<AudioSource>();
        audio[0].playOnAwake = false;
        audio[1].playOnAwake = false;
        audio[2].playOnAwake = false;
        audio[0].clip = shootSound;
        audio[0].volume = .2f;
        audio[0].playOnAwake = false;
        audio[1].clip = deathSound;
        audio[1].volume = .2f;
        audio[1].playOnAwake = false;
        audio[2].clip = boneSound;
        audio[2].volume = .2f;
        audio[2].playOnAwake = false;
        //audio.rolloffFactor = 0;
        //audio.loop = true;
        //audio.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
            state = "die";
        //timer instead of running coroutines
        timer += Time.deltaTime;

        if (!isBoneSound)
            StartCoroutine(Bones());

        //calc distance from enemy to player
        float dist = Vector3.Distance(transform.position,player.transform.position);
		
        

        //FSM Controller
        switch (state)
        {
            case "wander":
                agent.isStopped = false;
                agent.updateRotation = true;
                Debug.Log("Skeleton FSM - WANDER");
                if (timer >= 3f && !isDead)
                {
                    agent.SetDestination(WanderDestination());
                    timer = 0f;
                }
                //should I pursue player?
                if (dist <= lookRadius && !isDead)
                {
                    state = "pursue";
                }
                break;
            case "pursue":
                agent.isStopped = false;
                agent.updateRotation = false;
                Debug.Log("Skeleton FSM - PURSUE");
                LookAtPlayer();
                Fire();
                agent.SetDestination(player.transform.position);
                if (dist <= stoppingDistance && !isDead)
                    state = "attack";
                break;
            case "attack":
                Debug.Log("Skeleton FSM - ATTACK");
                agent.isStopped = true;
                agent.updateRotation = false;
                Fire();
                LookAtPlayer();
                if (dist >= stoppingDistance && dist <= lookRadius && !isDead)
                    state = "pursue";
                if (dist <= stoppingDistance / 2)
                    state = "flee";
                break;
            case "flee":
                agent.isStopped = false;
                agent.updateRotation = false;
                //move the enemy away from player
                Debug.Log("Skeleton FSM - FLEE");
                Transform start = transform;
                //transform.rotation = Quaternion.LookRotation(transform.position - player.transform.position);
                Vector3 fleeTo = transform.position - transform.forward * 3f;
                agent.SetDestination(fleeTo);
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
                    transform.GetChild(0).gameObject.SetActive(false);
                    audio[0].Stop();
                    audio[2].Stop();
                    audio[1].Play();
                    Destroy(gameObject, deathSound.length);

                }
                break;
        }
	}

    Vector3 WanderDestination()
    {
        Vector3 randomDir = Random.insideUnitSphere * maxDist;
        randomDir += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDir, out navHit, maxDist, -1);
        return navHit.position;
    }

    void Fire()
    {
        if (timer >= fireRate)
        {
            Debug.Log("Skeleton FSM - FIRE");
            RaycastHit hit;
            Debug.DrawRay(firePoint.position, firePoint.forward);
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit) && !isDead)
            {
                Debug.Log("Skeleton FSM - CAST");
                if (hit.collider.gameObject.tag == "Player")
                {
                    audio[0].Play();
                    Debug.Log("Skeleton FSM - ARROW");
                    arrow = Instantiate(arrowPrefab) as GameObject;
                    arrow.transform.position = firePoint.position;
                    arrow.transform.rotation = firePoint.rotation;
                }
            }
            timer = 0f;
        }
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

    IEnumerator Bones()
    {
        isBoneSound = true;
        audio[2].Play();
        yield return new WaitForSeconds(Random.Range(boneSound.length, boneSound.length + 2f));
        isBoneSound = false;
    }

}
