using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public Image bloodyScreen, healingScreen, crosshair, AKImg, handGunImg, grenadeImg, crowBarImg, oilImg, healthBar;
    public TMP_Text healthText, oilCountText, grenadeCountText, ammoText, situationText;

    private Color tempColor;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private ParticleSystem healParticle, damageParticle;

    private PlayerStats playerStats;
    private WeaponDisplay weaponDisplay;
    public bool isPaused;

    private void Awake()
    {
        isPaused = false;
        playerStats = FindObjectOfType<PlayerStats>();
        weaponDisplay = FindObjectOfType<WeaponDisplay>();
    }

    private void Start()
    {
        //BLOODYSCREEN & HEAL
        bloodyScreen.enabled = false;
        healingScreen.enabled = false;
        tempColor = bloodyScreen.color;
        tempColor.a = 1;
        bloodyScreen.color = tempColor;
        healingScreen.color = tempColor;

        //PAUSE PANEL
        isPaused = false;
        pausePanel.SetActive(false);

        //OIL, CROWBAR & GRENADE
        oilCountText.text = "0/3";
        oilImg.gameObject.SetActive(false);
        crowBarImg.gameObject.SetActive(false);
        grenadeImg.gameObject.SetActive(false);
        grenadeCountText.text = "0";

        //MOUSE LOCKER
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //HEAL TEXT
        healthText.text = playerStats.curHealth.ToString();

        //SITUATION TEXT
        situationText.gameObject.SetActive(false);

        //WEAPONS SETTINGS
        AKImg.gameObject.SetActive(false);
        handGunImg.gameObject.SetActive(true);

        
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausePanel.activeInHierarchy)
            {
                isPaused = true;
                PauseGame();
            }
            else
            {
                isPaused = false;
                ContinueGame();
            }
        }
        if (playerStats.haveCrowbar)
            crowBarImg.gameObject.SetActive(true);
        else
            crowBarImg.gameObject.SetActive(false);

        if (playerStats.haveElectricity)
        {
            playerStats.electricityBox.SetActive(false);
            foreach (var spark in playerStats.sparks)
            {
                spark.SetActive(false);
            }
        }
        else
            playerStats.electricityBox.SetActive(true);

        if (FindObjectOfType<WeaponDisplay>().haveAK)
        {
            if (FindObjectOfType<AutomaticGunScriptLPFP>().isAiming == true)
            {
                crosshair.gameObject.SetActive(false);
            }
            else
            {
                crosshair.gameObject.SetActive(true);
            }

        }

        if (FindObjectOfType<HandgunScriptLPFP>().isAiming == true)
        {
            crosshair.gameObject.SetActive(false);
        }
        else
            crosshair.gameObject.SetActive(true);

        if (playerStats.haveGrenade)
        {
            grenadeImg.gameObject.SetActive(true);
            grenadeCountText.gameObject.SetActive(true);
            grenadeCountText.text = "" + playerStats.grenadeCount;
        }
        else
        {
            grenadeImg.gameObject.SetActive(false);
            grenadeCountText.gameObject.SetActive(false);
        }

        oilCountText.text = playerStats.oilCount + "/3";
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetHealthBar()
    {
        healthText.text = playerStats.curHealth.ToString();
        healthBar.fillAmount = playerStats.curHealth / playerStats.maxHealth;
    }

    public IEnumerator AlphaBloody()
    {
        bool ignore = false;
        healingScreen.enabled = true;
        healingScreen.canvasRenderer.SetAlpha(1);
        damageParticle.Play();
        healingScreen.CrossFadeAlpha(0, 1, ignore);
        yield return new WaitForSeconds(.5f);
        ignore = true;
    }

    public IEnumerator AlphaHeal()
    {
        bool ignore = false;
        healingScreen.enabled = true;
        healingScreen.canvasRenderer.SetAlpha(1);
        healParticle.Play();
        healingScreen.CrossFadeAlpha(0, 1, ignore);
        yield return new WaitForSeconds(.5f);
        ignore = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealthBox"))
        {
            if (playerStats.curHealth < 100)
            {
                AlphaHeal();
            }
        }

        if (other.gameObject.CompareTag("GrenadeBox"))
        {
            grenadeImg.gameObject.SetActive(true);
            grenadeCountText.text = "" + playerStats.grenadeCount;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("AK47"))
        {
            situationText.text = "Presiona F para agarrar AK47";
            situationText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                situationText.gameObject.SetActive(false);
                AKImg.gameObject.SetActive(true);
            }
        }

        if (other.gameObject.CompareTag("Door"))
        {
            if (playerStats.oilCount == 3)
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Prefiona F para abrir";

                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (playerStats.openDoor)
                    {
                        situationText.gameObject.SetActive(false);
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                    }
                }
            }
            else
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Deberias conseguir mas combustible";
            }
        }

        if (other.gameObject.CompareTag("Generator"))
        {
            switch (playerStats.genCount)
            {
                case 0:
                    oilImg.gameObject.SetActive(true);
                    situationText.gameObject.SetActive(true);
                    situationText.text = "Necesitas combustible";
                    break;
                case 1:
                    situationText.gameObject.SetActive(true);
                    situationText.text = "Necesitas mas combustible";
                    break;
                case 2:
                    situationText.gameObject.SetActive(true);
                    situationText.text = "Necesitas mas combustible";
                    break;
            }
        }

        if (other.gameObject.CompareTag("Oil"))
        {
            oilImg.gameObject.SetActive(true);
            if (playerStats.oilCount > 3)
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Presiona F para agarrar";

                if (Input.GetKeyDown(KeyCode.F))
                {
                    situationText.gameObject.SetActive(false);
                }
            }
        }


        if (other.gameObject.CompareTag("Electricity"))
        {  
            if (playerStats.haveElectricity)
            {
                situationText.gameObject.SetActive(false);
            }
            else
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Deberia cortar la electricidad";
            }
        }

        if (other.gameObject.CompareTag("ElectricBox"))
        {
            if (playerStats.haveCrowbar)
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Presione F para usar la barreta";

                if (Input.GetKeyDown(KeyCode.F))
                {

                    crowBarImg.gameObject.SetActive(false);
                    situationText.gameObject.SetActive(false);
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

            situationText.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("AK47"))
        {
            situationText.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Crowbar"))
        {
            situationText.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Electricity"))
        {
            situationText.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("ElectricBox"))
        {
            situationText.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Generator"))
        {
            situationText.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Oil"))
        {
            situationText.gameObject.SetActive(false);
        }
    }
}
