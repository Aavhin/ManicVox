
using UnityEngine;

public class WeaponSwap : MonoBehaviour {

    public int CurrentWeapon = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        int previousWep = CurrentWeapon;

		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (CurrentWeapon >= transform.childCount - 1)
                CurrentWeapon = 0;
            else
                CurrentWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (CurrentWeapon <= 0)
                CurrentWeapon = transform.childCount - 1;
            else
                CurrentWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)){
            CurrentWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentWeapon = 2;
        }


        if (previousWep != CurrentWeapon)
        {
            WepSelect();
        }
    }

    void WepSelect()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == CurrentWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
