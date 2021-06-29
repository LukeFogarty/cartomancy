using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockY : MonoBehaviour
{
    public float y;
    public bool lockRoatation = false;
    public bool lockRoatationXY = false;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        if (lockRoatation) transform.eulerAngles = new Vector3(0, 180, 0);
        if (lockRoatationXY) transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
