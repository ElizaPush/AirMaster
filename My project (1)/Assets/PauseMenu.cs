using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject wristUI;
    public bool activeWristUI = true;
    void Start()
    {
        DisplayWristUI();
    }

    void Update()
    {

    }
    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayWristUI();
        }
    }
    public void DisplayWristUI()
    {
        if (activeWristUI)
        {
            wristUI.SetActive(false);
            activeWristUI = false;
            Time.timeScale = 1;
        }
        else if (!activeWristUI)
        {
            wristUI.SetActive(true);
            activeWristUI = true;
            Time.timeScale = 0;
        }
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Continue()
    {
        wristUI.SetActive(false);
        Time.timeScale = 1f;
        activeWristUI = false;
    }
    public void ExiGame()
    {
        Application.Quit();
    }
}
