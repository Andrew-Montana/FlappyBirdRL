using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface ICheck
{
    void Bla();
}

public class Bird : MonoBehaviour
{
    private Rigidbody2D rb;
    private Agent agent;
    private Animator animator;
    private float force;

    private Vector3 startPos;
    private int timeStep;
    public bool isDead;

    public bool isTriggered = false;

    private int action, state, newState; private float reward;
    private double rewards_current_episode = 0;

    private int episode;

    public GameObject scrollingBgObject;
    public GameObject colsObj;
    private Vector3 colsStartPos;
    public TextMeshProUGUI score;
    public int myScore;
    private float timer = 0f;

    // Temporary Data
    int tempMaxState = 0;
    float tempMaxReward = 0;
    float tempLastMaxREpisode = 0;

    // time delta delay
    private float delayTime = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        myScore = 0;
        Instantiate(scrollingBgObject);
        colsStartPos = colsObj.transform.position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isDead = false;
        force = 200;
        newState = 0;
        state = 0;
        action = 0;

        agent = GetComponent<Agent>();
        timeStep = 0;
        isDead = false;
        startPos = transform.localPosition;
        reward = 0;
        episode = 1;

        // 1 step
        agent.InitQTable();
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if(timer >= delayTime)
        {
            timer = 0.0f;
            if (!isDead)
            {
                animator.SetInteger("State", 0);
                // 4 step, reward
                if (timeStep > 0)
                {
                    if (isTriggered == false)
                    {
                        reward = 0.1f;
                    }
                    else
                    {
                        reward = 20;
                        isTriggered = false;
                        score.text = (++myScore).ToString();
                    }
                    // 5 step. Update Q Table
                    agent.UpdateQTable(state, newState, action, reward);
                }


                // 2 step and 3 steps. perform action
                //UserInput();
                action = BotInput();
                state = timeStep;
                newState = ++timeStep;
            }
            else
            {
                // 4 step. reward.
                reward = -1;
                // 5 Step. Update Q Table.
                agent.UpdateQTable(state, newState, action, reward);
                episode++;
                Agent.exploration_rate = Mathf.Clamp(Agent.exploration_rate - Agent.exploration_decay_rate, Agent.min_exploration_rate, Agent.max_exploration_rate);
               // agent.exploration_rate = agent.min_exploration_rate + (agent.max_exploration_rate - agent.min_exploration_rate) * Mathf.Exp((float)-agent.exploration_decay_rate * episode);
                ResetPos();
            }
            rewards_current_episode += reward;
            // agent.exploration_rate = Mathf.Clamp(agent.exploration_rate - agent.exploration_decay_rate, agent.min_exploration_rate, agent.max_exploration_rate);
             Debug.Log(string.Format("episode {0}. E = {1}", episode, Agent.exploration_rate));
        

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isDead == false)
        {
            rb.velocity = Vector2.zero;
            isDead = true;
            animator.SetInteger("State", 2);
        }
    }

    public void UserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Push();
        }
    }

    public int BotInput()
    {
        int action = agent.GetAction(timeStep);
        if (action == 1) Push();

        return action;
    }

    public void Push()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, force));
        animator.SetInteger("State", 1);
    }

    //
    public void SaveData()
    {
        tempMaxState = tempMaxState > state ? tempMaxState : state; // max state ever was
        bool rewardFlag = tempMaxReward > rewards_current_episode ? true : false; // max reward
        if(rewardFlag == false)
        {
            tempMaxReward = (float) rewards_current_episode;
            tempLastMaxREpisode = episode;
            SavePattern(@"D:\Data\bestQTable.txt");
        }
        SavePattern(@"D:\Data\qtable.txt");
    }

    public void SavePattern(string path)
    {
        using (System.IO.StreamWriter sr = new System.IO.StreamWriter(path))
        {
            sr.WriteLine("Episode №{0}", episode);
            sr.WriteLine("delay time №{0}", delayTime);
            sr.WriteLine("Agent data:");
            sr.WriteLine("/t-Learning Rate = {0}", Agent.learning_rate.ToString("0.00000"));
            sr.WriteLine("/t-Discount Rate = {0}", Agent.discount_rate);
            sr.WriteLine("/t-Exploration Rate Decay = {0}", Agent.exploration_decay_rate);
            sr.WriteLine("########");
            sr.WriteLine("Max reward ever was = {0}", tempMaxReward);
            sr.WriteLine("Episode of it = {0}", tempLastMaxREpisode);
            sr.WriteLine("########");
            for (int i = 0; i < tempMaxState; i++)
            {
                sr.WriteLine(i);
                for (int j = 0; j < 2; j++)
                {
                    sr.WriteLine("# " + agent.qTable[i, j]);
                }
            }
        }
    }

    public void ResetPos()
    {
        SaveData();
        // Reset
        animator.SetInteger("State", 0);
        rewards_current_episode = 0;
        rb.velocity = Vector3.zero;
        transform.localPosition = startPos;
        isDead = false;
        timeStep = 0;
        reward = 0;
        state = 0;
        newState = 0;
        action = 0;
        rewards_current_episode = 0;
        timer = 0f;
        myScore = 0;
        score.text = "0";
        GameObject.Destroy(GameObject.Find("Scroll(Clone)"));
        Instantiate(scrollingBgObject);
    }
}
