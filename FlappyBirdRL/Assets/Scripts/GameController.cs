using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public bool gameOver;
    private int score;

    public GameObject gameOverTMP;
    public TextMeshProUGUI tmp;

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

    public void GetScore()
    {
        if (!gameOver)
        {
            score++;
            tmp.text = "" + score.ToString();
        }
    }

    public void GameOver()
    {
        gameOverTMP.SetActive(true);
        gameOver = true;
    }
}
