
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
        exploration_rate = 100.0f;
        max_exploration_rate = 100.0f;
        min_exploration_rate = 0.01f;
        exploration_decay_rate = 0.01f;
    }

    public float[,] qTable = new float[100000, 2];

    public void InitQTable()
	{

		for(int i = 0; i < 100000; i++)
		{
			for(int j = 0; j < 2; j++)
			{
				qTable[i,j] = 0;
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

	public int GetAction(int state)
	{
		// Greedy Strategy
		/*System.Random rand = new System.Random();
        double exploration_rate_threshold = rand.NextDouble();
		int action = 0;
		if(exploration_rate_threshold > exploration_rate)
			action = qTable[state,0] > qTable[state,1] ? 0 : 1;
		else
			action = rand.Next(0,2);
        */
        int action = qTable[state, 0] > qTable[state, 1] ? 0 : 1;
        return action;

	}


}