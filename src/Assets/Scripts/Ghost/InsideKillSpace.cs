using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideKillSpace : MonoBehaviour
{
    public GameObject dust;
    public GameObject player;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("enemy") && other.gameObject.transform.position.y >2)
        {
            if (player.GetComponent<PlayerMovement>().canMove == true)
            {
                Instantiate(dust, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), Quaternion.identity);
                Destroy(other.gameObject);
            }
            
        }
        
    }
}
