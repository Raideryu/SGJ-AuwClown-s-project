using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    bool gameEnd = false;
    public Chunk currentChunk;
    ChunkPlacer chunkPlacer;
    MenuController menuController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnd)
            Pause();


        if (currentChunk && !currentChunk.isOpen)
        {
            foreach(Enemy enemy in currentChunk.currentEnemys)
            {
                if (!enemy.GetComponent<BaseCharacter>().isDied)
                    return;
            }
            //если все враги померли 


        }

    }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        chunkPlacer = FindObjectOfType<ChunkPlacer>();
        menuController = FindObjectOfType<MenuController>();
    }

    bool isPaused = false;
    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            menuController.pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            menuController.pauseMenu.SetActive(true);
        }
            
        isPaused = !isPaused;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameWin()
    {
        gameEnd = true;
        Time.timeScale = 0;
        menuController.winMenu.SetActive(true);
        // выиграл
    }

    public void GameOver()
    {
        gameEnd = true;
        Time.timeScale = 0;
        menuController.deathMenu.SetActive(true);
        //проиграл
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
