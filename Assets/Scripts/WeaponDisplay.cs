using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour
{
    public Transform ak, handGun;
    public bool isAK, haveAK;
    [SerializeField] private Image primaryWeapon, secondaryWeapon;

    private void Start()
    {
        isAK = false;
        GetComponent<FPSController>().arms = handGun;
    }

    private void Update()
    {
        if (haveAK)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && ak.gameObject.activeInHierarchy == false)
            {
                isAK = true;
                ak.transform.position = handGun.transform.position;
                ak.transform.rotation = handGun.transform.rotation;
                ak.gameObject.SetActive(true);
                GetComponent<FPSController>().arms = ak;
                StartCoroutine(changeWeapon());
                handGun.gameObject.SetActive(false);
            }

        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2) && handGun.gameObject.activeInHierarchy == false)
        {
            isAK = false;
            handGun.transform.position = ak.transform.position;
            handGun.transform.rotation = ak.transform.rotation;
            handGun.gameObject.SetActive(true);
            GetComponent<FPSController>().arms = handGun;
            StartCoroutine(changeWeapon());
            ak.gameObject.SetActive(false);

        }

        if (isAK)
        {
            primaryWeapon.gameObject.SetActive(true);
            secondaryWeapon.gameObject.SetActive(false);
        }
        else
        {
            secondaryWeapon.gameObject.SetActive(true);
            primaryWeapon.gameObject.SetActive(false);
        }
    }

    IEnumerator changeWeapon()
    {
        yield return new WaitForSeconds(.25f);
    }
}
