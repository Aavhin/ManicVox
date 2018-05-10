using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRC : MonoBehaviour {

    public GameObject impactFX;
    public ParticleSystem  flashFX;
    public GameObject muzzlePoint;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public Camera PlayerCam;
    public AudioClip laser;
    
    public int maxAmmo = 30;
    public int currentAmmo;
    private bool isReloading = false;
    public int reloadTime = 2;

    private float nextFire = 0f;
    AudioSource audio;

    void Start()
    {
        currentAmmo = maxAmmo;

        gameObject.AddComponent(typeof(AudioSource));
        audio = GetComponent<AudioSource>();
        audio.clip = laser;
        audio.volume = .08f;
        audio.loop = true;
        audio.Play();
    }
		
    void OnEnable()
    {
        isReloading = false;
    }
    void Update () {
        if (isReloading)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButton("Fire1") && Time.time >= nextFire )
        {
            nextFire = Time.time + 1f / fireRate;
            Shoot();
            audio.Play();
        }
        else
        {
            // audio.Stop();
        }
	}

    void Shoot()
    {
        flashFX.Play();
        currentAmmo -= 1;
        RaycastHit hit;
        if(Physics.Raycast(PlayerCam.transform.position, PlayerCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            GameObject target = hit.transform.gameObject;
            if (target.tag == "Skeleton")
            {
                SkeletonController targetConfirmed = hit.transform.GetComponent<SkeletonController>();
                targetConfirmed.takeDamage(damage);
            }
            if (target.tag == "Zombie")
            {
                ZombieController  targetConfirmed = hit.transform.GetComponent<ZombieController>();
                targetConfirmed.takeDamage(damage);
            }
            if (target.tag == "FlyingSkull")
            {
                FlyingSkullController  targetConfirmed = hit.transform.GetComponent<FlyingSkullController>();
                targetConfirmed.takeDamage(damage);
            }
            if (target.tag == "BossModel")
            {
                SPIDERQ targetConfirmed = hit.transform.GetComponent<SPIDERQ>();
                targetConfirmed.takeDamage(damage);
            }
            if (target.tag == "SKELLDAD")
            {
                SKELLDAD targetConfirmed = hit.transform.GetComponent<SKELLDAD>();
                targetConfirmed.takeDamage(damage);
            }


            GameObject impactGO = Instantiate(impactFX, hit.point, Quaternion.LookRotation(hit.normal));
           Destroy(impactGO, 2f);
            
           

        }
    }

    IEnumerator Reload()
    {
        currentAmmo = 0;
        isReloading = true;
        Debug.Log("reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
