using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFire : MonoBehaviour
{
    public GameObject fireball;
    public GameObject player;
    int fired = 0;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerMovement>().canMove == true)
        {
            
            if (fired > 0)
            {
                fired++;
            }
            if (fired == 10)
            {
                GameObject fire = Instantiate(fireball, new Vector3(transform.position.x, 4, transform.position.z), transform.rotation);
                fire.GetComponent<Fireball>().player = player;
            }
            if (fired > 300)
            {
                fired = 0;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && fired <= 0)
        {
            if (player.GetComponent<PlayerMovement>().canMove == true)
            {
                fired++;
            }
        }

    }
}
