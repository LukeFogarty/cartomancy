using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScript : MonoBehaviour
{
    public bool unlocked = false;
    public GameObject dust;
    public GameObject shield;
    public AudioSource audioSource;
    private float spin = 0;

    // Update is called once per frame
    void Update()
    {
        spin+=0.5f;
        if (spin >= 360) spin = 0;
        transform.eulerAngles = new Vector3(0, spin, 0);
        transform.position = new Vector3(transform.position.x, 8, transform.position.z);
        shield.transform.position = transform.position;
        if (unlocked == false) UnlockedNow();
        if (unlocked == true && shield.activeSelf == true)
        {
            shield.SetActive(false);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().canMove == true)
            {
                if (unlocked == true)
                {
                    audioSource.PlayDelayed(0.2f);
                    collision.gameObject.GetComponent<PlayerMovement>().gotCrystal = true;
                    
                }
                if (unlocked == false)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().PlayerHurt();
                }
            }
        }
    }
    public void UnlockedNow()
    {
        GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("switch");
        bool locked = false;
        foreach (GameObject go in gameObjectArray)
        {
            if (go.activeSelf == true)
            {
                locked = true;
            }
        }
        if (locked == false) 
        {
            Instantiate(dust, new Vector3(shield.transform.position.x, shield.transform.position.y, shield.transform.position.z), Quaternion.identity);
            unlocked = true; 
        }
    }

}
