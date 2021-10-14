using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public float levelStartDelay = 2f;
    public float turnDelay = 0.5f;    
    public int playerFood = 100;
    public bool playersTurn = true;
    public bool movingNow = false;

    Text levelText;
    Text debugText;
    Text framesText;
    GameObject levelImage;
    MapManager mapScript;
    int levelNum = 1;
    List<Enemy> enemies;
    bool enemiesMoving;
    bool doingSetup;

   void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();
        mapScript = GetComponent<MapManager>();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;

        InitGame();
    }

    void OnLevelWasLoaded(int level)
    {
        levelNum++;
        InitGame();
    }
       
    void InitGame()
    {
        doingSetup = true;

        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        debugText.text = "";
        framesText = GameObject.Find("FramesText").GetComponent<Text>();
        framesText.text = "FPS: 0";

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + levelNum;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        
        enemies.Clear();
        mapScript.SetupScene(levelNum);
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        enabled = false;
        levelText.text = "After " + levelNum + " days, you died";
        levelImage.SetActive(true);
    }

    void Update()
    {
        framesText.text = "FPS: " + (int)(1.0f / Time.smoothDeltaTime);
        debugText.text = "Moving now: " + movingNow;
        if (playersTurn || enemiesMoving || doingSetup || movingNow)
          return;

        StartCoroutine(MoveEnemies());
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;

        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            yield return new WaitWhile(() => movingNow);
            //WaitForSeconds(turnDelay);
            enemies[i].Move();            
        }       

        enemiesMoving = false;
        playersTurn = true;
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    public void SetDebugText(string text)
    {
        debugText.text = text;
    }
}
