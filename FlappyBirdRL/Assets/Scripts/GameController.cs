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
        if(gameOver == true && Bot.nextepisode == true)
        {
            Bot.nextepisode = false;
            //  if(Input.GetMouseButton(0))
            //  {
            //      UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            //  }
            Bot.num_episodes++;
            System.IO.File.WriteAllText(@"D:\QTableData.txt", string.Empty);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\QTableData.txt");
            foreach (var item in Bot.qTable)
            {
                sw.WriteLine("x_distance = {0}\ty_distance = {1}\taction0={2}\taction1={3}", item.x_state, item.y_state, item.action_doNothin, item.action_Jump);
            }
            sw.Close();
            Bot.isJump = false;
            Bot.curReward = 0;
            Bot.curAction = 0;
            Bot.isPassed = false;
            Debug.Log("dead");
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
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
        GameObject.Find("Bird").GetComponent<Bird>().isDead = true;
        gameOverTMP.SetActive(true);
        gameOver = true;

    }
}
