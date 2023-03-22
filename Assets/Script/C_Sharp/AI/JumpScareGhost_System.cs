using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareGhost_System : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] AudioSource audioSource;

    bool IsplayStart = false;
    bool IsplayEnd = false;
    private void Update()
    {
        if (!IsplayStart && Camera.main.WorldToScreenPoint(transform.position).x >= 0 && Camera.main.WorldToScreenPoint(transform.position).x <= Screen.width && !GameInstance.CharacterHide)
        {
            audioSource.clip = audioClips[0];
            audioSource.Play();
            IsplayStart = true;
        }

        if (IsplayEnd)
        {
            if(!audioSource.isPlaying)
                Destroy(gameObject);
        }
    }

    public void OnEndJumpScare()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
        Flashing_Lights.event_Light_On_Off(Flashing_Lights.Light_Mode.Turn_Off);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        IsplayEnd = true;
    }
}
