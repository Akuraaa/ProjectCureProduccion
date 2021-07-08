using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public float musicVolume;
    private float sensitivity;
    public GameObject pauseMenu, optionsMenu;

    private void Start()
    {
        sensitivity = FindObjectOfType<FPSController>().mouseSensitivity;
        musicVolume = AudioListener.volume;
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    private void Update()
    {
        sensitivity = FindObjectOfType<FPSController>().mouseSensitivity;
        AudioListener.volume = musicVolume;
    }

    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }

    public void SetSensitivity(float sens)
    {
        FindObjectOfType<FPSController>().mouseSensitivity = sens;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ReturnGame()
    {
        FindObjectOfType<UIManager>().isPaused = false;
        gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    public void OptionsButton()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ExitOptions()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
}
