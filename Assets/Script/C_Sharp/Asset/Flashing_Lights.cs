using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using static Flashing_Lights;

public class Flashing_Lights : MonoBehaviour
{
    [SerializeField] private int FlashingNum = 3;
    [SerializeField] List<AudioClip> m_AudioClipList = new List<AudioClip>();
    [SerializeField] Light m_light;
    [SerializeField] GameObject Light_Collider;
    [SerializeField] Character_Stay_In_Light character_Stay_In_Light;

    public bool Can_Flashing = true;
    public bool Can_Stop_Flashing = true;
    bool is_Stop_Flashing_Char = false;
    bool is_Stop_Flashing_Ghost = false;
    public enum Light_Mode
    {
        Flashing,
        Turn_On,
        Turn_Off,
    }

    public delegate void Turn_light_On_Off(Light_Mode light_Mode);
    public static Turn_light_On_Off event_Light_On_Off;

    Animator m_Animator;
    AudioSource m_AudioSource;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        m_AudioSource = gameObject.GetComponent<AudioSource>();
        FlashingNum += 1;
    }

    private void OnEnable()
    {
        event_Light_On_Off += Light_On_Off;
    }

    private void OnDisable()
    {
        event_Light_On_Off -= Light_On_Off;
    }

    public void Light_On_Off(Light_Mode light_Mode)
    {
        switch (light_Mode)
        {
            case Light_Mode.Flashing:
                m_light.intensity = 1;
                m_light.enabled = true;
                Can_Flashing = true;
                Can_Stop_Flashing = true;
                is_Stop_Flashing_Char = false;
                is_Stop_Flashing_Ghost = false;
                Light_Collider.SetActive(true);
                print("Flashinggggggggggggggggggggggggg");
                break;
            case Light_Mode.Turn_On:
                m_light.intensity = 1;
                m_light.enabled = true;
                Can_Flashing = false;
                Light_Collider.SetActive(true);
                print("Turn_Onnnnnnnnnnnnnnnnnnnnnnnnnn");
                break;
            case Light_Mode.Turn_Off:
                m_light.intensity = 0;
                m_light.enabled = false;
                Can_Flashing = false;
                
                Light_Collider.SetActive(false);
                TriggerGhostEvent();

                //print("Turn_Offffffffffffffffffffffffff");
                break;
            default:
                m_light.intensity = 1;
                m_light.enabled = true;
                Light_Collider.SetActive(true);
                break;
        }
    }

    private void TriggerGhostEvent()
    {
        if(character_Stay_In_Light.Ghost != null)
        {
            CustomEvent.Trigger(character_Stay_In_Light.Ghost, "Chasing");
            //print("GHOST CHASING");
        }
        
        //print("GHOST Trigger chasing");
    }

    public void PlayAnimation(bool Is_Character)
    {
        int m_random = 1;
        

        if (!Is_Character)
        {
            m_random = Random.Range(1, FlashingNum);
        }
        else
        {
            m_random = 1;
        }
        
        //print(m_Animator + " : " + m_random);
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

        //print("PlaySound");
    }

    void OnTriggerEnter(Collider collider)
    {
        int m_random_light = 0;

        if (Can_Flashing)
        {


            if (collider.gameObject.tag == "Player" && !is_Stop_Flashing_Char)
            {
                m_random_light = Random.Range(0, 6);
                if (m_random_light == 2 || m_random_light == 4)
                {
                    PlayAnimation(true);
                    if (Can_Stop_Flashing)
                        is_Stop_Flashing_Char = true;
                }
                else
                {
                    print("Random ----- : " + m_random_light + " : -----");
                    Light_On_Off(Light_Mode.Turn_On);
                }
            }

            if (collider.gameObject.tag == "Ghost" && !is_Stop_Flashing_Ghost)
            {
                PlayAnimation(false);
                if (Can_Stop_Flashing)
                    is_Stop_Flashing_Ghost = true;
            }
        }   
    }
}
