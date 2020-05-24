
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public static float learning_rate;

    public static float discount_rate;
    public static float exploration_rate;
    public static float max_exploration_rate;
    public static float min_exploration_rate;
    public static float exploration_decay_rate;

    static Agent()
    {
        learning_rate = 0.8f;
        discount_rate = 0.2f;
        exploration_rate = 1.5f;
        max_exploration_rate = 100.0f;
        min_exploration_rate = 1.5f;
        exploration_decay_rate = 0.1f;
    }

    public float[,] qTable = new float[100000, 2];

    public void InitQTable()
	{
        bool import = true;

        List<float> mydata = new List<float>();
        mydata = LoadData(@"D:\Data\import.txt");
        int dataCount = 0;

        for (int i = 0; i < 100000; i++)
		{
            for (int j = 0; j < 2; j++)
            {
                if (import)
                {
                    qTable[i, j] = dataCount >= mydata.Count ? 0 : mydata[dataCount];
                    dataCount++;
                }
                else
                {
                    qTable[i, j] = 0;
                }
			}
		}
	}

   /// public float[] GetValues()
  //  {
   //     return new float[] { learning_rate, discount_rate, exploration_rate, max_exploration_rate, min_exploration_rate, exploration_decay_rate };
  //  }

	public void UpdateQTable(int state, int newState, int action, float reward)
	{
        float maxFutureQ = qTable[newState, 0] > qTable[newState, 1] ? qTable[newState, 0] : qTable[newState, 1];
		float QValue = (1 - learning_rate) * (qTable[state, action]) + learning_rate * (reward + discount_rate * maxFutureQ );
		qTable[state, action] = QValue;

	}

    public void UpdateQTable_SARSA(int state, int newState, int action, int nextAction, float reward)
    {
        float predict = qTable[state, action];
        float target = reward + discount_rate * qTable[newState, nextAction];
        qTable[state, action] = qTable[state, action] + learning_rate * (target - predict);
    }



    public int GetAction(int state)
    {
        System.Random rand = new System.Random();
        // Greedy Strategy

        double exploration_rate_threshold = rand.Next(0, 101);
        Debug.Log("threshold is " + exploration_rate_threshold.ToString() + ". Epsilon is " + exploration_rate.ToString());
        int myaction = 0;
        if (exploration_rate_threshold > exploration_rate)
        {
            myaction = qTable[state, 0] > qTable[state, 1] ? 0 : 1;
            if (qTable[state, 0] == qTable[state, 1])
            {
                myaction = rand.Next(0, 2);
                Debug.Log("RANDOM");
            }
        }
        else
        {
            myaction = rand.Next(0, 2);
        }
            
            // without greedy strategy:
        //int myaction = qTable[state, 0] > qTable[state, 1] ? 0 : 1;
     //  if(qTable[state,0] == qTable[state,1])
      //  {
      //      myaction = rand.Next(0, 2);
       //     Debug.Log("RANDOM");
     //   }
        return myaction;

	}

    public static List<float> LoadData(string path)
    {
        List<float> list = new List<float>();
        if (System.IO.File.Exists(path))
        {
            //System.IO.StreamReader sr = new System.IO.StreamReader(path);
            string[] array = System.IO.File.ReadAllLines(path);
            int jump = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (jump >= 2)
                {
                    jump = 0;
                    continue;
                }
                list.Add((float)System.Convert.ToDouble(array[i]));
                ++jump;
            }
        }
        return list;
    }


}