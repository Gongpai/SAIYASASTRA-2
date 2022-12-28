using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flashing_Lights : MonoBehaviour
{
    [SerializeField] private int FlashingNum = 3;
    [SerializeField] List<AudioClip> m_AudioClipList = new List<AudioClip>();
    Animator m_Animator;
    AudioSource m_AudioSource;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        m_AudioSource = gameObject.GetComponent<AudioSource>();
        FlashingNum += 1;
    }

    public void PlayAnimation()
    {
        int m_random;
        m_random = Random.Range(1, FlashingNum);
        print(m_Animator + " : " + m_random);
        m_Animator.SetInteger("PlayAnimNUM", m_random);
    }

    public void StopAnimation()
    {
        m_Animator.SetInteger("PlayAnimNUM", 0);
    }

    public void PlaySound(Fluorescent_Tube_SFX_Play fluorescentTubeSfx)
    {
        switch (fluorescentTubeSfx)
        {
            case Fluorescent_Tube_SFX_Play.SFX1:
                m_AudioSource.clip = m_AudioClipList[0];
                m_AudioSource.Play();
                break;
            case Fluorescent_Tube_SFX_Play.SFX2:
                m_AudioSource.clip = m_AudioClipList[1];
                m_AudioSource.Play();
                break;
            case Fluorescent_Tube_SFX_Play.SFX3:
                m_AudioSource.clip = m_AudioClipList[2];
                m_AudioSource.Play();
                break;
            case Fluorescent_Tube_SFX_Play.SFX4:
                m_AudioSource.clip = m_AudioClipList[3];
                m_AudioSource.Play();
                break;
            case Fluorescent_Tube_SFX_Play.SFX5:
                m_AudioSource.clip = m_AudioClipList[4];
                m_AudioSource.Play();
                break;
            default:
                break;
        }

        print("PlaySound");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
            PlayAnimation();
    }
}
