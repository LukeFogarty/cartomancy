using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Transform title;
    public Transform select;
    public Transform selector;
    public Transform selectorSetting;
    public Transform selectorOption;
    public Transform wipe;
    public Transform sclimpini;
    private Transform selector1;
    public Transform level;
    public Transform settingsMenu;
    public GameObject starter;
    public AudioSource m_AudioSource;
    public AudioSource audioSource;
    public AudioClip[] clips;
    public TMPro.TextMeshPro levelTitle;

    private bool levelSelect = false;
    private bool levelSelected = false;
    private float rise = 0;
    private int lower = 0;
    private int lower2 = 0;
    readonly Transform[] levels = new Transform[20];
    public GameObject manager;
    int menu;
    int gogo = 0;
    int credit = 180;
    public int exiting = 0;
    bool settings = false;
    bool openSettings = false;
    int settingsOption =0;
    int settingsZ = -220;
    public Transform infobox1;
    public Transform infobox2;
    bool held;
    int heldCooldown;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");
        if (manager.GetComponent<GameManager>().started == true)
        {
            levelSelect = true;
            title.position = new Vector3(title.position.x, title.position.y, title.position.z - (8 * 30));
            sclimpini.position = new Vector3(sclimpini.position.x, sclimpini.position.y, sclimpini.position.z - 1000);
            credit = 0;
            lower = 8;
            m_AudioSource.volume = PlayerPrefs.GetFloat("volume");
            audioSource.volume = PlayerPrefs.GetFloat("volume");
        }

        for (int j = 0; j < levels.Length; j++)
        {
            
            levels[j] = Instantiate(level, new Vector3(-100+ 50 * (j%5), 10, 305- 45 * Mathf.Floor(j/5)), Quaternion.identity);
            levels[j].parent = select;
            levels[j].GetChild(0).GetComponent<TMPro.TextMeshPro>().text = 1+j + ".";
        }

        for (int j = 0; j < levels.Length; j++)
        {
            if (manager.GetComponent<GameManager>().levels[j].finished == false) { levels[j].GetChild(1).GetComponent<MeshRenderer>().enabled = false; }
            for (int c = 1; c <= 5; c++)
            {
                if (c <= manager.GetComponent<GameManager>().levels[j].coins)
                { levels[j].GetChild(2).GetComponent<TMPro.TextMeshPro>().text += "0"; }
                else
                { levels[j].GetChild(2).GetComponent<TMPro.TextMeshPro>().text += "-"; }
            }
        }
        selector1 = Instantiate(selector, new Vector3(-100 + 50 * (manager.GetComponent<GameManager>().lastPicked % 5), 11, 305 - 45 * Mathf.Floor(manager.GetComponent<GameManager>().lastPicked / 5)), Quaternion.identity);
        selector1.parent = select;
        menu = manager.GetComponent<GameManager>().lastPicked;
        m_AudioSource.Play();
    }

    void Update()
    {

        if (credit > 0)
        {
            credit--;
            if (Input.GetButtonDown("OpenMap") && credit > 50)
            {
                audioSource.clip = clips[2];
                audioSource.Play();
                credit = 50;
            }
            if (credit == 50) m_AudioSource.Play();
            if (credit < 50)
            {
                sclimpini.position = new Vector3(sclimpini.position.x, sclimpini.position.y, sclimpini.position.z + 15);
                if (m_AudioSource.volume < 1) m_AudioSource.volume += 0.05f;
            }
        }
        if (Input.GetButtonDown("OpenMap") && levelSelect == false && credit <= 0)
        {
            audioSource.clip = clips[2];
            audioSource.Play();
            levelSelect = true;
            lower = 8;
            manager.GetComponent<GameManager>().started = true;
        }
        if (levelSelect == true && rise < 15)
        {
            rise+=0.5f;
        }
        if (rise >= 15) lower = 0;

        title.position = new Vector3(title.position.x, title.position.y, title.position.z - lower);
        select.position = new Vector3(select.position.x, select.position.y, select.position.z - lower);
        wipe.position = new Vector3(wipe.position.x, wipe.position.y, wipe.position.z + lower2);
        settingsMenu.position = new Vector3(settingsMenu.position.x, settingsMenu.position.y, settingsZ);
        if (openSettings)
        {
            settingsMenu.GetChild(3).GetComponent<TMPro.TextMeshPro>().text = "            Volume      ";
            for(float i = 0; i<PlayerPrefs.GetFloat("volume")*10; i ++)
            {
                settingsMenu.GetChild(3).GetComponent<TMPro.TextMeshPro>().text += "|";
            }
        }
        if (settings == false) selectorSetting.position = new Vector3(0, 0, -100);
        if (rise >= 15)
        {
            if (settings == true) selectorSetting.position = new Vector3(-172.5f, 10, -88.5f);
            if (settings == false) selector1.position = new Vector3(-100 + 50 * (menu % 5), 10, 71 - 45 * Mathf.Floor(menu / 5));
            if (settings == true) selector1.position = new Vector3(-1100 + 50 * (menu % 5), 10, 71 - 45 * Mathf.Floor(menu / 5));
            if (settingsZ >= 0)
            {
                switch (settingsOption)
                {
                    case 1:
                        selectorOption.position = new Vector3(selectorOption.position.x, selectorOption.position.y, 5.5f);
                        break;
                    case 2:
                        selectorOption.position = new Vector3(selectorOption.position.x, selectorOption.position.y, -10);
                        break;
                    case 3:
                        selectorOption.position = new Vector3(selectorOption.position.x, selectorOption.position.y, -28f);
                        break;
                    case 4:
                        selectorOption.position = new Vector3(selectorOption.position.x, selectorOption.position.y, -51);
                        break;
                    default:
                        selectorOption.position = new Vector3(selectorOption.position.x, selectorOption.position.y, 22);
                        break;
                }
            }
            else { selectorOption.position = new Vector3(selectorOption.position.x, selectorOption.position.y, 200f); }
        }

        if (levelSelect == true)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if ((h != 0 || v != 0) && heldCooldown == 0) { held = true; heldCooldown = 20; }
            if (heldCooldown > 0) heldCooldown--;
            if (heldCooldown == 18) held = false;
            if (Input.GetButtonDown("ArrowL") ||(h<0 && held == true))
            {
                audioSource.clip = clips[1];
                audioSource.Play();
                if (openSettings == false)
                {
                    switch (menu)
                    {
                        case 0:
                        case 5:
                        case 10:
                        case 15:
                            settings = true;
                            break;
                        default:
                            if (menu > 0) menu--;
                            break;

                    }
                }
                if (openSettings == true)
                {
                    if (settingsOption == 1) manager.GetComponent<GameManager>().Volume(false);
                    m_AudioSource.volume = PlayerPrefs.GetFloat("volume");
                    audioSource.volume = PlayerPrefs.GetFloat("volume");
                }

            }
            if (Input.GetButtonDown("ArrowR") || (h > 0 && held == true))
            {
                audioSource.clip = clips[1];
                audioSource.Play();
                if (openSettings == false)
                {
                    if (menu < levels.Length - 1 && settings == false) menu++;
                    if (settings == true) settings = false;
                }
                if (openSettings == true)
                {
                    if (settingsOption == 1) manager.GetComponent<GameManager>().Volume(true);
                    m_AudioSource.volume = PlayerPrefs.GetFloat("volume");
                    audioSource.volume = PlayerPrefs.GetFloat("volume");
                }

            }
            if (Input.GetButtonDown("ArrowU") || (v > 0 && held == true))
            {
                audioSource.clip = clips[1];
                audioSource.Play();
                if (menu-5 >= 0 && settings == false) menu-=5;
                if (openSettings == true &&  infobox1.position.z < 0 && infobox2.position.z < 0 && openSettings == true && exiting ==0)
                {
                    if (settingsOption > 0) settingsOption--;
                }
              
            }
            if (Input.GetButtonDown("ArrowD") || (v < 0 && held == true))
            {
                audioSource.clip = clips[1];
                audioSource.Play();
                if (menu+5 <= levels.Length-1 && settings == false) menu+=5;
                if (settingsOption < 4 && infobox1.position.z < 0 && infobox2.position.z < 0 && openSettings == true && exiting == 0) settingsOption++;

            }

            //choose level
            if (rise >= 10 && Input.GetButtonDown("OpenMap") && levelSelect == true && gogo == 0 && settings == false && openSettings == false)
            {
                audioSource.clip = clips[0];
                audioSource.Play();
                levelSelected = true;
                manager.GetComponent<GameManager>().lastPicked = menu;
                m_AudioSource.volume = PlayerPrefs.GetFloat("volume");
                audioSource.volume = PlayerPrefs.GetFloat("volume");
            }

            //open setting
            if (rise >= 10 && Input.GetButtonDown("OpenMap") && levelSelect == true && gogo == 0 && settings == true)
            {
                if (openSettings == false)
                {
                    audioSource.clip = clips[2];
                    openSettings = true;
                }
                else
                {
                    audioSource.clip = clips[1];
                    switch (settingsOption)
                    {
                        case 0:
                            audioSource.clip = clips[3];
                            openSettings = false;
                            break;
                        case 2:
                            if (infobox1.position.z < 0) {infobox1.position = new Vector3(infobox1.position.x, infobox1.position.y, 0); audioSource.clip = clips[2]; }
                            else { infobox1.position = new Vector3(infobox1.position.x, infobox1.position.y, -500); audioSource.clip = clips[3]; }
                            break;
                        case 3:
                            if (infobox2.position.z < 0) { infobox2.position = new Vector3(infobox2.position.x, infobox2.position.y, 0); audioSource.clip = clips[2]; }
                            else { infobox2.position = new Vector3(infobox2.position.x, infobox2.position.y, -500); audioSource.clip = clips[3]; }
                            break;
                        case 4:
                            if (exiting == 0)
                            {
                                audioSource.clip = clips[0];
                                exiting = 1;
                            }
                            break;

                        default:
                            break;
                    }
                }
                audioSource.Play();
            }

            if (openSettings == true)
            {
                if (settingsZ < 0) settingsZ += 20;
            }
            if (openSettings == false)
            {
                if (settingsZ > -220) settingsZ -= 20;
            }

            if (exiting > 0)
            {
                exiting++;
                if (exiting >70)
                {
                    Application.Quit();
                    Debug.Log("Game is exiting");
                }
            }
            //when level is chosen
            if ( levelSelected == true)
            {
                levelTitle.text = (1+menu)+".\n"+ manager.GetComponent<GameManager>().levels[menu].name;
                lower2 = 10;
                gogo++;

                if (gogo > 30)
                {
                    if (m_AudioSource.volume > 0) m_AudioSource.volume -= 0.05f;
                    if (gogo < 70) lower2 = 0;
                }
                if (gogo > 70)
                {
                    lower2 = 10;
                }
                if (gogo > 100)
                {
                    
                    //creating the right level
                    switch (menu)
                    {
                        default:
                            {
                                Instantiate(starter, new Vector3(0, 0, 0), Quaternion.identity);
                                break;
                            }
                    }
                    m_AudioSource.Stop();
                    Destroy(gameObject);
                }
            }
        }
    }
}
