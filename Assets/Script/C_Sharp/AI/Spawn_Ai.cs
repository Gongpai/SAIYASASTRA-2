using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Ai : MonoBehaviour
{
    [SerializeField] GameObject Spawn_Ai_Ghost;
    [SerializeField] AudioSource audioSource;
    [SerializeField] bool IsPlayAudio_SpawnGhost = false;

    GameObject spwnGhost;

    public void On_Spawn_Ai()
    {
        Spawn_Ai_Ghost.transform.position = transform.position;
        spwnGhost = Instantiate(Spawn_Ai_Ghost);
        
    }

    public void OnPlaySound()
    {
        if (IsPlayAudio_SpawnGhost)
        {
            spwnGhost.GetComponent<AudioSource>().Play();
        }
        else
        {
            audioSource.Play();
        }
    }
}
