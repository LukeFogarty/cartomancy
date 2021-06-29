using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int moveSpeed = 0;
    public GameObject explosion;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerMovement>().canMove == true) 
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (transform.position.x >150 || transform.position.x < -150 || transform.position.z > 150 || transform.position.z < -150)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (player.GetComponent<PlayerMovement>().canMove == true)
            {
                player.GetComponent<PlayerMovement>().PlayerHurt();
                Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                Destroy(gameObject);

            }
        }
        if(other.gameObject.CompareTag("wall"))
        {
            if (player.GetComponent<PlayerMovement>().canMove == true)
            {
                Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
