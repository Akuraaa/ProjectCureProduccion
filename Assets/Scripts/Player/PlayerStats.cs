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
    private bool _lockedGen = false;
    private bool lockDoor = false;
    public int genCount = 0;
    public int grenadeCount = 0;

    public bool hitPlayer = false;
    public bool haveGrenade = false;

    [SerializeField] public bool haveCode, openDoor, haveMap, haveElectricity, haveCrowbar, canGenerator;
    public GameObject electricityBox;
    public GameObject explosion;
    public GameObject[] sparks;
    public GameObject giantBoxCollider;
    public GameObject finalDoor;

    [SerializeField] private Color normalColor, warningColor;
    [SerializeField] public AudioClip pickUp, healthSound;

    public AudioClip doorSound, genSound;
    public AudioSource audioSource;
    private UIManager uiManager;


    public Image healScreen, alphaBloody;
    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        _lockedGen = false;
        
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
            _lockedGen = false;
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
            _lockedGen = false;
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
            _lockedGen = true;
            if (_lockedGen)
            {
                genCount = oilCount;
            }

            switch (genCount)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.F) & !lockDoor)
                    {
                        finalDoor.GetComponent<Animator>().SetInteger("OilCount", 0);
                        lockDoor = true;
                       
                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.F) & !lockDoor)
                    {
                        finalDoor.GetComponent<Animator>().SetInteger("OilCount", 1);
                        finalDoor.GetComponent<AudioSource>().PlayOneShot(doorSound);
                        lockDoor = true;
                        other.gameObject.GetComponent<AudioSource>().enabled = true;
                        if (finalDoor.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                        {
                            other.gameObject.GetComponent<AudioSource>().Stop();
                        }
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.F) & !lockDoor)
                    {
                        finalDoor.GetComponent<Animator>().SetInteger("OilCount", 2);
                        finalDoor.GetComponent<AudioSource>().PlayOneShot(doorSound);
                        lockDoor = true;
                        other.gameObject.GetComponent<AudioSource>().Play();
                        if (finalDoor.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                        {
                            other.gameObject.GetComponent<AudioSource>().Pause();
                        }
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown(KeyCode.F) & !lockDoor)
                    {
                        other.GetComponent<Outline>().enabled = false;
                        finalDoor.GetComponent<Animator>().SetInteger("OilCount", 3);
                        finalDoor.GetComponent<AudioSource>().PlayOneShot(doorSound);
                        lockDoor = true;
                        other.gameObject.GetComponent<AudioSource>().Play();
                        if (finalDoor.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                        {
                            other.gameObject.GetComponent<AudioSource>().Pause();
                        }
                    }
                    break;
            }
        }

        if (other.gameObject.CompareTag("Oil"))
        {
            _lockedGen = false;
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
            _lockedGen = false;
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

    private void OnTriggerExit(Collider other)
    {
       if (other.gameObject.CompareTag("Door"))
       {
            _lockedGen = false;
       }
       if (other.gameObject.CompareTag("AK47"))
       {
           _lockedGen = false;
       }
       if (other.gameObject.CompareTag("Crowbar"))
       {
            _lockedGen = false;
       }
       if (other.gameObject.CompareTag("Electricity"))
       {
            _lockedGen = false;
       }
       if (other.gameObject.CompareTag("ElectricBox"))
       {
            _lockedGen = false;
       }
       if (other.gameObject.CompareTag("Generator"))
       {
            _lockedGen = false;
       }
       if (other.gameObject.CompareTag("Oil"))
       {
            _lockedGen = false;
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
