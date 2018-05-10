using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour {

    public float lookRadius = 11f;
    public float stoppingDistance = 1f;
    public float fireRate = 1.8f;
    public float speed = 2f;
    public float health = 60;
    public float damage = 7;
    public AudioClip moanSound;
    public AudioClip deathSound;
    

    string state;
    float maxDist = 9f;
    float timer = 0f;
    bool isDead = false;
    bool isMoaning = false;
    NavMeshAgent agent;
    GameObject player;
    AudioSource[] audio;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(WanderDestination());
        player = GameObject.FindGameObjectWithTag("Player");
        state = "wander";

        gameObject.AddComponent(typeof(AudioSource));
        gameObject.AddComponent(typeof(AudioSource));
        audio = GetComponents<AudioSource>();
        audio[0].clip = moanSound;
        audio[0].volume = .2f;
        audio[0].playOnAwake = false;
        audio[1].clip = deathSound;
        audio[1].volume = .2f;
        audio[1].playOnAwake = false;
        //audio.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
            state = "die";
        //timer instead of running coroutines
        timer += Time.deltaTime;

        if (!isMoaning)
            StartCoroutine(Moan());

        //calc distance from enemy to player
        float dist = Vector3.Distance(transform.position, player.transform.position);

        switch (state)
        {
            case "wander":
                agent.isStopped = false;
                Debug.Log("Zombie FSM - WANDER");
                if (timer >= 3f && !isDead)
                {
                    agent.SetDestination(WanderDestination());
                    timer = 0f;
                }
                if (dist <= lookRadius && !isDead)
                    state = "pursue";
                break;
            case "pursue":
                Debug.Log("Zombie FSM - PURSUE");
                agent.isStopped = false;
                LookAtPlayer();
                agent.SetDestination(player.transform.position);
                if (dist <= stoppingDistance && !isDead)
                    state = "attack";
                break;
            case "attack":
                Debug.Log("Zombie FSM - ATTACK");
                agent.isStopped = true;
                if (timer >= fireRate)
                {
                    Attack();
                    timer = 0f;
                }
                if (dist >= stoppingDistance && dist <= lookRadius && !isDead)
                    state = "pursue";
                break;
            case "die":
                if (!isDead) { 
                    PlayerPrefs.SetInt("enemyCount", PlayerPrefs.GetInt("enemyCount") - 1);
                    PlayerPrefs.SetInt("battlePoints", PlayerPrefs.GetInt("battlePoints") + 1);
                    isDead = true;
                    audio[0].Stop();
                    audio[1].Play();
                    transform.GetChild(0).gameObject.SetActive(false);
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

    void LookAtPlayer()
    {
        if (!isDead)
        {
            Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
        }
    }
    
    void Attack()
    {
        Debug.Log("Zombie FSM - ATTACK - SLASH");
        StartCoroutine(AttackAnim());
        PlayerPrefs.SetInt("playerHealth", PlayerPrefs.GetInt("playerHealth") - (int)damage);
        player.GetComponent<PlayerController>().health   -= damage;
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

    IEnumerator Moan()
    {
        isMoaning = true;
        audio[0].Play();
        yield return new WaitForSeconds(Random.Range(moanSound.length, moanSound.length + 2f));
        isMoaning = false;
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
