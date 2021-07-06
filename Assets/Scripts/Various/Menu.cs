using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject startButton;
    public GameObject controlButton;
    public GameObject creditButton;
    public GameObject exitButton;
    public GameObject controlPanel;
    public GameObject creditPanel;

    void Start()
    {
        Time.timeScale = 1;
        controlPanel.SetActive(false);
        creditPanel.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnMouse()
    {
        Start1();
        Controls();
        Credits();
        Exit();
    }

    public void Start1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Controls()
    {  
        controlPanel.SetActive(!controlPanel.activeSelf);
    }

    public void Credits()
    {
        creditPanel.SetActive(!creditPanel.activeSelf);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
