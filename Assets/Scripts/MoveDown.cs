using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    public float speed = 3;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
        if (this.gameObject.CompareTag("Background"))//move background
        {
            if (transform.position.z  - startPosition.z < -50)
            {
                transform.position = startPosition;
            }
        }
        else
        {
            if (transform.position.z < -20)
            {
                Destroy(gameObject);
            }
        }
        
    }
}
