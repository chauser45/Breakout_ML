using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip BounceSound, BustSound;
    public static bool SoundOn;
    static AudioSource audioSrc;
    
    void Start()
    {
        BounceSound = Resources.Load<AudioClip>("Bounce");
        BustSound = Resources.Load<AudioClip>("Bust");
        audioSrc = GetComponent<AudioSource>();
    }

    //method to be called in other scripts which plays the preloaded sounds
    public static void PlaySound(string clip){
        if (SoundOn){
            switch(clip){
            case "Bounce":
                audioSrc.PlayOneShot(BounceSound);
                break;
            case "Bust":
                audioSrc.PlayOneShot(BustSound);
                break;
            }
        }    
    }
}
