using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float speed = 10;
    int numberOfCollisions = 0;
    private GameManager gameManager;
    private LaserType laserType = LaserType.PLAYER;

    enum LaserType
    {
        PLAYER,
        POWER,
        ENEMY,
        BOSS1,
        ERROR
    };

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        //find which laser we are
        if (gameObject.name.StartsWith("Laser"))
        {
            laserType = LaserType.PLAYER;
        }
        else if (gameObject.name.StartsWith("Power"))
        {

            laserType = LaserType.POWER;
        }
        else if (gameObject.name.StartsWith("Enemy"))//enemy laser
        {
            laserType = LaserType.ENEMY;
        }
        else if (gameObject.name.StartsWith("Boss"))//boss laser
        {
            //Debug.Log("boss type");
            laserType = LaserType.BOSS1;
        }
        else
        {
            laserType = LaserType.ERROR;
        }


    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.up*Time.deltaTime*speed);
        transform.Translate(transform.forward * Time.deltaTime * speed);
        if (transform.position.z > GlobalConstants.zPlayerRange + GlobalConstants.rangeBuffer || transform.position.z < -GlobalConstants.zPlayerRange - GlobalConstants.rangeBuffer || transform.position.x < -40 || transform.position.x > 40) 
        {
            ReleaseLaser();
            //gameManager.playerLaserPool.Release(gameObject);
        }
    }

    private void ReleaseLaser()
    {
        switch (laserType)
        {
            case LaserType.PLAYER:
                gameManager.playerLaserPool.Release(gameObject);

                break;
            case LaserType.POWER:
                gameManager.playerPowerupLaserPool.Release(gameObject);

                break;
            case LaserType.ENEMY:
                transform.rotation = Quaternion.Euler(90,0,0);
                gameManager.enemyLaserPool.Release(gameObject);


                break;
            case LaserType.BOSS1:
                //Debug.Log("destroying");
                transform.rotation = Quaternion.identity;
                gameManager.bossLaserPool.Release(gameObject);

                break;
            case LaserType.ERROR:
                Debug.LogError("Cannot release " + gameObject.name);
                break;

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("on trigger enter " + gameManager.gameOn);
        if (gameManager != null && gameManager.gameOn)
        {
            //Debug.Log("game is on " + other.gameObject.tag + " " + this.gameObject.tag);

            if (other.gameObject.CompareTag("Enemy") && this.gameObject.CompareTag("Laser"))
            {
                numberOfCollisions++;
                if (laserType!=LaserType.POWER || (laserType==LaserType.POWER && numberOfCollisions > 1))
                {
                    //Destroy(gameObject);
                    ReleaseLaser();
                    numberOfCollisions = 0;
                }
                other.GetComponent<EnemyController>().KillEnemy();

            }

            if (other.gameObject.CompareTag("Shield") && this.gameObject.CompareTag("EnemyLaser"))
            {
                Debug.Log("laser on sheild");
                ReleaseLaser();

            }
        }
    }
}
