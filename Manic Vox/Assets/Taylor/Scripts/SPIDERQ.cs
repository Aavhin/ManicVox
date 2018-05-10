using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SPIDERQ : MonoBehaviour {

    public float lookRadius = 200f;
    public float stoppingDistance = 30f;
    public bool skelly = false;
    public float rangedFireRate = .5f;
    public float speed = 2f;
    public float health = 60;
    GameObject arrow;
    public GameObject arrowPrefab;
    public float damage = 15;
    public Transform skeleton;
    string state;
    float maxDist = 9f;
    float timer = 0f;
    bool isDead = false;
    NavMeshAgent agent;
    GameObject player;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(WanderDestination());
        player = GameObject.FindGameObjectWithTag("Player");
        state = "wander";
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            state = "die";
        //timer instead of running coroutines
        timer += Time.deltaTime;

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
                    state = "rangedAttack";
                break;
            
            case "rangedAttack":
                Debug.Log("Skeleton FSM - ATTACK");
                agent.isStopped = true;
                agent.updateRotation = false;
                Fire();
                LookAtPlayer();
                if (dist >= stoppingDistance && dist <= lookRadius && !isDead)
                    state = "pursue";
                break;
            case "die":
                Destroy(gameObject);
                if (skelly == true)
                {
                    SceneManager.LoadScene("YouWin");
                }
                else {SceneManager.LoadScene("MapGenTiles");
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
        player.GetComponent<PlayerController>().health -= damage;
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

    void Fire()
    {
        if (timer >= rangedFireRate)
        {
            Debug.Log("Skeleton FSM - FIRE");
            RaycastHit hit;
            Debug.DrawRay(skeleton.position + Vector3.up, skeleton.forward);
            if (Physics.Raycast(skeleton.position + Vector3.up, skeleton.forward, out hit) && !isDead)
            {
                Debug.Log("Skeleton FSM - CAST");
                if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("Skeleton FSM - ARROW");
                    arrow = Instantiate(arrowPrefab) as GameObject;
                    arrow.transform.position = skeleton.position + Vector3.up;
                    arrow.transform.rotation = skeleton.rotation;
                }
            }
            timer = 0f;
        }
    }

    public void takeDamage(float dmg)
    {
        health -= dmg;
    }
}

