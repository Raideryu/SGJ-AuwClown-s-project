using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Chunk currentChunk;
    ChunkPlacer chunkPlacer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
        
     

    }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        chunkPlacer = FindObjectOfType<ChunkPlacer>();
    }

    bool isPaused = false;
    public void Pause()
    {
        if (isPaused)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
        isPaused = !isPaused;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameWin()
    {
        Time.timeScale = 0;
        // выиграл
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        //проиграл
    }
}
