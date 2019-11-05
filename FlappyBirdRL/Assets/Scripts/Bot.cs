using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public static bool isJump = false;

    public static int num_episodes = 0;

    public static float learning_rate = 0.7f;
    public static float discount_rate = 0.9f;

    public static float exploration_rate = 1f;
    public static float max_exploration_rate = 1f;
    public static float min_exploration_rate = 0.01f;
    public static float exploration_decay_rate = 0.001f;
    public static float current_reward = 0;

    public static List<double> rewards_all_episodes = new List<double>();

    public class Q_Table {
        public double x_state { get; set; }
        public double y_state { get; set; }
        public double action_doNothin { get; set; }
        public double action_Jump { get; set; }
    }

    public static List<Q_Table> qTable = new List<Q_Table>();

    private int t = 0;
    private bool isDone = false;
    Bird bird;

    private void Start()
    {
        bird = GetComponent<Bird>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(isDone == true)
        {
            if(bird.isDead == true)
            {
                // end episode
            }
            else
            {
               // qTable[t-1].action_Jump based on prev action do calculation
            }
        }

        if(!isDone)
        {
            double E = UnityEngine.Random.Range(0f, 1f);
            System.Random rand = new System.Random();
            // Perform Action
            double action = 0;
            if (E > exploration_rate)
                action = (qTable.Count - 1) >= t ? (qTable[t].action_doNothin > qTable[t].action_Jump ? 0 : 1) : rand.Next(0, 2);
            else
                action = rand.Next(0, 2);
            isJump = action == 0 ? false : true;
            isDone = true;
          //  qTable.Add(new Q_Table() { x_state = Distance, y_state = Distance })
            t++;
        }

    }
}
