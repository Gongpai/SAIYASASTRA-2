using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Door_Lawson_System : MonoBehaviour
{
    public Animator animator;

    [SerializeField] private GameObject DoorCollider;
    [SerializeField] private GameObject ExitDoorCollider;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenOrClose(bool SetAnimOpen = false, bool SetAnimClose = false, bool IsSet = false)
    {
        if (DoorCollider.GetComponent<MoveCameraToNewScene>().IsCharacterEnter)
        {
            GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message("[E] ª‘¥ª√–µŸ");
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
                    GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message("[E] ‡ª‘¥ª√–µŸ");
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
                GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message("ª√–µŸ≈ÁÕ§");
            }
            else
            {
                GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Hide_Message();
            }
        }
    }
}
