using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShowMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Bgtext;

    [SerializeField] private TextMeshProUGUI TextShow;

    public delegate void m_ShowMessageEvent(string text);
    public static m_ShowMessageEvent showMessageEvent;

    public delegate void m_HideMessageEvent();
    public static m_HideMessageEvent hideMessageEvent;
    // Start is called before the first frame update
    void Start()
    {
        showMessageEvent = Show_Message;
        hideMessageEvent = Hide_Message;
    }

    public void Show_Message(string text)
    {
        print(text + " Test");
        this.GetComponent<Animator>().SetBool("IsShow?", true);
        this.GetComponent<Animator>().SetBool("IsHide?", false);
        Bgtext.SetText(text);
        TextShow.SetText(text);
    }

    public void Hide_Message()
    {
        this.GetComponent<Animator>().SetBool("IsShow?", false);
        this.GetComponent<Animator>().SetBool("IsHide?", true);
    }

    public void SetActiveMessage(active _active)
    {
        switch (_active)
        {
            case active.True:
                this.gameObject.GetComponent<Canvas>().enabled = true;
                break;
            case active.False:
                this.gameObject.GetComponent<Canvas>().enabled = false;
                break;
            default:
                break;
        }
        
    }
}
