using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject gameOverTMP;
    public bool gameOver;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            GameObject.Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        gameOver = false;
    }

    private void Update()
    {
        if(gameOver == true)
        {
            if(Input.GetMouseButton(0))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void GameOver()
    {
        gameOverTMP.SetActive(true);
        gameOver = true;
    }
}
