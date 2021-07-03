using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public float musicVolume = 1;
    private float sensitivity;

    private void Start()
    {
        sensitivity = FindObjectOfType<FPSController>().mouseSensitivity;
    }

    private void Update()
    {
        sensitivity = FindObjectOfType<FPSController>().mouseSensitivity;
    }

    public void SetVolume(float vol)
    {
        AudioListener.volume = vol;
    }

    public void SetSensitivity(float sens)
    {
        FindObjectOfType<FPSController>().mouseSensitivity = sens;
    }

    public void ReturnGame()
    {
        FindObjectOfType<UIManager>().isPaused = false;
        gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
}
