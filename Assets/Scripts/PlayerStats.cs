using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public Image healthBar;
    public Image bloodyScreen;
    public Image crowbarImg;
    public Image oilImg;

    public TMP_Text healthText;
    public TMP_Text oilCountText;
    public float maxHealth = 100;
    public float curHealth = 0;
    public int oilCount = 0;
    private bool _lockedGen = false;
    private bool lockDoor = false;
    public int genCount = 0;

    public float timeToFade = 2;
    public bool hitPlayer = false;
    private Color alphaColor;

    //public float timerToFinishLevel;

    [SerializeField] private TMP_Text situationText;
    [SerializeField] public bool haveCode, openDoor, haveMap, haveElectricity, haveCrowbar, canGenerator;
    public GameObject electricityBox;
    public GameObject explosion;
    public GameObject[] sparks;
    public GameObject giantBoxCollider;
    public GameObject finalDoor;

    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Light[] spotLights;
    [SerializeField] private Color normalColor, warningColor;
    [SerializeField] private AudioClip pickUp, healthSound;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject pausePanel;

    public AudioClip doorSound, genSound;

    private void Start()
    {
        _lockedGen = false;
        pausePanel.SetActive(false);
        oilCount = 0;
        oilCountText.text = oilCount + "/3";
        oilImg.gameObject.SetActive(false);
        Cursor.visible = false;
        alphaColor = bloodyScreen.color;
        alphaColor.a = 0;
        bloodyScreen.color = alphaColor;
        curHealth = maxHealth;
        healthText.text = curHealth.ToString();
        SetHealthBar();
        situationText.gameObject.SetActive(false);
        //timeText.gameObject.SetActive(false);
        for (int i = 0; i < spotLights.Length; i++)
        {
            spotLights[i].color = normalColor;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausePanel.activeInHierarchy)
            {
                
                PauseGame();
            }
            else
            {
                ContinueGame();
                
            }
        }

        bloodyScreen.color = alphaColor;
        if (hitPlayer)
        {
            timeToFade -= Time.deltaTime;
            if (timeToFade <= 0)
            {
                bloodyScreen.color = alphaColor;
                alphaColor.a -= Time.deltaTime;
                timeToFade = .25f;
                return;
            }

            if (alphaColor.a <= 0)
            {
                hitPlayer = false;
                timeToFade = 2;
            }
        }

        if (haveMap && Input.GetKey(KeyCode.V))
        {
            map.SetActive(true);
        }
        else
        {
            map.SetActive(false);
        }

        if (haveCrowbar)
        {
            crowbarImg.gameObject.SetActive(true);
        }
        else
        {
            crowbarImg.gameObject.SetActive(false);
        }

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
            electricityBox.SetActive(true);
        }

        oilCountText.text = oilCount + "/3";
    

    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetHealthBar()
    {
        healthText.text = curHealth.ToString();
        healthBar.fillAmount = curHealth / maxHealth;
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;
        alphaColor.a = (maxHealth - curHealth) * 0.01f;
        bloodyScreen.color = alphaColor;
        SetHealthBar();
        if (curHealth < 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Derrota");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mapa"))
        {
            haveMap = true;
            GetComponent<AudioSource>().PlayOneShot(pickUp);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Healthpickup"))
        {
            GetComponent<AudioSource>().PlayOneShot(healthSound);
            if (curHealth > 100)
            {
                curHealth += 20;
                if (curHealth > 100)
                    curHealth = 100;
            }
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            _lockedGen = false;
            if (oilCount == 3)
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Presiona F para abrir";
                if (Input.GetKeyDown(KeyCode.F))
                {            
                    openDoor = true;
                    if (openDoor)
                    {
                        situationText.gameObject.SetActive(false);
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        SceneManager.LoadScene("Win");
                    }
                }
            }
            else
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "No puedes abrir porque no hay electricidad";
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
                     oilImg.gameObject.SetActive(true);
                     situationText.gameObject.SetActive(true);
                     situationText.text = "Necesitas combustible";
                    if (Input.GetKeyDown(KeyCode.F) & !lockDoor)
                    {
                        finalDoor.GetComponent<Animator>().SetInteger("OilCount", 0);
                        lockDoor = true;
                       
                    }
                    break;
                case 1:
                        situationText.gameObject.SetActive(true);
                        situationText.text = "Necesitas mas combustible";
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
                        situationText.gameObject.SetActive(true);
                        situationText.text = "Necesitas mas combustible";
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
            oilImg.gameObject.SetActive(true);
            if (oilCount < 3)
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Presiona F para agarrar";

                if (Input.GetKeyDown(KeyCode.F))
                {
                    oilCount += 1;
                    GetComponent<AudioSource>().PlayOneShot(pickUp);
                    Destroy(other.gameObject);
                    situationText.gameObject.SetActive(false);
                }
            }
        }

        if (other.gameObject.CompareTag("Electricity"))
        {
            _lockedGen = false;
            if (haveElectricity)
            {
                situationText.gameObject.SetActive(false);         
            }
            else
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Deberia cortar la electricidad";
            }
        }

        if (other.gameObject.CompareTag("Crowbar"))
        {
            _lockedGen = false;
            if (!haveCrowbar)
            {
                haveCrowbar = true;
                GetComponent<AudioSource>().PlayOneShot(pickUp);
                Destroy(other.gameObject);
            }
        }


        if (other.gameObject.CompareTag("ElectricBox"))
        {
            _lockedGen = false;
            bool haveExplosion = false;
            if (haveCrowbar)
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Presione F para usar la barreta";

                if (Input.GetKeyDown(KeyCode.F))
                {
                    haveElectricity = true;
                    
                    crowbarImg.gameObject.SetActive(false);          
                    situationText.gameObject.SetActive(false);
                    giantBoxCollider.SetActive(false);
                    if (!haveExplosion)
                    {
                        haveExplosion = true;
                        Instantiate(explosion, other.transform.position, other.transform.rotation);
                        Destroy(explosion, 10f);
                    }
                }
            }
            else
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Deberia buscar una barreta";
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
       if (other.gameObject.CompareTag("Door"))
       {
            _lockedGen = false;
            situationText.gameObject.SetActive(false);
       }
       if (other.gameObject.CompareTag("Crowbar"))
       {
            _lockedGen = false;
            situationText.gameObject.SetActive(false);
       }
       if (other.gameObject.CompareTag("Electricity"))
       {
            _lockedGen = false;
            situationText.gameObject.SetActive(false);
       }
       if (other.gameObject.CompareTag("ElectricBox"))
       {
            _lockedGen = false;
            situationText.gameObject.SetActive(false);
       }
       if (other.gameObject.CompareTag("Generator"))
       {
            _lockedGen = false;
            situationText.gameObject.SetActive(false);
       }
       if (other.gameObject.CompareTag("Oil"))
       {
            _lockedGen = false;
            situationText.gameObject.SetActive(false);
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
