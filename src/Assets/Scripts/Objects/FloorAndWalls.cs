using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAndWalls : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] walls = new GameObject[4];
    public Transform[] points = new Transform[4];

    // Update is called once per frame
    void Start()
    {
        //this sets up the walls without having to think too much
        CheckForNeighbours();
    }

    void CheckForNeighbours()
    {
        // Bit shift the index of the layer (8) to get floor layer
        int layerMask = 1 << 8;
  
        RaycastHit[] hit = new RaycastHit[4];
        Ray[] ray = new Ray[4];
        for (int h = 0; h < 4; h++)
        {
            switch (h)
            {
                case 0:
                    ray[h] = new Ray(new Vector3(points[h].position.x, -5, points[h].position.z), new Vector3(0, 5, 0));
                    break;
                case 1:
                    ray[h] = new Ray(new Vector3(points[h].position.x, - 5, points[h].position.z), new Vector3(0, 5, 0));
                    break;
                case 2:
                    ray[h] = new Ray(new Vector3(points[h].position.x, - 5, points[h].position.z), new Vector3(0, 5, 0));
                    break;
                default:
                    ray[h] = new Ray(new Vector3(points[h].position.x, - 5, points[h].position.z), new Vector3(0, 5, 0));
                    break;
            }

            if (Physics.Raycast(ray[h], out hit[h], Mathf.Infinity, layerMask))
            {
                Destroy(walls[h]);//walls[h].SetActive(false);

            }
            /*else
            {
                walls[h].SetActive(true);
            }*/
        }
    }
}
