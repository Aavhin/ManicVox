using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProj : MonoBehaviour {

    public GameObject bulletFX;
    public ParticleSystem flashFX;
    public GameObject muzzlePoint;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 5f;
    public int maxAmmo = 10;
    public int currentAmmo;
    public Camera PlayerCam;
    public AudioClip shot;

    AudioSource audio;
    public int reloadTime;
    public GameObject ammoDisplay;

    private bool isReloading = false;
    private float nextFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
        gameObject.AddComponent(typeof(AudioSource));
        audio = GetComponent<AudioSource>();
        audio.clip = shot;
        audio.volume = .18f;
        audio.playOnAwake = false;
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
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFire )
        {
            nextFire = Time.time + 1f / fireRate;
            Shoot();
            audio.Play();
        }
	}

    void Shoot()
    {
        flashFX.Play();
        currentAmmo -= 1;
        Instantiate(bulletFX, muzzlePoint.transform.position, Quaternion.LookRotation(PlayerCam.transform.forward));
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


