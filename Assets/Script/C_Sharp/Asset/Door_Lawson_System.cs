using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Door_Lawson_System : MonoBehaviour
{
    public Animator animator;

    [SerializeField] private GameObject DoorCollider;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenOrClose(bool SetAnimOpen = false, bool SetAnimClose = false, bool IsSet = false)
    {
        if (DoorCollider.GetComponent<MoveCameraToNewScene>().IsCharacterEnter || IsSet)
        {
            if (IsSet)
            {
                animator.SetBool("IsDoorOpen?", SetAnimOpen);
                animator.SetBool("IsDoorClose?", SetAnimClose);
            }
            else
            {
                animator.SetBool("IsDoorOpen?", !animator.GetBool("IsDoorOpen?"));
                animator.SetBool("IsDoorClose?", !animator.GetBool("IsDoorClose?"));
            }
        }
    }
}
