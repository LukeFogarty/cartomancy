using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float MoveSpeed = 8;
    public AudioSource audioSource;
    public AudioClip[] clips;

    public Transform startRoom;
    public Transform startPoint;
    public Transform mapMarker;
    readonly int MaxDist = 55;
    readonly int MinDist = 1;
    int wait = 0;
    int pause = 0;
    int attack = 0;
    bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        startPoint.parent = startRoom;
        startPoint.position = new Vector3(startRoom.position.x+5, startRoom.position.y, startRoom.position.z+5);
        startPoint.eulerAngles = new Vector3(startRoom.eulerAngles.x, Random.Range(0,359), startRoom.eulerAngles.z);
    }

    void FixedUpdate()
    {
        transform.LookAt(player);
        if (player.GetComponent<PlayerMovement>().canMove == true && active == true)
        {
            //what to do if within distance
            if (Vector3.Distance(transform.position, player.position) >= MinDist && Vector3.Distance(transform.position, player.position) <= MaxDist)
            {
                mapMarker.GetComponent<MeshRenderer>().enabled = true;
                mapMarker.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                int layerMask = 1 << 10;
                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.forward, out _, Vector3.Distance(transform.position, player.position), layerMask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * Vector3.Distance(transform.position, player.position), Color.white);
                    if (player.GetComponent<PlayerMovement>().canMove == true) wait++;
                }
                //if the player is not in safe space
                else
                {
                    //move towards player
                    if (transform.position.y < 4) transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
                    transform.position += transform.forward * MoveSpeed * Time.deltaTime;
                    wait = 0;
                }
            }
            //attack
            if (Vector3.Distance(transform.position, player.position) <= MinDist + 4 && wait <= 0 && pause<=0)
            {
                attack++;
                if (attack >= 13 && player.GetComponent<PlayerMovement>().canMove == true)
                {
                    audioSource.clip = clips[Random.Range(0,clips.Length)];
                    audioSource.Play();
                    player.GetComponent<PlayerMovement>().PlayerHurt();
                    pause = 30;
                    active = false;
                }
            }
            else
            {
                attack = 0;
            }
        }
        //if the player dies
        if (player.GetComponent<PlayerMovement>().respawn>0)
        {
            pause = 10;
            active = false;
        }
        //if outside range
        if (Vector3.Distance(transform.position, player.position) > MaxDist)
        {
            wait++;
        }
        //keeping in place when not chasing and changing the map
        if (transform.position.y <= -5 && player.GetComponent<PlayerMovement>().canMove == false)
        {
            transform.position = new Vector3(startPoint.transform.position.x, startPoint.transform.position.y - 7, startPoint.transform.position.z);
            mapMarker.GetComponent<MeshRenderer>().enabled = false;
            mapMarker.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
        //pause countdown after an attack for sinking into the ground
        if (pause > 0)
        {
            pause--;
        }
        if (pause == 1) wait = 100;

        //teleporting back to start after waiting
        if (wait >= 100)
        {
            if (transform.position.y >-5) transform.position = new Vector3(transform.position.x, transform.position.y-0.4f, transform.position.z);
            if (transform.position.y <= -5)
            { 
                transform.position = new Vector3(startPoint.transform.position.x, startPoint.transform.position.y - 7, startPoint.transform.position.z);
                wait = 0;
                pause = 0;
                active = true;
            }
        }
    }
}
