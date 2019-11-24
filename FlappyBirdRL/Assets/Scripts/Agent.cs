
using UnityEngine;

public class Agent : MonoBehaviour
{
	public double learning_rate = 0.6;
	public double discount_rate = 0.99;

	public double exploration_rate = 0.3;
	public double max_exploration_rate = 1;
	public double min_exploration_rate = 0.01;
	public double exploration_decay_rate = 0.01;

    public double[,] qTable = new double[10000, 2];

    public void InitQTable()
	{

		for(int i = 0; i < 10000; i++)
		{
			for(int j = 0; j < 2; j++)
			{
				qTable[i,j] = 0;
			}
		}
	}

	public void UpdateQTable(int state, int newState, int action, double reward)
	{
		double maxFutureQ = qTable[newState, 0] > qTable[newState, 1] ? qTable[newState, 0] : qTable[newState, 1];
		double QValue = (1 - learning_rate) * (qTable[state, action]) + learning_rate * (reward + discount_rate * maxFutureQ );
		qTable[state, action] = QValue;

	}

	public int GetAction(int state)
	{
		// Greedy Strategy
		System.Random rand = new System.Random();
        double exploration_rate_threshold = rand.NextDouble();
		int action = 0;
		if(exploration_rate_threshold > exploration_rate)
			action = qTable[state,0] > qTable[state,1] ? 0 : 1;
		else
			action = rand.Next(0,2);

		return action;

	}


}