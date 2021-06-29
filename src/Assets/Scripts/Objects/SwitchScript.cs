using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    public bool active = false;
    public GameObject dust;
    public bool isTorch = false;
    public GameObject player;
    public GameObject fire;
    // Start is called before the first frame update

    void Start()
    {
        active = false;
        if (isTorch) fire.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && isTorch == false)
        {
            if (player.GetComponent<PlayerMovement>().canMove == true)
            {
                Instantiate(dust, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                active = true;
                gameObject.SetActive(false);
            }
        }
        if (collision.gameObject.CompareTag("fireball") && isTorch == true)
        {
            if (player.GetComponent<PlayerMovement>().canMove == true)
            {
                Instantiate(dust, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                active = true;
                fire.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

}
