using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour
{
    public Transform AK47, handGun;
    public bool isAK, haveAK;

    private void Start()
    {
        isAK = false;
        FindObjectOfType<FPSController>().arms = handGun;
    }

    private void Update()
    {
        if (haveAK)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && AK47.gameObject.activeInHierarchy == false)
            {
                isAK = true;
                AK47.transform.position = handGun.transform.position;
                AK47.transform.rotation = handGun.transform.rotation;
                GetComponent<FPSController>().arms = AK47;
                StartCoroutine(changeWeapon());
                handGun.gameObject.SetActive(false);
            }

        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2) && handGun.gameObject.activeInHierarchy == false)
        {
            isAK = false;
            handGun.transform.position = AK47.transform.position;
            handGun.transform.rotation = AK47.transform.rotation;
            GetComponent<FPSController>().arms = handGun;
            StartCoroutine(changeWeapon());
            AK47.gameObject.SetActive(false);

        }

        //if (isAK)
        //{
        //    primaryWeapon.gameObject.SetActive(true);
        //    secondaryWeapon.gameObject.SetActive(false);
        //}
        //else
        //{
        //    secondaryWeapon.gameObject.SetActive(true);
        //    primaryWeapon.gameObject.SetActive(false);
        //}
    }

    IEnumerator changeWeapon()
    {
        yield return new WaitForSeconds(.25f);
    }
}
