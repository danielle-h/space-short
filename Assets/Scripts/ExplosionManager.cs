using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{

    private GameManager gameManager;
    

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        //audioSource = GetComponent<AudioSource>();
        //Debug.Log("got audio");
        //audioSource.Play();

    }

    /*private void OnEnable()
    {
        //audioSource = GetComponent<AudioSource>();
       // Debug.Log(audioSource);
        //audioSource.Play();
        //Debug.Log("enabled");
    }*/


    public void OnParticleSystemStopped()
    {
        if (gameObject.name.StartsWith("Explosion"))
        {
            gameManager.explosionPool.Release(GetComponent<ParticleSystem>());
            //audioSource.Stop();
        }
    }

    
}
