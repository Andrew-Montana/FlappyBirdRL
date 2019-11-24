using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject columnControllerPrefab;

    private float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
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

        if(timer >= 0.5f)
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
                ResetPos();
            }
            rewards_current_episode += reward;
            agent.exploration_rate = Mathf.Clamp(agent.exploration_rate - agent.exploration_decay_rate, agent.min_exploration_rate, agent.max_exploration_rate);

            Debug.Log(action);
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

    public void ResetPos()
    {
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
    }
}
