using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MapController : MonoBehaviour
{
    public GameObject map;
    public PlayerMovement player;
    public bool mapOpen = false;
    public Transform infoBox;
    public Transform exitBox;
    public Transform exitSelect;
    public Transform fadeScreen;

    private float h;
    private float v;
    public AudioSource audioSource;
    public AudioClip[] clips;
    private bool turning = false;
    private int command = 0;
    private int turner = 0;
    public int reset = 0;

    private bool turnt = false;
    public Camera cam1;
    public Camera cam2;
    Vector3 matchAngle;
    Vector3 STARTP;
    int cooldown = 0;
    bool exit = false;
    bool exitQ = false;
    bool confirmexit = false;
    int info = -130;
    int exitY = -130;
    Vector3 exitPosition;
    float fader = 0;
    int camZ = 0;
    bool closing = false;

    // Start is called before the first frame update
    void Start()
    {
        cam1.enabled = true;
        cam2.enabled = false;
        STARTP = map.transform.position;
        exitPosition = exitSelect.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (reset == 0)
        {
            if (fader < 1 && mapOpen == false)
            {
                fader += 0.05f;
                fadeScreen.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", fader);
            }
            if (fader > 0 && mapOpen == true)
            {
                if (camZ <= 0) fader -= 0.05f;
                fadeScreen.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", fader);
            }
            if (fader <= 0f && closing == false)
            {
                cam1.enabled = false;
                cam2.enabled = true;
                if (camZ < 90) camZ += 15;
            }
            if (fader >= 1f && closing == false)
            {
                cam1.enabled = true;
                cam2.enabled = false;
                if (camZ > 0) camZ -= 15;
            }
        }
        cam2.transform.eulerAngles = new Vector3(camZ, 0, 0);

        //finish level
        if (player.GetComponent<PlayerMovement>().gotCrystal == true)
        {
            reset++;
            if (reset > 200)
            {
                fader -= 0.025f;
                fadeScreen.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", fader);
            }
            if (reset > 270)
            {
                GameObject manager = GameObject.FindGameObjectWithTag("GameController");
                int currentLevel = manager.GetComponent<GameManager>().lastPicked;
                manager.GetComponent<GameManager>().levels[currentLevel].finished = player.GetComponent<PlayerMovement>().gotCrystal;
                if (manager.GetComponent<GameManager>().levels[currentLevel].coins < player.GetComponent<PlayerMovement>().coinsCollected)
                    manager.GetComponent<GameManager>().levels[currentLevel].coins = player.GetComponent<PlayerMovement>().coinsCollected;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }


        if (player.useMap == true)
        {

            if (Input.GetButtonDown("OpenMap") && player.hurt == false && turning == false && exit == false && cooldown == 0)
            {
                if (mapOpen == false)
                {
                    mapOpen = true;
                    player.canMove = false;
                    audioSource.clip = clips[0];
                    cooldown = 35;    
                    audioSource.Play();
                }
                else
                {
                    if (turner == 0 && cooldown == 0 && command == 0) {
                        if (player.canMove == false && closing == false)
                        {
                            // Bit shift the index of the layer (8) to get floor layer
                            int layerMask = 1 << 8;

                            Ray ray = new Ray(new Vector3(player.transform.position.x, -5, player.transform.position.z), new Vector3(0, 5, 0));
                            if (!Physics.Raycast(ray, out _, Mathf.Infinity, layerMask))
                            {
                                audioSource.clip = clips[5];
                                cooldown = 10;
                                audioSource.Play();
                            }
                            else
                            {
                                closing = true;
                                map.transform.eulerAngles = matchAngle;
                                audioSource.clip = clips[1];
                                cooldown = 5;
                                audioSource.Play();
                            }
                        }
                    }
                }

            }

            if (closing == true)
            {
                if (camZ >= 0) camZ -= 15;
                if (camZ <= 0)
                {
                    mapOpen = false;
                    player.canMove = true;
                    closing = false;
                    cam1.enabled = true;
                    cam2.enabled = false;
                }
            }
            if (mapOpen == true)
            {
                if (cooldown > 0) cooldown--;
                //get input from joystick/arrows/WASD
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");

                if (!turning && cooldown <= 0 && reset == 0 && player.canMove == false && command == 0 && mapOpen == true)
                {
                    if (h > 0)
                    {
                        if (exit == false)
                        {
                            audioSource.clip = clips[2];
                            audioSource.Play();
                            command = 1;
                            turnt = !turnt;
                            matchAngle = new Vector3(map.transform.eulerAngles.x, map.transform.eulerAngles.y + 90, map.transform.eulerAngles.z);
                            turning = true;
                        }
                        if (exitQ == true)
                        {
                            audioSource.clip = clips[3];
                            audioSource.Play();
                            confirmexit = false;
                            cooldown = 35;
                        }

                    }
                    if (h < 0)
                    {
                        if (exit == false)
                        {
                            audioSource.clip = clips[2];
                            audioSource.Play();
                            command = 2;
                            turnt = !turnt;
                            matchAngle = new Vector3(map.transform.eulerAngles.x, map.transform.eulerAngles.y - 90, map.transform.eulerAngles.z);
                            turning = true;
                        }
                        if (exitQ == true)
                        {
                            audioSource.clip = clips[3];
                            audioSource.Play();
                            confirmexit = true;
                            cooldown = 35;
                        }
                    }

                    if (v > 0)
                    {
                        if (exit == false)
                        {
                            audioSource.clip = clips[2];
                            audioSource.Play();
                            command = 3;
                            matchAngle = (!turnt) ?
                            new Vector3(map.transform.eulerAngles.x + 180, map.transform.eulerAngles.y, map.transform.eulerAngles.z) :
                            new Vector3(map.transform.eulerAngles.x, map.transform.eulerAngles.y, map.transform.eulerAngles.z + 180);

                            turning = true;
                        }
                        else
                        {
                            if (exit == true && exitQ == false)
                            {
                                audioSource.clip = clips[3];
                                audioSource.Play();
                                exit = false;
                                cooldown = 35;
                            }
                        }
                    }

                    if (v < 0)
                    {
                        cooldown = 35;
                        audioSource.clip = clips[3];
                        audioSource.Play();
                        exit = true;
                    }

                    if (Input.GetButtonDown("OpenMap") && player.hurt == false && turning == false && exit == true)
                    {
                        if (exitQ == false)
                        {
                            exitQ = true;
                            audioSource.clip = clips[3];
                            audioSource.Play();
                        }
                        else
                        {
                            if (confirmexit == true)
                            {
                                audioSource.clip = clips[4];
                                audioSource.Play();
                                reset++;
                            }
                            if (confirmexit == false)
                            {
                                audioSource.clip = clips[2];
                                audioSource.Play();
                                exitQ = false;
                                exit = false;
                            }
                        }
                    }


                }

                //menu positioning
                if (exit == true)
                {
                    if (info < -100) info += 3;
                }
                else
                {
                    if (info > -130) info -= 3;
                }
                if (exitQ == true)
                {
                    if (exitY < 0) exitY += 10;
                }
                else
                {
                    if (exitY > -130) exitY -= 10;
                }
                infoBox.transform.position = new Vector3(infoBox.position.x, infoBox.position.y, info);
                exitBox.transform.position = new Vector3(exitBox.position.x, exitBox.position.y, exitY);

                if (exit == true)
                {
                    exitSelect.transform.position = exitPosition;
                    if (exitQ)
                    {
                        if (confirmexit)
                        {
                            exitSelect.transform.position = new Vector3(-30, exitBox.position.y, exitY - 9);
                        }
                        else
                        {
                            exitSelect.transform.position = new Vector3(30, exitBox.position.y, exitY - 9);
                        }
                    }
                }
                else
                {
                    exitSelect.transform.position = new Vector3(0, 0, -150);
                }
                if (reset > 0)
                {
                    reset++;
                    if (reset > 50)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                }

            }



            if (turning)
            {
                if (command == 1)//rotatrR
                {

                    map.transform.eulerAngles = new Vector3(map.transform.eulerAngles.x, map.transform.eulerAngles.y + 3, map.transform.eulerAngles.z);
                    if (turner == 29)
                    {
                        map.transform.eulerAngles = matchAngle;
                        command = 0;
                    }
                }
                if (command == 2)//rotatrL
                {
                    map.transform.eulerAngles = new Vector3(map.transform.eulerAngles.x, map.transform.eulerAngles.y - 3, map.transform.eulerAngles.z);
                    if (turner == 29)
                    {
                        map.transform.eulerAngles = matchAngle;
                        command = 0;
                    }
                }

                if (command == 3)//flip
                {
                    if (turner < 14)
                    {
                        map.transform.position = new Vector3(map.transform.position.x, map.transform.position.y, map.transform.position.z + 15);
                    }
                    if (turner == 14)
                    {
                        map.transform.eulerAngles = matchAngle;

                    }
                    if (turner > 14)
                    {
                        map.transform.position = new Vector3(map.transform.position.x, map.transform.position.y, map.transform.position.z - 15);
                    }
                    if (turner == 29)
                    {
                        map.transform.position = STARTP;
                        command = 0;
                    }
                }


                turner++;
                if (command == 0)
                {

                    if (turner >= 30)
                    {
                        player.spawnPoint = new Vector3(player.spawn.transform.position.x, 3, player.spawn.transform.position.z);
                        turner = 0;
                        turning = false;
                        map.transform.eulerAngles = matchAngle;
                    }

                }
            }
        }
    }
}
