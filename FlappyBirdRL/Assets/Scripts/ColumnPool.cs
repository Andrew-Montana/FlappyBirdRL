using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnPool : MonoBehaviour
{
    private GameObject[] columns;
    public int columnPoolSoze = 5;
    public GameObject columnPrefab;
    private Vector2 objectPoolPosition = new Vector2(-15f, -25f);
    private float counter;
    public float spawnRate = 4f;
    public float colMin = -1f;
    public float colMax = 3.5f;
    private float spawnXPos = 5f;
    private int currentCol = 0;
    // Start is called before the first frame update
    void Start()
    {
        columns = new GameObject[columnPoolSoze];
        for (int i = 0; i < columnPoolSoze; i++)
        {
            columns[i] = (GameObject)Instantiate(columnPrefab, objectPoolPosition, Quaternion.identity);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if(GameController.instance.gameOver == false)
        {
            if(counter >= spawnRate)
            {
                counter = 0;
                float spawnYPos = UnityEngine.Random.Range(colMin, colMax);
                columns[currentCol].transform.position = new Vector2(spawnXPos, spawnYPos);
                currentCol++;

                if(currentCol >= columnPoolSoze)
                {
                    currentCol = 0;
                }
            }
        }
    }
}
