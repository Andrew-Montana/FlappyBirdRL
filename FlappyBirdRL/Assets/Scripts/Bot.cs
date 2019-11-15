using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public static bool isJump = false;
    public static int curReward = 0;
    public static int curAction = 0;
    public static bool isPassed = false;
    //

    public static int num_episodes = 1;

    public static float learning_rate = 0.7f;
    public static float discount_rate = 0.99f;

    public static float exploration_rate = 0.7f;
    public static float max_exploration_rate = 0.7f;
    public static float min_exploration_rate = 0.01f;
    public static float exploration_decay_rate = 0.001f;
   // public static float current_reward = 0;

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
    public static bool nextepisode = false;
    Bird bird;

    private void Start()
    {
        bird = GetComponent<Bird>();   
    }


    // Update is called once per frame
    void Update()
    {
      //  Debug.Log(string.Format("t={0}, qtable.count={1}", t, qTable.Count));
        
            if(bird.isDead == true)
            {
                curReward = -999;
                // Calculate Q for that action qTable[t-1]. 
                double QValue;
               if(curAction == 0)
                {
                    double newState0 = (qTable.Count - 1) >= t ? qTable[t].action_doNothin : 0;
                    double newState1 = (qTable.Count - 1) >= t ? qTable[t].action_Jump : 0;
                    double newstate = newState0 > newState1 ? newState0 : newState1;
                    QValue = qTable[t - 1].action_doNothin * (1 - learning_rate) + learning_rate * (curReward + discount_rate * newstate);
                    qTable[t - 1].action_doNothin = QValue;
                    rewards_all_episodes.Add(curReward);
                }
               else if(curAction == 1)
                {
                    double newState0 = (qTable.Count - 1) >= t ? qTable[t].action_doNothin : 0;
                    double newState1 = (qTable.Count - 1) >= t ? qTable[t].action_Jump : 0;
                    double newstate = newState0 > newState1 ? newState0 : newState1;
                    QValue = qTable[t - 1].action_Jump * (1 - learning_rate) + learning_rate * (curReward + discount_rate * newstate);
                    qTable[t - 1].action_Jump = QValue;
                    rewards_all_episodes.Add(curReward);
                }
                // end episode
                isPassed = false;
                nextepisode = true;
            isDone = false;
            GameObject.Find("GameController").GetComponent<GameController>().GameOver();
            }
            else if(isPassed == true && isDone == true)
            {
                curReward = 10;
                // Calculate Q for that action qTable[t-1]. 
                double QValue;
                if (curAction == 0)
                {
                    double newState0 = (qTable.Count - 1) >= t ? qTable[t].action_doNothin : 0;
                    double newState1 = (qTable.Count - 1) >= t ? qTable[t].action_Jump : 0;
                    double newstate = newState0 > newState1 ? newState0 : newState1;
                    QValue = qTable[t - 1].action_doNothin * (1 - learning_rate) + learning_rate * (curReward + discount_rate * newstate);
                    qTable[t - 1].action_doNothin = QValue;
                    rewards_all_episodes.Add(curReward);
                }
                else if (curAction == 1)
                {
                    double newState0 = (qTable.Count - 1) >= t ? qTable[t].action_doNothin : 0;
                    double newState1 = (qTable.Count - 1) >= t ? qTable[t].action_Jump : 0;
                    double newstate = newState0 > newState1 ? newState0 : newState1;
                    QValue = qTable[t - 1].action_Jump * (1 - learning_rate) + learning_rate * (curReward + discount_rate * newstate);
                    qTable[t - 1].action_Jump = QValue;
                    rewards_all_episodes.Add(curReward);
                }
                // end episode
                isPassed = false;
                isDone = false;
            exploration_rate = min_exploration_rate + (max_exploration_rate - min_exploration_rate) * Mathf.Exp(-exploration_decay_rate * num_episodes);
        }
            else if(isDone == true)
            {
                curReward = 1;
                // Calculate Q for that action qTable[t-1]. 
                double QValue;
                if (curAction == 0)
                {
                    double newState0 = (qTable.Count - 1) >= t ? qTable[t].action_doNothin : 0;
                    double newState1 = (qTable.Count - 1) >= t ? qTable[t].action_Jump : 0;
                    double newstate = newState0 > newState1 ? newState0 : newState1;
                    QValue = qTable[t - 1].action_doNothin * (1 - learning_rate) + learning_rate * (curReward + discount_rate * newstate);
                    qTable[t - 1].action_doNothin = QValue;
                    rewards_all_episodes.Add(curReward);
                exploration_rate = min_exploration_rate + (max_exploration_rate - min_exploration_rate) * Mathf.Exp(-exploration_decay_rate * num_episodes);
            }
                else if (curAction == 1)
                {
                    double newState0 = (qTable.Count - 1) >= t ? qTable[t].action_doNothin : 0;
                    double newState1 = (qTable.Count - 1) >= t ? qTable[t].action_Jump : 0;
                    double newstate = newState0 > newState1 ? newState0 : newState1;
                    QValue = qTable[t - 1].action_Jump * (1 - learning_rate) + learning_rate * (curReward + discount_rate * newstate);
                    qTable[t - 1].action_Jump = QValue;
                    rewards_all_episodes.Add(curReward);
                }
                // end episode
                isPassed = false;
                 isDone = false;
            exploration_rate = min_exploration_rate + (max_exploration_rate - min_exploration_rate) * Mathf.Exp(-exploration_decay_rate * num_episodes);
        }

            if (num_episodes == 1000)
            {
                double sum = 0;
                for (int i = 0; i < rewards_all_episodes.Count; i++)
                {
                    sum += rewards_all_episodes[i];
                }
                sum /= 1000;
                Debug.Log("AvG per1k after 1000 episodes ="+ sum );
            }
            if (num_episodes == 2000)
            {
                double sum = 0;
                for (int i = 1000; i < rewards_all_episodes.Count; i++)
                {
                    sum += rewards_all_episodes[i];
                }
                sum /= 1000;
                Debug.Log("AvG per 1k after 2000 episodes =" + sum);
            }
            if (num_episodes == 3000)
            {
                double sum = 0;
                for (int i = 2000; i < rewards_all_episodes.Count; i++)
                {
                    sum += rewards_all_episodes[i];
                }
                sum /= 1000;
                Debug.Log("AvG per 1k after 2000 episodes =" + sum);
            }
        

        if(!isDone && bird.isDead == false)
        {
            double E = UnityEngine.Random.Range(0f, 1f);
         //   Debug.Log(string.Format("E = {0} and rate = {1}",E,exploration_rate));
            System.Random rand = new System.Random();
            // Perform Action
           // double action = 0;
            if (E > exploration_rate)
                curAction = (qTable.Count - 1) >= t ? (qTable[t].action_doNothin > qTable[t].action_Jump ? 0 : 1) : 0;
            else
                curAction = rand.Next(0, 2);
            if((qTable.Count - 1) >= t)
                Debug.Log(qTable[t].action_doNothin.ToString() + " > " + qTable[t].action_Jump.ToString() + " ? " + curAction);
         //   Debug.Log((E > exploration_rate));
            isJump = curAction == 0 ? false : true;
            isDone = true;
            if((qTable.Count - 1) < t )
                qTable.Add(new Q_Table() { x_state = bird.horizontalDistance, y_state = bird.verticalDistance });
            else
            {
                qTable[t].x_state = bird.horizontalDistance;
                qTable[t].y_state = bird.verticalDistance;
            }
            t++;
        }

    }
}
