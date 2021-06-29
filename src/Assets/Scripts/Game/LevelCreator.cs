using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    int level;
    public Texture2D[] sourceMap;
    public static int mapSize = 15;
    public static int tileSize = 10;
    public Transform player;
    public Transform tile;
    public Transform spikes;
    public Transform switchTile;
    public Transform ghost;
    public Transform coin;
    public Transform crystal;
    public Transform exit;
    public Transform holy;
    public Transform cannon;
    public Transform torch;
    public Transform[] quarters = new Transform[4];
    public int[,] mapArray = new int[mapSize, mapSize];
    GameObject manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");
        level = manager.GetComponent<GameManager>().lastPicked;

        SetArray(mapArray, sourceMap[level]);
        MakeFromArray(mapArray);
    }

    //method for creating our map
    void SetArray(int[,] array, Texture2D image)
    {
        for (int i = 0; i <mapSize; i++)
        {
            for(int j = 0; j < mapSize; j++)
            {
                Color pixel = image.GetPixel(i, j);
                //spikes
                if (pixel.r >= 0.5 && pixel.g >= 0.5 && pixel.b>= 0.5 && pixel.r <= 0.75 && pixel.g <= 0.75 && pixel.b <= 0.75)
                {
                    array[i, j] = 1;
                }
                //floor
                else if (pixel.r == 1 && pixel.g == 1 && pixel.b == 1)
                {
                    array[i, j] = 2;
                }
                //crystal
                else if(pixel.r == 0 && pixel.g == 0 && pixel.b == 1)
                {
                    array[i, j] = 3;
                }
                //switch
                else if(pixel.r == 0 && pixel.g == 1 && pixel.b == 0)
                {
                    array[i, j] = 4;
                }
                //ghost
                else if(pixel.r == 1 && pixel.g == 0 && pixel.b == 0)
                {
                    array[i, j] = 5;
                }
                //coins
                else if(pixel.r == 1 && pixel.g == 1 && pixel.b == 0)
                {
                    array[i, j] = 6;
                }
                //start
                else if(pixel.r == 0 && pixel.g == 1 && pixel.b == 1)
                {
                    array[i, j] = 7;
                }
                //holytile
                else if(pixel.r == 1 && pixel.g == 0 && pixel.b == 1)
                {
                    array[i, j] = 8;
                }
                //cannon
                else if (pixel.r == 1 && pixel.g > 0.4f && pixel.g < 0.6f && pixel.b == 0)
                {
                    array[i, j] = 9;
                }
                //torch
                else if (pixel.r == 1 && pixel.b> 0.4f && pixel.b < 0.6f && pixel.g == 0)
                {
                    array[i, j] = 10;
                }
            }
        }
    }

    private void Update()
    {
        if (player.GetComponent<PlayerMovement>().useMap == false)transform.position = Vector3.zero;
    }

    void MakeFromArray(int[,] array)
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                //create spikes
                if (array[i, j] == 1)
                {
                    Transform newtile = Instantiate(spikes, new Vector3(-tileSize/2+(tileSize * (-mapSize / 2)) + tileSize * i, 0, -tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * j), Quaternion.identity);
                    newtile.GetChild(11).GetComponent<SpikeFloors>().player = player.gameObject;
                    if (i < 10 && j >= 10) newtile.parent = quarters[0];
                    if (i >= 10 && j >= 10) newtile.parent = quarters[1];
                    if (i < 10 && j < 10) newtile.parent = quarters[2];
                    if (i >= 10 && j < 10) newtile.parent = quarters[3];

                }
                //create room
                if (array[i, j] >= 2)
                {
                    Transform newtile = Instantiate(tile, new Vector3(-tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * i, 0, -tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * j), Quaternion.identity);

                    if (i < 10 && j >= 10) newtile.parent = quarters[0];
                    if (i >= 10 && j >= 10) newtile.parent = quarters[1];
                    if (i < 10 && j < 10) newtile.parent = quarters[2];
                    if (i >= 10 && j < 10) newtile.parent = quarters[3];


                    //create crystal
                    if (array[i, j] == 3)
                    {
                        Transform newObject = Instantiate(crystal, new Vector3((tileSize * (-mapSize / 2)) + tileSize * i, 3, (tileSize * (-mapSize / 2)) + tileSize * j), Quaternion.identity);

                        if (i < 10 && j >= 10) newObject.parent = quarters[0];
                        if (i >= 10 && j >= 10) newObject.parent = quarters[1];
                        if (i < 10 && j < 10) newObject.parent = quarters[2];
                        if (i >= 10 && j < 10) newObject.parent = quarters[3];

                    }
                    //create switch
                    if (array[i, j] == 4)
                    {
                        Transform newObject = Instantiate(switchTile, new Vector3(-tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * i, 0, -tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * j), Quaternion.identity);
                        newObject.GetChild(1).GetComponent<SwitchScript>().player = player.gameObject;
                        if (i < 10 && j >= 10) newObject.parent = quarters[0];
                        if (i >= 10 && j >= 10) newObject.parent = quarters[1];
                        if (i < 10 && j < 10) newObject.parent = quarters[2];
                        if (i >= 10 && j < 10) newObject.parent = quarters[3];
                    }
                    //create ghost
                    if (array[i, j] == 5)
                    {
                        Transform newObject = Instantiate(ghost, new Vector3((tileSize * (-mapSize / 2)) + tileSize * i, -5, (tileSize * (-mapSize / 2)) + tileSize * j), Quaternion.identity);
                        newObject.GetComponent<FollowPlayer>().startRoom = newtile;
                        newObject.GetComponent<FollowPlayer>().player = player;

                    }
                    //create coin
                    if (array[i, j] == 6)
                    {
                        Transform newObject = Instantiate(coin, new Vector3((tileSize * (-mapSize / 2)) + tileSize * i, 4, (tileSize * (-mapSize / 2)) + tileSize * j), Quaternion.identity);

                        if (i < 10 && j >= 10) newObject.parent = quarters[0];
                        if (i >= 10 && j >= 10) newObject.parent = quarters[1];
                        if (i < 10 && j < 10) newObject.parent = quarters[2];
                        if (i >= 10 && j < 10) newObject.parent = quarters[3];
                    }
                    //create exit
                    if (array[i, j] == 7)
                    {
                        Transform newObject = Instantiate(exit, new Vector3(-tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * i + 5, 0, -tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * j + 7), Quaternion.identity);

                        if (i < 10 && j >= 10) newObject.parent = quarters[0];
                        if (i >= 10 && j >= 10) newObject.parent = quarters[1];
                        if (i < 10 && j < 10) newObject.parent = quarters[2];
                        if (i >= 10 && j < 10) newObject.parent = quarters[3];

                        if (j + 1 >= mapSize || (j + 1 < mapSize && array[i, j + 1] == 0))
                        {
                            newObject.transform.eulerAngles = new Vector3(newObject.transform.eulerAngles.x, newObject.transform.eulerAngles.y + 180, newObject.transform.eulerAngles.z);
                            newObject.transform.position = new Vector3(newObject.transform.position.x, newObject.transform.position.y, newObject.transform.position.z - 4.5f);
                            
                        }
                        else if (j - 1 < 0 || (j - 1 >= 0 && array[i, j - 1] == 0))
                        {
                            newObject.transform.eulerAngles = new Vector3(newObject.transform.eulerAngles.x, newObject.transform.eulerAngles.y, newObject.transform.eulerAngles.z);
                            newObject.transform.position = new Vector3(newObject.transform.position.x, newObject.transform.position.y, newObject.transform.position.z);

                        }
                        else if (i + 1 >= mapSize || (i + 1 < mapSize && array[i + 1, j] == 0))
                        {
                            newObject.transform.eulerAngles = new Vector3(newObject.transform.eulerAngles.x, newObject.transform.eulerAngles.y + 270, newObject.transform.eulerAngles.z);
                            newObject.transform.position = new Vector3(newObject.transform.position.x - 2f, newObject.transform.position.y, newObject.transform.position.z - 2);
                            
                        }
                        else if (i - 1 < 0 || (i - 1 > 0 && array[i - 1, j] == 0))
                        {
                            newObject.transform.eulerAngles = new Vector3(newObject.transform.eulerAngles.x, newObject.transform.eulerAngles.y + 90, newObject.transform.eulerAngles.z);
                            newObject.transform.position = new Vector3(newObject.transform.position.x + 2f, newObject.transform.position.y, newObject.transform.position.z - 2);
                            
                        }


                        player.gameObject.GetComponent<PlayerMovement>().spawn = newObject;
                        player.gameObject.GetComponent<PlayerMovement>().spawnPoint = new Vector3(newObject.transform.position.x, 4, newObject.transform.position.z);
                        player.position = new Vector3(newObject.transform.position.x, 4f, newObject.transform.position.z);
                        player.eulerAngles = new Vector3(player.eulerAngles.x, newObject.transform.eulerAngles.y, player.eulerAngles.z);
                    }
                    //create holy
                    if (array[i, j] == 8)
                    {
                        Transform newObject = Instantiate(holy, new Vector3(-tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * i, 0, -tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * j), Quaternion.identity);
                        newObject.GetChild(4).GetComponent<InsideKillSpace>().player = player.gameObject;
                        if (i < 10 && j >= 10) newObject.parent = quarters[0];
                        if (i >= 10 && j >= 10) newObject.parent = quarters[1];
                        if (i < 10 && j < 10) newObject.parent = quarters[2];
                        if (i >= 10 && j < 10) newObject.parent = quarters[3];
                    }
                    //create cannon
                    if (array[i, j] == 9)
                    {
                        Transform newObject = Instantiate(cannon, new Vector3(-tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * i + 5, 0, -tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * j + 7), Quaternion.identity);
                        newObject.GetComponent<CannonFire>().player = player.gameObject;

                        if (i < 10 && j >= 10) newObject.parent = quarters[0];
                        if (i >= 10 && j >= 10) newObject.parent = quarters[1];
                        if (i < 10 && j < 10) newObject.parent = quarters[2];
                        if (i >= 10 && j < 10) newObject.parent = quarters[3];

                        if (j - 1 >= 0 && array[i, j - 1] > 0)
                        {
                            newObject.transform.eulerAngles = new Vector3(newObject.transform.eulerAngles.x, newObject.transform.eulerAngles.y + 180, newObject.transform.eulerAngles.z);
                            newObject.transform.position = new Vector3(newObject.transform.position.x, newObject.transform.position.y, newObject.transform.position.z+0.3f);
                        }
                        else if (j + 1 < mapSize && array[i, j + 1] > 0)
                        {
                            newObject.transform.eulerAngles = new Vector3(newObject.transform.eulerAngles.x, newObject.transform.eulerAngles.y, newObject.transform.eulerAngles.z);
                            newObject.transform.position = new Vector3(newObject.transform.position.x, newObject.transform.position.y, newObject.transform.position.z - 4.8f);
                        }
                        else if (i + 1 < mapSize && array[i + 1, j] > 0)
                        {
                            newObject.transform.eulerAngles = new Vector3(newObject.transform.eulerAngles.x, newObject.transform.eulerAngles.y + 90, newObject.transform.eulerAngles.z);
                            newObject.transform.position = new Vector3(newObject.transform.position.x - 2.3f, newObject.transform.position.y, newObject.transform.position.z - 2);
                        }
                        else if (i - 1 > 0 && array[i - 1, j] > 0)
                        {
                            newObject.transform.eulerAngles = new Vector3(newObject.transform.eulerAngles.x, newObject.transform.eulerAngles.y + 270, newObject.transform.eulerAngles.z);
                            newObject.transform.position = new Vector3(newObject.transform.position.x + 2.3f, newObject.transform.position.y, newObject.transform.position.z - 2);
                        }
                    }
                    //create torch
                    if (array[i, j] == 10)
                    {
                        Transform newObject = Instantiate(torch, new Vector3(-tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * i + 5, 0, -tileSize / 2 + (tileSize * (-mapSize / 2)) + tileSize * j + 5), Quaternion.identity);
                        newObject.GetChild(4).GetComponent<SwitchScript>().player = player.gameObject;

                        if (i < 10 && j >= 10) newObject.parent = quarters[0];
                        if (i >= 10 && j >= 10) newObject.parent = quarters[1];
                        if (i < 10 && j < 10) newObject.parent = quarters[2];
                        if (i >= 10 && j < 10) newObject.parent = quarters[3];
                    }
                }
            }
        }
    }

}
