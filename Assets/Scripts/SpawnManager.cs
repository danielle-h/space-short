using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject boss1;
    public GameObject boss2;
    private int level = 1;
    private GameManager gameManager;
    [SerializeField] AudioClip mainMusic;
    [SerializeField] AudioClip bossMusic;
    //AudioSource audioSource;
    AudioManager audioManager;

    bool isShort = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        //Debug.Log("start level is " + level);
        SpawnLevel();//TODO maybe update should be in gamemanager?
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        //audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().GetComponent<AudioSource>();
        //audioSource.clip = mainMusic;
        //audioSource.loop = true;
        //audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        int numEnemies = GameObject.FindObjectsOfType<EnemyController>().Length;
        if (numEnemies == 0 && gameManager.gameOn && (level != 5 && level != 10))
        {
            level++;
            SpawnLevel();
        }
        int numBosses = GameObject.FindObjectsOfType<Boss1Controller>().Length;
        if ((level == 5 || level==10) && numBosses == 0 && gameManager.gameOn)
        {
            level++;
            SpawnLevel();
        }
       

    }

    //Spawns all the Enemies for hte current level
    //todo spawn objects according to screen width
    void SpawnLevel()//todo different layouts and different number of spaceships at higher levels
    {
        gameManager.ShowLevel(level);
        EnemyController tmp;
        //Debug.Log("level is " + level);

        if (level <= 4)
        {
            int numCols = 9;
            int numRows = 3;
            float xSpace = 3.0f;
            float zSpace = 2.5f;
            float xOffset = -12;
            float zOffset = 10;
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    GameObject enemy = gameManager.enemyPool.Get();//Instantiate(enemyPrefab, new Vector3(j * xSpace + xOffset, 0, zSpace * i + zOffset), enemyPrefab.transform.rotation);
                    enemy.transform.position = new Vector3(j * xSpace + xOffset, 0, zSpace * i + zOffset);
                    tmp = enemy.GetComponent<EnemyController>();
                    tmp.SetRate(1f / level);
                    tmp.StartMoving();
                    //enemy.setRate
                }

            }
        }
        else if (level == 5)
        {
            //show boss 1
            Instantiate(boss1, new Vector3(0, 0, 0), boss1.transform.rotation);
            audioManager.MusicToBoss();
            //audioSource.clip = bossMusic;
            //audioSource.Play();

        }
        else if (isShort)
        {
            gameManager.GameOver(true);
        }
        else if (level < 10)
        {
            if (level == 6)
            {
                audioManager.MusicToRegular();
                //audioSource.clip = mainMusic;
                //audioSource.Play();
            }

            for (int i = 0; i < 5; i++)
            {
                GameObject enemy = gameManager.enemy2Pool.Get();//Instantiate(enemyPrefab, new Vector3(j * xSpace + xOffset, 0, zSpace * i + zOffset), enemyPrefab.transform.rotation);
                enemy.transform.position = new Vector3(-20 + i * 3.0f  , 0,  5);
                tmp = enemy.GetComponent<EnemyController>();
                tmp.SetRate(1f / (level - 5));
                tmp.StartMoving();

            }

            for (int i = 0; i < 5; i++)
            {
                GameObject enemy = gameManager.enemy2Pool.Get();//Instantiate(enemyPrefab, new Vector3(j * xSpace + xOffset, 0, zSpace * i + zOffset), enemyPrefab.transform.rotation);
                enemy.transform.position = new Vector3(-1*(-20 + i * 3.0f +  2), 0, 10);
                enemy.transform.Rotate(0, 180, 0);
                tmp = enemy.GetComponent<EnemyController>();
                tmp.SetRate(1f / (level - 5));
                tmp.StartMoving();
            }
        }
        else if (level == 10)
        {
            //show big boss
            Instantiate(boss2, new Vector3(0, 0, 0), boss1.transform.rotation);
            audioManager.MusicToBoss();
            //audioSource.clip = bossMusic;
            //audioSource.Play();
        }
        else
        {
            gameManager.GameOver(true);
        }
       // Debug.Log("rate: " + 1f / level);
        //TODO - add max level to high score
        //TODO - music for the other scenes, and the you won/game over
    }


}
