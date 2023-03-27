using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bom_Effect_Animation : MonoBehaviour
{
    public void SetSound_Bom(AudioClip audioClip)
    {
        GetComponent<AudioSource>().clip = audioClip;
    }

    public void End_Anim()
    {
        Destroy(gameObject, 1);
    }
}
