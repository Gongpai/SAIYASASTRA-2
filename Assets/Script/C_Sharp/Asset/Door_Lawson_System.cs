using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Door_Lawson_System : MonoBehaviour
{
    public Animator animator;
    [SerializeField] string m_closeDoorText = "»Ô´»ÃÐµÙ";
    [SerializeField] string m_openDoorText = "à»Ô´»ÃÐµÙ";
    [SerializeField] private GameObject DoorCollider;
    [SerializeField] private GameObject ExitDoorCollider;
    
    private string OpenDoorText
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
                return $"[E] {m_openDoorText}";
            else
                return m_openDoorText;
        }
    }
    private string CloseDoorText
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
                return $"[E] {m_closeDoorText}";
            else
                return m_closeDoorText;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenOrClose(bool SetAnimOpen = false, bool SetAnimClose = false, bool IsSet = false)
    {
        if (DoorCollider.GetComponent<MoveCameraToNewScene>().IsCharacterEnter)
        {
            GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message(CloseDoorText);
            if (IsSet)
            {
                animator.SetBool("IsDoorOpen?", SetAnimOpen);
                animator.SetBool("IsDoorClose?", SetAnimClose);
            }
            else
            {
                animator.SetBool("IsDoorOpen?", !animator.GetBool("IsDoorOpen?"));
                animator.SetBool("IsDoorClose?", !animator.GetBool("IsDoorClose?"));
                if (animator.GetBool("IsDoorClose?"))
                {
                    GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message(OpenDoorText);
                }
            }
        }
        else
        {
            if (IsSet && SetAnimClose)
            {
                animator.SetBool("IsDoorOpen?", SetAnimOpen);
                animator.SetBool("IsDoorClose?", SetAnimClose);
            }
        }
    }

    public void Cant_Open_Door()
    {
        if (!DoorCollider.GetComponent<MoveCameraToNewScene>().IsCharacterEnter)
        {
            if (ExitDoorCollider.GetComponent<MoveCameraToNewScene>().IsCharacterEnter)
            {
                ExitDoorCollider.GetComponent<MoveCameraToNewScene>().IsPlayerCantOpenDoor = true;
            }
            else
            {
                GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Hide_Message();
            }
        }
    }
}
