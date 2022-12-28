using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cupboard_Hide : MonoBehaviour
{
    [SerializeField] private GameObject Door;
    [SerializeField] private string InteractMessage;
    [SerializeField] private string HideMessage;
    private Animator animator;

    private bool CharacterEnter = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = Door.GetComponent<Animator>();
    }

    public void DoorOpen(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CharacterEnter == true)
        {
            animator.SetBool("CharacterEnter?", !animator.GetBool("CharacterEnter?"));
            animator.SetBool("IsDoorOpen?", !animator.GetBool("IsDoorOpen?"));
            animator.SetBool("IsDoorClose?", !animator.GetBool("IsDoorClose?"));
            GameInstance.CharacterHide = !GameInstance.CharacterHide;
            switch (GameInstance.CharacterHide)
            {
                case true:
                    ShowMessage.showMessageEvent?.Invoke(HideMessage);
                    break;
                case false:
                    ShowMessage.showMessageEvent?.Invoke(InteractMessage);
                    break;
                default:
                    break;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        { 
            CharacterEnter = true;
            ShowMessage.showMessageEvent?.Invoke(InteractMessage);
            print("DDDDDDD");
        }
           
        
        if (GameInstance.CharacterHide && collider.gameObject.tag == "Player")
            GameInstance.CharacterHide = false;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            animator.SetBool("IsDoorOpen?", false);
            animator.SetBool("IsDoorClose?", true);
            CharacterEnter = false;
            ShowMessage.hideMessageEvent?.Invoke();
        }
    }
}
