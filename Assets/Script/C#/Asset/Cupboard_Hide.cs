using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cupboard_Hide : MonoBehaviour
{
    [SerializeField] private GameObject Door;
    [SerializeField] private string InteractMessage;
    [SerializeField] private string HideMessage;

    private Animator animator;

    public bool CharacterInside = false;
    private bool CharacterEnter = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = Door.GetComponent<Animator>();
    }

    void Update()
    {
        if (CharacterInside && CharacterEnter && GameInstance.CharacterHide == false && animator.GetBool("IsDoorClose?") && !GameInstance.Ghost.GetComponent<Ai_Movement>().IsSeeCharacter)
        {
                GameInstance.CharacterHide = true;
                GameInstance.Player.GetComponent<Player_Movement>().ObjectHide = gameObject;
        }
    }

    //â¤é´à»Ô´»ÃÐµÙ
    public void DoorOpen(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CharacterEnter == true)
        {
            animator.SetBool("CharacterEnter?", !animator.GetBool("CharacterEnter?"));
            animator.SetBool("IsDoorOpen?", !animator.GetBool("IsDoorOpen?"));
            animator.SetBool("IsDoorClose?", !animator.GetBool("IsDoorClose?"));

            if (CharacterInside && animator.GetBool("IsDoorClose?") == true && CharacterInside && animator.GetBool("IsDoorOpen?") == false && !GameInstance.Ghost.GetComponent<Ai_Movement>().IsSeeCharacter)
            {
                GameInstance.CharacterHide = true;
                GameInstance.Player.GetComponent<Player_Movement>().ObjectHide = gameObject;
            }
            else
            {
                GameInstance.CharacterHide = false;
                GameInstance.Player.GetComponent<Player_Movement>().ObjectHide = null;
            }

            if (!CharacterInside)
                GameInstance.CharacterHide = false;

            print(CharacterEnter + "  " + GameInstance.CharacterHide + "  " + CharacterInside);

            switch (GameInstance.CharacterHide)
            {
                case true:
                    ShowMessage.showMessageEvent?.Invoke(HideMessage);
                    break;
                case false:
                    ShowMessage.showMessageEvent?.Invoke(InteractMessage);
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
            GameInstance.CharacterHide = false;
        }
    }
}
