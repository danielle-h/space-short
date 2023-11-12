using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float stepSize = 1;
    [SerializeField]private int movementCounter = 0;
    private float rate = 1;//seconds
    public GameObject laserPrefab;
    public ParticleSystem explosion;
    public int shootProbability = 5;

    public int price = 10;
    public int powerupProbability = 2;

    public enum EnemyType
    {
        TYPE1,
        TYPE2,

    };

    public EnemyType type = EnemyType.TYPE1;


    private GameManager gameManager;
    //private SpawnManager spawnManager;
    public GameObject[] powerups;
    // Start is called before the first frame update
    void Start()
    {
        //jerky movement of space invaders
        //spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        //rate = 1f / spawnManager.level;
        
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        //


    }

    public void SetRate(float rate)
    {
        this.rate = rate;

    }

    public void StartMoving()
    {
       
            InvokeRepeating("MoveStep", 0, rate);
        

    }

    // Update is called once per frame
    void Update()
    {
                //if enemy is out of frame, destroy
        if (transform.position.z < -10 || transform.position.x <-30 || transform.position.x > 30)
        {
            removeEnemy();
        }
    }
    //one step in jerky movement of space invader
    void MoveStep()
    {
        Vector3 movementDirection;

        //choose direction according to current state
        if (type == EnemyType.TYPE1)
        {
            switch (movementCounter)
            {
                case 0:
                    movementDirection = new Vector3(0, 0, -stepSize);
                    break;
                case 1:
                    movementDirection = new Vector3(0, -stepSize, 0);

                    break;
                case 2:
                    movementDirection = new Vector3(0, 0, -stepSize);
                    break;
                default:
                    movementDirection = new Vector3(0, stepSize, 0);
                    break;
            }
        }
        else
        {
            movementDirection =  new Vector3(0, 0, stepSize);
        }
       //move in directions chosen
        transform.Translate(movementDirection);
        //Enemy should shoot
        int rand = Random.Range(0, 100);
        if (rand <= shootProbability) // TODO this number should also change with levels, when enemies move faster there are too many lasers! 
        {
            //Instantiate(laserPrefab, transform.position, laserPrefab.transform.rotation);
            GameObject laser = gameManager.enemyLaserPool.Get();
            laser.transform.position = transform.position;//TODO shoot at angle if type 2, shoot two lasers
            if (type == EnemyType.TYPE2)
            {
                laser.transform.rotation = Quaternion.Euler(0, 10F, 0) * laser.transform.rotation;
                laser = gameManager.enemyLaserPool.Get();
                laser.transform.position = transform.position;
                laser.transform.rotation = Quaternion.Euler(0, -10F, 0) * laser.transform.rotation;
            }
        }
        //update movement counter
        movementCounter++;
        if (movementCounter > 3)
        {
            movementCounter = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.CompareTag("Shield"))
        {
            Debug.Log("Enemy collided with shield");
            KillEnemy();
        }
    }

    public void KillEnemy()
    {
        //ParticleSystem ps =  Instantiate(explosion, transform.position, explosion.transform.rotation);
        //create  explosion
        ParticleSystem ps = gameManager.explosionPool.Get();
        ps.transform.position = transform.position;
        //update score
        gameManager.AddScore(price);
        //drop powerup
        int powerupType = Random.Range(0, 2);
        int rand = Random.Range(0, 10);
        if (rand < powerupProbability) //todo this number needs ot change as levels get more difficult
        {
            Debug.Log("creating powerup");
            Instantiate(powerups[powerupType], transform.position, powerups[powerupType].transform.rotation);
        }
        removeEnemy();
       // Destroy(gameObject);
    }

    private void ResetEnemy()
    {
        CancelInvoke();
        movementCounter = 0;
    }

    private void removeEnemy()
    {
        //destroy
        ResetEnemy();
        if (type == EnemyType.TYPE1)
        {
            gameManager.enemyPool.Release(gameObject);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
            gameManager.enemy2Pool.Release(gameObject);
        }
    }

    /*private void OnDestroy()
    {
        Instantiate(explosion, transform.position, explosion.transform.rotation);
    }*/


}
