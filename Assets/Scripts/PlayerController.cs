using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    //movement
    private readonly float speed = 10;
    // private float horizontalInput;
    //private float verticalInput;
    private float xRange;
    private float zRange;

    //game parameters
    public bool gameOver = false;
    private GameManager gameManager;

    //public ParticleSystem powerupEffect;
    //shooting
    public GameObject laserPrefab;


    //powerup variables
    private readonly float powerupTime = 5;
    public bool hasPowerup = false;
    public GameObject powerLaserPrefab;
    DateTime startPowerup;
    float lastShot = 0.0f;
    float fireRate = 0.25f;
    float mobileFireRate = 0.5f;


    //shield pwerup variables TODO amybe create powerup class?
    public GameObject shield;
    DateTime startShield;
    private float shieldTime = 5;
    private bool hasShield = false;

    //Explosion
    public ParticleSystem explosion;

    private Vector3 startPos;
    public bool impervious = false;//TODO change to private
    private int imperviousTime = 3;
    [SerializeField] GameObject spaceship;

    private Vector2 movement = new Vector2(0, 0);

    //SFX
    AudioManager audioManager;




    // Start is called before the first frame update
    void Start()
    {
        xRange = GlobalConstants.xPlayerRange;
        zRange = GlobalConstants.zPlayerRange;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        shield.SetActive(false);
        startPos = transform.position;
        movement = new Vector2(0, 0);
        StartImpervious();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }

    public void OnShoot()
    {
        OnShoot(false);
    }


    public void OnShoot(bool isMobile = false)
    {

        //Debug.Log("onshoot");
        if (Time.time > lastShot + (isMobile ? mobileFireRate : fireRate))
        {
            //Debug.Log(Time.time);
            lastShot = Time.time;
            if (hasPowerup)
            {
                GameObject laser = gameManager.playerPowerupLaserPool.Get();
                laser.transform.position = transform.position;
                //Debug.Log(laser.name);

            }
            else
            {

                GameObject laser = gameManager.playerLaserPool.Get();
                laser.transform.position = transform.position;
                //Debug.Log(laser.name);



            }
            //playerAudio.PlayOneShot(laserSound, 1.0f);
            audioManager.PlaySFX(audioManager.playerShoot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //MovePlayer();
        String scheme = GetComponent<PlayerInput>().currentControlScheme;
        Debug.Log(movement.x + " " + movement.y);
        if (scheme == "Keyboard")
        {
            transform.Translate((Vector3.right * movement[0] + Vector3.forward * movement[1]) * Time.deltaTime * speed);
        }
        else
        {
            if (movement.x != 0 || movement.y != 0)
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(movement);
                position.y = transform.position.y;
                transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * speed);
            }

            OnShoot(true);

        }

        ConstrainPlayerPosition();
        //update powerups
        if (hasShield)
        {
            StopShield();
        }
        if (hasPowerup)
        {
            StopPowerup();
        }
        //TODO delete this: shield for testing
        /*if (Input.GetKeyUp(KeyCode.S) && Input.GetKeyUp(KeyCode.LeftShift))//TODO add to actions
        {
            Debug.Log("shilding");
            shieldTime = 30;
            ActivateShield();
        }*/
    }

    void StartImpervious()
    {
        movement = new Vector2(0, 0);
        spaceship.SetActive(true);// deactivating the player causes input to not work when reactivated
        transform.position = startPos;
        impervious = true;
        StartCoroutine(Blink(imperviousTime));
    }


    void CancelImpervious()
    {
        impervious = false;
    }


    void OnMove(InputValue value)
    {
        //move on user input
        movement = value.Get<Vector2>();
        //Debug.Log(movement);

    }

    void ConstrainPlayerPosition()
    {
        //constrain movement
        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        if (transform.position.z > zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRange);
        }
        if (transform.position.z < -zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zRange);
        }
    }

    //destroy on enemy collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyLaser") || other.gameObject.CompareTag("Boss"))
        {
            if (!hasShield && !impervious)
            {
                gameManager.DecrementLives();
                audioManager.PlaySFX(audioManager.playerExplosion);

                Instantiate(explosion, transform.position, explosion.transform.rotation);
                if (!gameManager.gameOn)
                {
                    //KillPlayer();
                    Destroy(gameObject);
                    gameOver = true;
                }
                else
                {
                    //deactivating spaceship instead of player to keep input working
                    spaceship.SetActive(false);

                    Invoke("StartImpervious", 1); // StartImpervious();
                }
            }
        }
        else if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            startPowerup = DateTime.Now;
            audioManager.PlaySFX(audioManager.powerup);

        }
        else if (other.gameObject.CompareTag("ShieldPowerup"))
        {
            Destroy(other.gameObject);
            ActivateShield();
            audioManager.PlaySFX(audioManager.powerup);


        }
        else if (other.gameObject.CompareTag("HeartPowerup"))
        {
            Destroy(other.gameObject);
            // Debug.Log("got heart");
            gameManager.IncrementLives();
            audioManager.PlaySFX(audioManager.powerup);

        }

    }

    private IEnumerator Blink(double waitTime)
    {

        double endTime = Time.time + waitTime;
        while (Time.time < endTime)
        {
            spaceship.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            spaceship.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
        impervious = false;


    }

    void ActivateShield()
    {
        hasShield = true;
        shield.SetActive(true);
        shield.GetComponent<Animator>().Play("ShieldAnimation");
        startShield = DateTime.Now;
    }

    void StopShield()
    {
        DateTime tmp = DateTime.Now;

        if (tmp >= startShield.AddSeconds(shieldTime))
        {
            hasShield = false;
            shield.SetActive(false);
            shieldTime = 5;
        }
    }

    void StopPowerup()
    {
        DateTime tmp = DateTime.Now;
        if (tmp >= startPowerup.AddSeconds(powerupTime))
        {
            hasPowerup = false;
        }
    }

}