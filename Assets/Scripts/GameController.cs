using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Define some variables that control spawning my waves of enemies
    [Header("Wave Settings")]
    public GameObject hazard;   // What are we spawning?
    public Vector2 spawnValue;  // Where do we spawn our hazards?
    public int hazardCount;     // How many hazards per wave?
    public float startWait;     // How long until the first wave?
    public float spawnWait;     // How much time between each hazard in a wave?
    public float waveWait;      // How long between each wave of hazard?

    [Header("UI Options")]
    public Text scoreText;
    public Text gameOverText;
    public Text restartText;

    // Private variables
    private int score = 0;
    private bool gameOver;
    private bool restart;

    // Start is called before the first frame update
    void Start()
    {
        // Run a separate function from the rest of the code
        // In it's own thread
        UpdateScore();
        StartCoroutine(SpawnWaves());
        gameOver = false;
        restart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(restart)
        {
            // Listen for a key press
            if(Input.GetKeyDown(KeyCode.R))
            {
                // THE OLD WAY (DON'T DO THIS) BECAUSE IT'S OLD AND OBSOLETE
                //Application.LoadLevel(Application.loadedLevel);

                // The better, but error prone way
                //SceneManager.LoadScene("SampleScene");

                // The best way to reload a scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    // IEnumerator return type is required for Coroutines
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait); // "Pause". This will "wait" for "startWait" seconds
        while(true) // Now we want to spawn some stuff...
        {
            
            for (int i = 0; i < hazardCount; i++)
            {
                Vector2 spawnPosition = new Vector2(spawnValue.x, Random.Range(-spawnValue.y, spawnValue.y));

                Instantiate(hazard, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnWait); // Wait time between spawning each asteroid
            }
            yield return new WaitForSeconds(waveWait);

            // Check if the game is over
            if(gameOver)
            {
                // Tell the user how to restart their game.
                restartText.gameObject.SetActive(true);
                restartText.text = "Press R for Restart";
                restart = true;

                break;
            }
        }

        // Detect that my game is over and show the game over text

    }
    
    // Updates my score text
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    // Accepts score values, then calls update score.
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    // Perform Game Over duties
    public void GameOver()
    {
        // Enable Text on the screen
        gameOverText.gameObject.SetActive(true);
        gameOver = true;
    }
}
