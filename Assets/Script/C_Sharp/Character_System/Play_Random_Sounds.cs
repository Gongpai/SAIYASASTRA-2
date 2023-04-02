using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_Random_Sounds : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();

    private void Start()
    {
        
    }

    public void Play_Random_SFX()
    {
        int i = 0;
        i = Random.Range(0, 2);
        audioSource.clip = audioClips[i];
        audioSource.Play();
    }
}
