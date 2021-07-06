using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100;
    public float curHealth = 0;
    public int oilCount = 0;
    private bool lockDoor = false;
    public int grenadeCount = 0;

    public bool hitPlayer = false;
    public bool haveGrenade = false;

    [SerializeField] public bool haveCode, openDoor, haveMap, haveElectricity, haveCrowbar, canGenerator;
    public GameObject electricityBox;
    public GameObject explosion;
    public GameObject[] sparks;
    public GameObject giantBoxCollider;
    public GameObject finalDoor;
    public GameObject finalHorde;

    [SerializeField] private Color normalColor, warningColor;
    [SerializeField] public AudioClip pickUp, healthSound, finalHordeSound;

    public AudioClip doorSound, genSound;
    public AudioSource audioSource;
    private UIManager uiManager;


    public Image healScreen, alphaBloody;
    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        
        oilCount = 0;

        curHealth = maxHealth;

    }

    private void Update()
    {
        if (FindObjectOfType<UIManager>().isPaused == false)
        {
            if (haveElectricity)
            {
                electricityBox.SetActive(false);
                foreach (var spark in sparks)
                {
                    spark.SetActive(false);
                }
            }
            else
            {
                //electricityBox.SetActive(true);
            }

            if (grenadeCount > 0)
            {
                haveGrenade = true;
            }
            else
            {
                haveGrenade = false;
            }

        }
    }


    public void TakeDamage(float damage)
    {
        curHealth -= damage;
        StartCoroutine(uiManager.AlphaBloody());
        uiManager.SetHealthBar();
        if (curHealth < 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Derrota");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealthBox"))
        {
            audioSource.PlayOneShot(healthSound);
            if (curHealth < 100)
            {
                curHealth += 20;
                if (curHealth > 100)
                    curHealth = 100;
            }
            Destroy(other.gameObject);
            uiManager.StartCoroutine(uiManager.AlphaHeal());
        }

        if (other.gameObject.CompareTag("Crowbar"))
        {
            if (!haveCrowbar)
            {
                haveCrowbar = true;
                audioSource.PlayOneShot(pickUp);
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("GrenadeBox"))
        {
            audioSource.PlayOneShot(pickUp);
            haveGrenade = true;
            grenadeCount += 1;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("AK47"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                FindObjectOfType<WeaponDisplay>().haveAK = true;
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("Door"))
        {
            if (oilCount == 3)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {            
                    openDoor = true;
                    if (openDoor)
                    {
                        SceneManager.LoadScene("Win");
                    }
                }
            }
            else
            {
                uiManager.situationText.gameObject.SetActive(true);
                uiManager.situationText.text = "No puedes abrir porque no hay electricidad";
            }

        }

        if (other.gameObject.CompareTag("Generator"))
        {
            if (oilCount < 3)
            {
                finalDoor.GetComponent<Animator>().SetTrigger("Open");
                finalDoor.GetComponent<AudioSource>().PlayOneShot(doorSound);
                finalHorde.SetActive(true);
                finalHorde.GetComponent<AudioSource>().PlayOneShot(finalHordeSound);
            }
                       
        }

        if (other.gameObject.CompareTag("Oil"))
        {
            lockDoor = false;
            if (oilCount < 3)
            {

                if (Input.GetKeyDown(KeyCode.F))
                {
                    oilCount += 1;
                    audioSource.PlayOneShot(pickUp);
                    Destroy(other.gameObject);
                }
            }
        }

        if (other.gameObject.CompareTag("ElectricBox"))
        {
            bool haveExplosion = false;
            if (haveCrowbar)
            {         

                if (Input.GetKeyDown(KeyCode.F))
                {
                    haveElectricity = true;
                    giantBoxCollider.SetActive(false);
                    if (!haveExplosion)
                    {
                        haveExplosion = true;
                        Instantiate(explosion, other.transform.position, other.transform.rotation);
                        Destroy(explosion, 10f);
                    }
                }
            }

        }

    }

    //private void SetLightning()
    //{
    //    for (int i = 0; i < spotLights.Length; i++)
    //    {
    //        spotLights[i].GetComponent<LightScript>().changeColor = true;
    //        spotLights[i].color = warningColor;
    //    }
    //}
    //
    //private void SetTimerOn()
    //{
    //    if (timerToFinishLevel > 0)
    //    {
    //        timerToFinishLevel -= Time.deltaTime;
    //    }
    //    else
    //    {
    //        timerToFinishLevel = 0;
    //    }
    //    DisplayTime(timerToFinishLevel);
    //}
    //
    //void DisplayTime(float timeToDisplay)
    //{
    //    if (timeToDisplay < 0)
    //    {
    //        timeToDisplay = 0;
    //    }
    //
    //    float seconds = Mathf.FloorToInt(timeToDisplay % 60);
    //    float miliseconds = timeToDisplay % 1 * 100;
    //
    //    timeText.text = string.Format("{0:00}:{1:00}", seconds, miliseconds);
    //}
}
