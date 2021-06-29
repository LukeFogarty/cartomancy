using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public GameObject bm1;
    public GameObject bm2;
    public GameObject bm3;
    public GameObject bm4;
    public MapController e;
    float[] vol = new float[4];
    int playing = 0;
    // Start is called before the first frame update
    void Start()
    {
        bm1.GetComponent<AudioSource>().Play();
        bm2.GetComponent<AudioSource>().Play();
        bm3.GetComponent<AudioSource>().Play();
        bm4.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        bm1.GetComponent<AudioSource>().volume = vol[0];
        bm2.GetComponent<AudioSource>().volume = vol[1];
        bm3.GetComponent<AudioSource>().volume = vol[2];
        bm4.GetComponent<AudioSource>().volume = vol[3];
        if (e.gameObject.GetComponent<PlayerMovement>().gotCrystal == false)
        {
            if (e.mapOpen == true) playing = 1;
            if (e.mapOpen == false && playing == 1) playing = 0;
            if (playing == 0)
            {
                if (vol[0] < PlayerPrefs.GetFloat("volume")) vol[0] += 0.003f;
                if (vol[1] > 0f) vol[1] -= 0.003f;
                if (vol[2] > 0f) vol[2] -= 0.003f;
                if (vol[3] > 0f) vol[3] -= 0.003f;
            }
            if (playing == 1)
            {
                if (vol[1] < PlayerPrefs.GetFloat("volume")) vol[1] += 0.003f;
                if (vol[2] > 0f) vol[2] -= 0.003f;
                if (vol[0] > 0.0f) vol[0] -= 0.003f;
                if (vol[3] > 0f) vol[3] -= 0.003f;
            }
            if (playing == 2)
            {
                if (vol[2] < PlayerPrefs.GetFloat("volume")) vol[2] += 0.003f;
                if (vol[1] > 0f) vol[1] -= 0.003f;
                if (vol[0] > 0.0f) vol[0] -= 0.003f;
                if (vol[3] > 0f) vol[3] -= 0.003f;
            }
            if (playing == 3)
            {
                if (vol[3] < PlayerPrefs.GetFloat("volume")) vol[3] += 0.003f;
                if (vol[1] > 0f) vol[1] -= 0.003f;
                if (vol[0] > 0.0f) vol[0] -= 0.003f;
                if (vol[2] > 0f) vol[2] -= 0.003f;
            }
        }
        else
        {
            if (vol[3] > 0f) vol[3] -= 0.003f;
            if (vol[1] > 0f) vol[1] -= 0.003f;
            if (vol[0] > 0f) vol[0] -= 0.003f;
            if (vol[2] > 0f) vol[2] -= 0.003f;
        }

    }


    void OnTriggerStay(Collider other)
    {

        if(other.gameObject.CompareTag("crystal"))
        {
            playing = 2;
        }
        if (other.gameObject.CompareTag("enemy"))
        {
            playing = 3;
        }

    }
    void OnTriggerExit(Collider other)
    {
        playing = 0;
    }

}
