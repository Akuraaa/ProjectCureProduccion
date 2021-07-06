using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour
{
    public Transform AK47, handGun;
    public GameObject handgunObject, akObject;
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
                akObject.SetActive(true);
                isAK = true;
                AK47.transform.position = handGun.transform.position;
                AK47.transform.rotation = handGun.transform.rotation;
                FindObjectOfType<FPSController>().arms = AK47;
                StartCoroutine(changeWeapon());
               // handGun.gameObject.SetActive(false);
                handgunObject.SetActive(false);
            }

        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2) && handGun.gameObject.activeInHierarchy == false)
        {
            handgunObject.SetActive(true);
            isAK = false;
            handGun.transform.position = AK47.transform.position;
            handGun.transform.rotation = AK47.transform.rotation;
            FindObjectOfType<FPSController>().arms = handGun;
            StartCoroutine(changeWeapon());
            //AK47.gameObject.SetActive(false);
            akObject.SetActive(false);

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
