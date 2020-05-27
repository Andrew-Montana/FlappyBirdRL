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
    private int a1;
    private int a2;

    private Rigidbody2D rb;
    private Agent agent;
    private Animator animator;
    private float force;

    private Vector3 startPos;
    private int timeStep;
    public bool isDead;

    public bool isTriggered = false;
    public bool isEndState = false;

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
    private float delayTime = 0.4f;

    private string collectdatapath = @"D:\Evaluation\1 QLearning_determ\find alpha and discount tests\alpha 0_8 discount UP\1.txt";
    private int testNumber = 1;

    private void StartTest()
    {
        if(episode > 1350)
        {
            isEndState = true;
        }

        if (episode > 0 && episode < 200)
        {
            Agent.discount_rate = 0.2f;
        }
        else if (episode > 201 && episode < 400)
        {
            Agent.discount_rate = 0.3f;
        }
        else if (episode > 401 && episode < 600)
        {
            Agent.discount_rate = 0.4f;
        }
        else if (episode > 601 && episode < 800)
        {
            Agent.discount_rate = 0.5f;
        }
        else if (episode > 801 && episode < 1000)
        {
            Agent.discount_rate = 0.6f;
        }
        else if (episode > 1200)
        {
            Agent.discount_rate = 0.7f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartTest();
        a1 = 0;
        a2 = 0;
        Time.timeScale = 3f;
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

    private void Update()
    {
        Debug.Log(string.Format("episode {0} and reward {1}. Exploration now: {2}", episode, rewards_current_episode, Agent.exploration_rate));
    }

    private void FixedUpdate()
    {
        StartTest();
        QLearning();
        Debug.Log("reward = " + rewards_current_episode.ToString());
    }

    private void QLearning()
    {
        timer += Time.deltaTime;

        if (timer >= delayTime)
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
                        reward = 1f; // was 0.1f
                    }
                    else
                    {
                        reward = 200; // was 20
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
                reward = -1000; // was -1
                // 5 Step. Update Q Table.
                agent.UpdateQTable(state, newState, action, reward);
                episode++;
               ///// Agent.exploration_rate = Mathf.Clamp(Agent.exploration_rate - Agent.exploration_decay_rate, Agent.min_exploration_rate, Agent.max_exploration_rate);
                // agent.exploration_rate = agent.min_exploration_rate + (agent.max_exploration_rate - agent.min_exploration_rate) * Mathf.Exp((float)-agent.exploration_decay_rate * episode);
                ResetPos();
            }
            rewards_current_episode += reward; // перенести 2 раза в выше
            // agent.exploration_rate = Mathf.Clamp(agent.exploration_rate - agent.exploration_decay_rate, agent.min_exploration_rate, agent.max_exploration_rate);
            Debug.Log(string.Format("episode {0}. E = {1}", episode, Agent.exploration_rate));
            //

            // Is that end state?
            if (isEndState == true)
            {
                episode++;
                SaveData();
                CollectData();
                GameCompleted();
            }

        }
    }

    private void SARSA()
    {
        timer += Time.deltaTime;

        if (timer >= delayTime)
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
                        reward = 1f; // was 0.1f
                    }
                    else
                    {
                        reward = 200; // was 20
                        isTriggered = false;
                        score.text = (++myScore).ToString();
                    }
                    // 5 step. Update Q Table
                    agent.UpdateQTable_SARSA(state, newState, a1, a2, reward);
                    a1 = a2;
                }


                // 2 step and 3 steps. perform action
                //UserInput();


                // action = BotInput();
                // # action Perform ()
                // # action = Get()

                if (timeStep == 0)
                {
                    action = GetAction(timeStep); 
                    a1 = action;
                }
                DoAction(a1);
                action = GetAction(timeStep+1); // a2
                a2 = action;

                state = timeStep;
                newState = ++timeStep;
            }
            else
            {
                // 4 step. reward.
                reward = -1000; // was -1
                // 5 Step. Update Q Table.
                agent.UpdateQTable_SARSA(state, newState, a1, a2, reward);
                episode++;
            //    Agent.exploration_rate = Mathf.Clamp(Agent.exploration_rate - Agent.exploration_decay_rate, Agent.min_exploration_rate, Agent.max_exploration_rate);
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

    // state
    public int GetAction(int _timestep)
    {
        return agent.GetAction(_timestep);
    }

    public void DoAction(int _action)
    {
        if(_action == 1)
        {
            Push();
        }
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

    public void SaveCollectDataPattern(string path)
    {
        using (System.IO.StreamWriter sr = new System.IO.StreamWriter(path,true))
        {
            //  sr.WriteLine("{0};{1};",episode,rewards_current_episode);
            sr.WriteLine("{0};{1};{2};", episode, rewards_current_episode, Agent.exploration_rate.ToString().Replace(',','.')); // for greedy str.
        }
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

    private void GameCompleted()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        ResetPos();
        episode = 0;
        testNumber++;
        collectdatapath = @"D:\Evaluation\1 QLearning_determ\find alpha and discount tests\alpha 0_8 discount UP\" + testNumber.ToString() + ".txt";
        isEndState = false;
        Agent.exploration_rate = -1.5f;
        agent.qTable = null;
        agent.qTable = new float[100000, 2];
        agent.InitQTable();
        isTriggered = false;
        // EndGame
    }

    // Collects data of current iteration/episode
    private void CollectData()
    {
        SaveCollectDataPattern(collectdatapath);
        // Episode number                   - 1 column
        // Max. Cumulative Reward number    - 2 column

        // # if exploration takes place
        // Explore Decay value               - 3 column
        // Exploration value of the episode  - 4 col

    }

    public void ResetPos()
    {
        SaveData();
        CollectData();
        a1 = 0;
        a2 = 0;
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
