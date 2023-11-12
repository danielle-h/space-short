using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
   [SerializeField] private GameObject titleScreen;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject levelContainer;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI livesText;

    [SerializeField] private GameObject highScoreScreen;
    [SerializeField] private Camera mainCamera;
    //private int highScoreTime = 3;//seconds
    [SerializeField] private TMP_InputField highScoreName;

    public GameObject laserPrefab;
    public GameObject powerLaserPrefab;
    public GameObject enemyLaserPrefab;
    public GameObject enemyPrefab;
    public GameObject bossLaserPrefab;
    public ParticleSystem explosionEffect;
    public GameObject enemy2Prefab;

    public ObjectPool<GameObject> playerLaserPool;
    public ObjectPool<GameObject> playerPowerupLaserPool;
    public ObjectPool<GameObject> enemyLaserPool;
    public ObjectPool<GameObject> enemyPool;
    public ObjectPool<ParticleSystem> explosionPool;
    public ObjectPool<GameObject> bossLaserPool;
    public ObjectPool<GameObject> enemy2Pool;

 



    public bool gameOn;
    int score;
    int highScore;
    int lives = 4;
    HighScoreManager highScoreManager;
    string playerName;
    int lifeScoreMultiplier = 100;

    private int levelTime = 3;//seconds

    private int maxLives = 10;

    AudioManager audioManager;

    
    GameObject CreatePlayerLaser()
    {
        GameObject laser = Instantiate(laserPrefab);
        return laser;
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
         playerLaserPool = new ObjectPool<GameObject>(
             createFunc: CreatePlayerLaser, 
             actionOnGet: (obj) => obj.SetActive(true), 
             actionOnRelease: (obj) => obj.SetActive(false), 
             actionOnDestroy: (obj) => Destroy(obj), 
             defaultCapacity:30,
             collectionCheck: true,  maxSize: 100);
        playerPowerupLaserPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(powerLaserPrefab),
             actionOnGet: (obj) => obj.SetActive(true),
             actionOnRelease: (obj) => obj.SetActive(false),
             actionOnDestroy: (obj) => Destroy(obj),
             defaultCapacity: 30,
             collectionCheck: true, maxSize: 100);
        enemyLaserPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(enemyLaserPrefab),
             actionOnGet: (obj) => obj.SetActive(true),
             actionOnRelease: (obj) => obj.SetActive(false),
             actionOnDestroy: (obj) => Destroy(obj),
             defaultCapacity: 30,
             collectionCheck: true, maxSize: 100);
        bossLaserPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(bossLaserPrefab),
             actionOnGet: (obj) => obj.SetActive(true),
             actionOnRelease: (obj) => obj.SetActive(false),
             actionOnDestroy: (obj) => Destroy(obj),
             defaultCapacity: 100,
             collectionCheck: true, maxSize: 200);
        enemyPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(enemyPrefab),
             actionOnGet: (obj) => obj.SetActive(true),
             actionOnRelease: (obj) => obj.SetActive(false),
             actionOnDestroy: (obj) => Destroy(obj),
             defaultCapacity: 30,
             collectionCheck: true, maxSize: 100);
        enemy2Pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(enemy2Prefab),
             actionOnGet: (obj) => obj.SetActive(true),
             actionOnRelease: (obj) => obj.SetActive(false),
             actionOnDestroy: (obj) => Destroy(obj),
             defaultCapacity: 30,
             collectionCheck: true, maxSize: 100);
        explosionPool = new ObjectPool<ParticleSystem>(
            createFunc: () => Instantiate(explosionEffect),
             actionOnGet: (obj) => { obj.Play();
                 audioManager.PlaySFX(audioManager.enemyExplosion);
             },
             actionOnRelease: null,
             actionOnDestroy: (obj) => Destroy(obj),
             defaultCapacity: 30,
             collectionCheck: true, maxSize: 100);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        ResetGame();
        //highScore = PlayerPrefs.GetInt(GlobalConstants.HIGH_SCORE_KEY, 0);
        DecrementLives();
        //highScore = 0;
        highScoreManager  = HighScoreManager.Instance;
        highScoreManager.LoadHighScores();//TODO sometimes is null, need to cancel the singleton, there is no point in it.
        highScore = highScoreManager.GetLowestHighScore();
        
    }

    private void OnDestroy()
    {
        playerLaserPool.Dispose();
    }


    private void ResetGame()
    {
        gameOn = true;
        score = 0;
        scoreText.SetText("Score: " + score);
        currentLevel.SetText("Level: 1");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void GameOver(bool won = false)
    {
        gameOn = false;
        if (won)
        {
            //add to score
            score += lifeScoreMultiplier * lives;
            if (score > highScore)
            {
                PlayerPrefs.SetInt(GlobalConstants.HIGH_SCORE_KEY, score);
                PlayerPrefs.SetInt(GlobalConstants.IS_HIGH_SCORE, 1);

            }
            else
            {
                PlayerPrefs.SetInt(GlobalConstants.IS_HIGH_SCORE, 0);

            }
            SceneManager.LoadScene(GlobalConstants.YOU_WON_INDEX);

        }
        else if (score > highScore)//go to high score scene
        {
            PlayerPrefs.SetInt(GlobalConstants.HIGH_SCORE_KEY, score);
            SceneManager.LoadScene(GlobalConstants.NEW_HIGHSCORE_SCENE_INDEX);


        }
        else //Show game over
        {

            SetTitleScreen();
            //SceneManager.LoadScene(GlobalConstants.START_SCENE_INDEX);

        }

    }

    public void SetTitleScreen()//TODO wrong name, do we even need htis ?
    {
        titleScreen.gameObject.SetActive(true);// TODO restart button should have permanent background for mobile, not only on hover.
        levelContainer.gameObject.SetActive(false);
        highScoreScreen.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        StartCoroutine(StopHighScore());

    }

    IEnumerator StopHighScore()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(GlobalConstants.START_SCENE_INDEX);
        //SetTitleScreen();

        //powerupEffect.Stop();
    }

    public void RestartGame() //TODO Do we need this
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResetGame();
    }

    public void ShowLevel(int levelNumber)
    {
        levelText.SetText("Level " + levelNumber);
        levelContainer.gameObject.SetActive(true);
        levelContainer.GetComponent<Animator>().SetTrigger("DoAnimation");
        StartCoroutine(LevelTime());
        currentLevel.SetText("Level: " + levelNumber);
        //save current level and score
    }

    public void AddScore(int newScore)
    {
        score += newScore;
        scoreText.SetText("Score: " + score);
        //Debug.Log("score: " + score);
    }

    private void OnApplicationPause(bool pause)
    {
        //TODO take care of this case
    }

    

    public void DecrementLives()
    {
        lives--;
        //Debug.Log("lives: " + lives);
        livesText.SetText("Lives: " + lives);
        if (lives == 0)
        {
            GameOver();
        }
    }

    public void IncrementLives()
    {
        lives++;
        if (lives > maxLives)
        {
            lives = 10;
        }
        livesText.SetText("Lives: " + lives);
    }


    // Coroutine to count down powerup duration
    IEnumerator LevelTime()
    {
        yield return new WaitForSeconds(levelTime);
        levelContainer.gameObject.SetActive(false);
    }

}
