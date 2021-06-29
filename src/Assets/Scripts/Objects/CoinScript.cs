using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public GameObject dust;
    private int spin = 0;

    // Update is called once per frame
    void Update()
    {
        spin++;
        if (spin >= 360) spin = 0;
        transform.eulerAngles = new Vector3(0, spin, 0);
        transform.position = new Vector3(transform.position.x, 2, transform.position.z);
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().canMove == true)
            {
                Instantiate(dust, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                collision.gameObject.GetComponent<PlayerMovement>().coinsCollected++;
                gameObject.SetActive(false);
            }
        }
    }
}
