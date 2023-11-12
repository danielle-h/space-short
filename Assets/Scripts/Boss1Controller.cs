using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    private int life = 100;
    private int maxLife = 100;
    private Animator[] animators;
    private float prevTime;//for debug
    private float currentTime = 0;
    float speed = 0.5F;
     float xScale = 12;
     float yScale = 8;
    private float hitTime = 0;
    private bool isDead = false;
    //movement
    float t = 0;

    //shooting
    //public GameObject bossLaser;
    int numMissiles = 8;
    float shootTime = 1.5F;
    float offset = 0;

    private GameManager gameManager;
    public HealthbarBehaviour healthbar;
    public GameObject powerup;
    private double heartProbability = 0.1;
    public int numBosses = 1;
    public GameObject boss1;
    bool randomMovement = false;
    Vector3 randomDir;
    bool justChangedX = false;
    bool justChangedZ = false;

    //SFX
    AudioManager audioManager;






    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        animators = new Animator[numBosses];
        for (int i = 0; i < numBosses; i++) {
            animators[i] = GetComponent<Transform>().GetChild(i).GetComponent<Animator>();
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();


        //animator.SetTrigger("triggerTaunt");



        life = 100;
        prevTime = Time.time + offset;
        healthbar.SetHealth(life, maxLife);
    }
    //TODO need meteor

    // Update is called once per frame
    void Update()
    {
        GameObject tmp;
        GameObject tmpPower;
        float rand;

         currentTime = Time.time;
        /*if (currentTime - hitTime > tauntTime)
        {
            animator.SetTrigger("triggerTaunt");
            hitTime = currentTime;
        }*/
        if (currentTime - prevTime > shootTime && !isDead)
        {
            prevTime = currentTime;
            //AnimatorClipInfo[] stateInfo = animators[0].GetCurrentAnimatorClipInfo(0);
            //Debug.Log("Current Animation: " + stateInfo[0].clip.name);
            for (int i = 0; i < numMissiles; i++)
            {
                //Debug.Log("creating boss laser");
                tmp = gameManager.bossLaserPool.Get();
                tmp.transform.position = transform.position;
                tmp.transform.rotation = Quaternion.Euler(0, i * 22.5F, 0) * tmp.transform.rotation;
                //tmp = Instantiate(bossLaser, transform.position, Quaternion.Euler(0, i*22.5F,0) * bossLaser.transform.rotation);
                //Debug.Log("current clone: " + tmp.transform.rotation.x + " " + tmp.transform.rotation.y + " " + tmp.transform.rotation.z);
                //tmp.GetComponent<BossLaserScript>().SetDir(i * 45);

            }
            rand = Random.Range(0.0f, 1.0f);
            //Debug.Log(rand);
            if (rand < heartProbability)
            {
                tmpPower = Instantiate(powerup, transform.position, powerup.transform.rotation);
            }


        }
        //move
        if (!isDead)
        {
            if (!randomMovement)
            {
                transform.position = new Vector3(Mathf.Cos(t) * xScale, 0, yScale * Mathf.Sin(2 * t) / 2);
                t += Time.deltaTime * speed;
            }
            else
            {
                transform.Translate(randomDir * Time.deltaTime * speed * 10);
                if ((transform.position.x < -xScale || transform.position.x > xScale) && !justChangedX)
                {
                    randomDir.x = -randomDir.x;
                    justChangedX = true;
                }
                else
                {
                    justChangedX = false;
                }
                if ((transform.position.z < -yScale || transform.position.z > yScale) && !justChangedZ)
                {
                    randomDir.z = -randomDir.z;
                    justChangedZ = true;
                }
                else
                {
                    justChangedZ = false;
                }
            }
        }     


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            hitTime = Time.time;
            
            life = life - 10;
            //Debug.Log("life is now " + life);
            healthbar.SetHealth(life, maxLife);
            audioManager.PlaySFX(audioManager.bossHit);


            if (life == 0)
            {
                isDead = true;
                gameManager.AddScore(100);
                if (numBosses == 1)
                {
                    animators[0].SetTrigger("triggerDie");
                    StartCoroutine(KillBoss());
                }
                else
                {
                    //create 3 bosses in place of this one, with lifebars
                    for (int i = 0; i < numBosses; i++)
                    {
                        Vector3 position = GetComponent<Transform>().GetChild(i).position;
                        //Debug.Log(position);
                        GameObject tmp = Instantiate(boss1,position , boss1.transform.rotation);
                        Boss1Controller tmpController = tmp.GetComponent<Boss1Controller>();
                        tmpController.randomMovement = true;
                        Vector2 dir2d = Random.insideUnitCircle.normalized;
                        tmpController.randomDir = new Vector3(dir2d.x,0,dir2d.y);
                        tmpController.shootTime = shootTime * 3;
                        tmpController.offset = shootTime* i;
                    }
                    Destroy(gameObject);

                }
            }
            else if (life >0)
            {
                for (int i = 0; i < numBosses; i++)
                {
                    animators[i].SetTrigger("triggerHit");
                }
            }
            Destroy(other.gameObject);
        }
    }

    private IEnumerator KillBoss()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
