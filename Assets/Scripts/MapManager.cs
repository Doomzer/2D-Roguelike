using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
   [Serializable]
   public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int cols = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject player;
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform mapHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitList()
    {
        gridPositions.Clear();
        
        for(int x = 1; x< cols - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void MapSetup()
    {
        mapHolder = new GameObject("Map").transform;
        for (int x = -1; x < cols + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInst = floorTiles[UnityEngine.Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == cols || y == -1 || y == rows)
                    toInst = outerWallTiles[UnityEngine.Random.Range(0, outerWallTiles.Length)];

                GameObject inst = Instantiate(toInst, new Vector3(x, y, 0f), Quaternion.identity);
                inst.transform.SetParent(mapHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = UnityEngine.Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        int objCount = UnityEngine.Random.Range(min, max + 1);
        for (int x = -1; x < objCount; x++)
        {
            Vector3 randomPos = RandomPosition();
            GameObject tileChose = tileArray[UnityEngine.Random.Range(0, tileArray.Length)];
            Instantiate(tileChose, randomPos, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        MapSetup();
        InitList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(cols - 1, rows - 1, 0f), Quaternion.identity);
        //Instantiate(player, new Vector3(0, 0, 0f), Quaternion.identity);
    }
}
