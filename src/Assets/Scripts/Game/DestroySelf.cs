using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public ParticleSystem parts;
    public AudioSource audioSource;
    public AudioClip[] clips;
    // Update is called once per frame
    private void Start()
    {
        if (audioSource != null) {
            if (clips.Length > 0)
            {
                audioSource.clip = clips[Random.Range(0,clips.Length)];
            }
            audioSource.Play(); 
        }
        float totalDuration = parts.main.duration;
        Destroy(gameObject, totalDuration);
    }
}
