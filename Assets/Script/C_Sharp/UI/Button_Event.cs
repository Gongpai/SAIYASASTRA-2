using UnityEngine;
using UnityEngine.Events;

using System;
using System.Collections;

[Serializable] public class ButtonEvent : UnityEvent { }

public class Button_Event : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] bool Set_Dont_Anim_Play_Start = false;

    public ButtonEvent OnAnimStart;
    public ButtonEvent OnAnimStop;

    bool Is_Start = true;
    bool Can_Play_start = true;

    private void OnEnable()
    {
        if (Can_Play_start && !Set_Dont_Anim_Play_Start)  
        {
            print("Cannn Startttt");
            Is_Start = true;
            animator.SetBool("IsIn", true);
            animator.SetBool("IsOut", false);
        }
        if (Set_Dont_Anim_Play_Start)
        {
            print("Dont Play Startttt");
            animator.SetBool("IsIn", false);
            animator.SetBool("IsOut", false);
            animator.SetBool("DontPlayIn", true);
        }
    }
    private void OnDisable()
    {
        Can_Play_start = true;
    }

    public void OnStart()
    {
        if (!Is_Start)
        {
            Is_Start = true;
            OnAnimStart.Invoke();
        }   
    }

    public void OnStop()
    {
        if (Is_Start)
        {
            OnAnimStop.Invoke();
        }
    }

    public void OnClose()
    {
        Is_Start = false;
        Can_Play_start = true;
        animator.SetBool("IsIn", false);
        animator.SetBool("IsOut", true);
        animator.SetBool("DontPlayIn", false);
    }

    public void DontPlayIn(bool Is_Can_Play_Start = false)
    {
        Can_Play_start = Is_Can_Play_Start;
        if (!Is_Can_Play_Start)
        {
            animator.SetBool("IsIn", false);
            animator.SetBool("IsOut", false);
            animator.SetBool("DontPlayIn", true);
        }
        else
        {
            animator.SetBool("IsIn", true);
            animator.SetBool("IsOut", false);
            animator.SetBool("DontPlayIn", false);
        }
    }

    public void t_1()
    {
        print("Startttttttttttttttttttttttttttttttttt");
    }

    public void t_2()
    {
        print("Stoppppppppppppppppppppppppppppppppppp");
    }
}
