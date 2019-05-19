using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContract : MonoBehaviour
{
    public GameObject explosion;
    public GameObject explosionPlayer;
    public int scoreValue = 10;
    public GameController gameController;

    void Start()
    {
        GameObject gameControllerObj = GameObject.FindWithTag("GameController");

        if(gameControllerObj != null)
        {
            gameController = gameControllerObj.GetComponent<GameController>();
        }

        if(gameController == null)
        {
            Debug.Log("Cannot find Game Controller script on Object");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Boundary")
        {
            return;
        }

        if(other.tag == "Player")
        {
            // Trigger game over logic in here
            GameObject temp = Instantiate(explosionPlayer, other.transform.position, other.transform.rotation);
            Destroy(temp, 2.0f);
            Destroy(other.gameObject);
            gameController.GameOver();
        }

        // Debug.Log(other.gameObject.name);
        gameController.AddScore(scoreValue);

        // Instantiate asteroid explosion animation
        Instantiate(explosion, this.transform.position, this.transform.rotation);

        Destroy(other.gameObject);  // Laser
        Destroy(this.gameObject);   // Asteroid
    }
}
