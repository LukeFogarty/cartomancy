using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool started = false;
    public int lastPicked = 0;
    public Level[] levels;
    public string[] names;
    public float v;
    readonly int numberOfLevels = 20;
    readonly int framerate = 60;

    void Awake()
    {

    QualitySettings.vSyncCount = 0;
    Application.targetFrameRate = framerate;
    GameObject[] array = GameObject.FindGameObjectsWithTag("GameController");

        v = PlayerPrefs.GetFloat("volume");
        if (PlayerPrefs.GetFloat("volume") < 0.1) PlayerPrefs.SetFloat("volume", 0.4f);
        if (array.Length > 1)
        {
            for (int i = 1; i < array.Length; i++)
            {
                Destroy(array[1]);
            }
        }
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        DontDestroyOnLoad(gameObject);
        levels = new Level[numberOfLevels];

        for (int i =0; i < numberOfLevels; i++)
        {
            levels[i] = new Level(names[i], false, 0);
        }
    }

    public void Volume(bool Higher)
    {
        float newVolume = PlayerPrefs.GetFloat("volume");
        if (Higher)
        {
            if (newVolume < 1) newVolume += 0.1f;
        }
        else
        {
            if (newVolume > 0) newVolume -= 0.1f;
        }
        PlayerPrefs.SetFloat("volume", newVolume);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }
}

public class Level
{
    public string name;
   // public string image;
    public bool finished;
    public int coins;

    public Level(string levelName, bool levelFinished, int coinsGotten )
    {
        name = levelName;
        finished = levelFinished;
        coins = coinsGotten;
    }
}

