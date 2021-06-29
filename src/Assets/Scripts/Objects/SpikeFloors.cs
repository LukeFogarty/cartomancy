using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeFloors : MonoBehaviour
{
    bool active = false;
    int counter = 0;
    int trapY = -6;
    public GameObject player;
    public AudioSource audioSource;
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, trapY, transform.position.z);
        if (active == true && player.GetComponent<MapController>().mapOpen == false)
        {
            counter++;

            if (counter > 30)
            {
                if (trapY < 0) trapY+=2;
            }
            if (counter == 32)
            {
                audioSource.Play();
            }
            if (counter > 60)
            {
                active = false; counter = 0;
            }
        }
        if (active == false)
        {
            if (trapY > -6) trapY-=2;
        }
    }
     
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (player.GetComponent<PlayerMovement>().canMove == true)
            {
                if (active == false && trapY <-5) active = true;
                if (trapY > -5 && trapY < 0 && active ==true) { player.GetComponent<PlayerMovement>().PlayerHurt(); }
            }
        }

    }
}
