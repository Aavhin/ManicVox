using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float walkSpeed;
    public float jumpHeight;
    int bossBeaten = 0;
    float walkSpeedTemp;
    float sprintSpeed;
    float crouchSpeed;
    string state = "idle";
    public float battlePoints;
    public float health;
    public float ammo;
    public float atkDmg;
    public float sprintSpeedTemp;
    Scene scene;
    GameObject Spoder;
    GameObject Skelly;
    bool triggerLock = false;
    bool speedLock = false;
    bool cultistLock = false;

    Rigidbody rb;
    Vector3 moveDirection;
    Vector3 currentPosition;
    Vector3 lastPosition;
    Vector3 crouchHeight;
    Vector3 standingHeight;
    GameObject cam;
    Vector3 crouchCamPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Scene scene = SceneManager.GetActiveScene();
    }
    // Use this for initialization
    void Start () {
        Debug.Log(PlayerPrefs.GetInt("levelCount"));
        if (!PlayerPrefs.HasKey("walkSpeed")) { PlayerPrefs.SetInt("walkSpeed", 300); }        
        walkSpeed = PlayerPrefs.GetInt("walkSpeed");

        if (!PlayerPrefs.HasKey("playerHealth")) { PlayerPrefs.SetInt("playerHealth", 100); }
        health = PlayerPrefs.GetInt("playerHealth");

        if (!PlayerPrefs.HasKey("battlePoints")) { PlayerPrefs.SetInt("battlePoints", 0); }
        battlePoints = PlayerPrefs.GetInt("battlePoints");

        if (!PlayerPrefs.HasKey("jumpHeight")) { PlayerPrefs.SetInt("jumpHeight", 20); }
        jumpHeight = PlayerPrefs.GetInt("jumpHeight");

        if(!PlayerPrefs.HasKey("levelCount"))
            PlayerPrefs.SetInt("levelCount", 1);
        standingHeight = transform.localScale;
        crouchHeight = new Vector3(transform.localScale.x, transform.localScale.y * .5f, transform.localScale.z);

        cam = GameObject.FindGameObjectWithTag("Camera");
        
        sprintSpeed = walkSpeed * 1.5f;
        crouchSpeed = walkSpeed * .5f;
        walkSpeedTemp = walkSpeed;
        sprintSpeedTemp = sprintSpeed;
         

	}
	
	// Update is called once per frame
	void Update () {
        if (scene.name == "SpiderQueen")
        {
            Spoder = GameObject.FindGameObjectWithTag("BossModel");
            if (Spoder.GetComponent<SPIDERQ>().health <= 0)
            {
                bossBeaten += 1;
                SceneManager.LoadScene("MapGenTiles");
            }
        }
        else if (scene.name == "SkullDaddy")
        {
            PlayerPrefs.SetInt("enemyCount", 1);
            Skelly = GameObject.FindGameObjectWithTag("BossModel");
            if (Skelly.GetComponent<SPIDERQ>().health <= 0)
            {
               
                SceneManager.LoadScene("YouWin");
            }
        }
        else if (PlayerPrefs.GetInt("enemyCount") == 0)
        {
            PlayerPrefs.SetInt("levelCount", PlayerPrefs.GetInt("levelCount") + 1);
            SceneManager.LoadScene("MapGenTiles");
        }
        

        battlePoints = PlayerPrefs.GetInt("battlePoints");


        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
        if (state == "walking" && !speedLock)
        {
            walkSpeed = PlayerPrefs.GetInt("walkSpeed");
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && (state =="idle" || state =="walking" || state =="sprinting"))
        {
            state = "falling";
            rb.velocity = new Vector3(0, 10 * jumpHeight * Time.deltaTime, 0);

        }
        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded() &&  state == "walking" )
        {
            walkSpeed *=1.5f;
            state = "sprinting";
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && IsGrounded() && state == "sprinting")
        {
            walkSpeed /=1.5f;
            state = "walking";
        }
        if (Input.GetKey(KeyCode.C) && (state =="walking" || state =="idle"))
        {          
            state = "crouching";
            walkSpeed *= .5f;
            transform.localScale = crouchHeight;    
        }
        if (Input.GetKeyUp(KeyCode.C) && state == "crouching")

        {
            transform.localScale = standingHeight;
            walkSpeed *= 2;
            state = "walking";
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("MapGenTiles");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            PlayerPrefs.SetInt("levelCount", PlayerPrefs.GetInt("levelCount") + 1);
            if (PlayerPrefs.GetInt("levelCount") % 3 == 0)
            {
                if (PlayerPrefs.GetInt("levelCount") == 3)
                {
                    SceneManager.LoadScene("SpiderQueen");
                }
                else
                {
                    SceneManager.LoadScene("SkullDaddy");
                }
            }
            else { 
            SceneManager.LoadScene("MapGenTiles");
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            PlayerPrefs.SetInt("playerHealth", 9999);
            health = PlayerPrefs.GetInt("playerHealth");
        }


    }
    private void FixedUpdate()
    {
        currentPosition = transform.position;
        switch (state)
        {
            case "idle":
                //Debug.Log("idle");
                transform.localScale = standingHeight;
                if (IsMoving() && IsGrounded())
                {
                    state = "walking";
                }
                break;

            case "walking":
               // Debug.Log("walking");
                
                
                
                if (lastPosition == currentPosition)
                {
                    state = "idle";
                }

                break;
            case "sprinting":
              
               // Debug.Log("sprinting");

                break;
            case "falling":
                //Debug.Log("falling");
                if (IsMoving() && IsGrounded())
                {
                    state = "walking";
                }
                if (!IsMoving() && IsGrounded())
                {
                    state = "idle";
                }
                break;
            case "crouching":
              //  Debug.Log("crouching");

               
                
                break;

        }

        lastPosition = currentPosition;
        Move();
    }
    void Move()
    {
        Vector3 yVelFix = new Vector3(0, rb.velocity.y, 0);
        rb.velocity = moveDirection * walkSpeed * Time.deltaTime;
        rb.velocity += yVelFix;
    }
    bool IsGrounded()
    {
        return (rb.velocity.y > -.001f && rb.velocity.y < .001f);
        }
    bool IsMoving()
    {
        return (lastPosition != currentPosition);
        
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "killFloor")
        {
            health = 0;
        }
        if (other.gameObject.tag == "EnemyProjectile")
        {
            int dmg = other.gameObject.GetComponent<ProjectileController>().damage;
            PlayerPrefs.SetInt("playerHealth", PlayerPrefs.GetInt("playerHealth") - dmg);
            health -= dmg;
            Destroy(other.gameObject);
        }
        
            
        
        if (other.gameObject.tag == "TightPants")
        {
            if (!speedLock)
            {
                StartCoroutine(SpeedUp());
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "CultistMask")
        {
            if (!cultistLock)
            {
                Debug.Log("cultistmask");
                StartCoroutine(IncomingDamageReduction());
                Destroy(other.gameObject);

            }
        }
        if (other.gameObject.tag == "TriggerFingers")
        {
            if (!triggerLock)
            {
                StartCoroutine(TriggerFingers());
                Destroy(other.gameObject);

            }
        }
        if (other.gameObject.tag == "teleport")
        {
            Debug.Log("scene name: " +SceneManager.GetActiveScene());

            PlayerPrefs.SetInt("playerHealth", PlayerPrefs.GetInt("playerHealth")+20);
            PlayerPrefs.SetInt("levelCount", PlayerPrefs.GetInt("levelCount") + 1);
            SceneManager.LoadScene("MapGenTiles");
        }
    }
    IEnumerator SpeedUp()
    {
        GameObject.FindGameObjectWithTag("speedUpDisplay").GetComponent<Text>().text = "Speed Up!";
        speedLock = true;
        walkSpeed = walkSpeed * 2;
        //Debug.Log(walkSpeed);
        yield return new WaitForSeconds(5);
        walkSpeed = PlayerPrefs.GetInt("walkSpeed");
        speedLock = false;
        GameObject.FindGameObjectWithTag("speedUpDisplay").GetComponent<Text>().text = "";
    }
    IEnumerator IncomingDamageReduction()
    {
        var cultistMask = GameObject.FindGameObjectWithTag("CultistMaskDisplay").GetComponent<Text>();
        cultistMask.text = "Health Up";
        
        cultistLock = true;
        PlayerPrefs.SetInt("playerHealth", PlayerPrefs.GetInt("playerHealth") +50);
        health = PlayerPrefs.GetInt("playerHealth");
        yield return new WaitForSeconds(.5f);
        cultistMask.text = "";
        yield return new WaitForSeconds(.5f);
        cultistMask.text = "Health Up";
        yield return new WaitForSeconds(.5f);
        cultistMask.text = "";
        cultistLock = false;
    }
    IEnumerator TriggerFingers()
    {
        triggerLock = true;
        var TriggerFingers = GameObject.FindGameObjectWithTag("TriggerFingersDisplay").GetComponent<Text>();
        var gun = GameObject.FindGameObjectWithTag("laser").GetComponent<GunRC>();
        var tempRate = gun.fireRate;
        gun.fireRate = gun.fireRate * 1.5f;
        TriggerFingers.text = "Fire Rate Up!";
        yield return new WaitForSeconds(5);
        TriggerFingers.text = "";
        gun.fireRate = tempRate;
        triggerLock = false;
    }

    
}
