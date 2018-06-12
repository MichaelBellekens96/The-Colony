using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Kitchen : MonoBehaviour
{
    public AudioSource audioSource;
    public bool soundPlaying = false;

    public void PlaySound()
    {
        if (!soundPlaying)
        {
            audioSource.Play();
            soundPlaying = true;
        }
    }

    public void StopPlaying()
    {
        audioSource.Stop();
        soundPlaying = false;
    }
}
