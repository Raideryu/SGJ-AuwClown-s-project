using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject winMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        winMenu.SetActive(false);
    }

    public void Restart()
    {
        GameManager.Instance.Restart();
    }

    public void MainMenu()
    {
        GameManager.Instance.MainMenu();
    }

    public void Unpause()
    {
        GameManager.Instance.Pause();
    }

}
