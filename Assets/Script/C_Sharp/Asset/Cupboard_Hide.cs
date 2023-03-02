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

    public Animator animator;

    public bool CharacterInside = false;
    private bool CharacterEnter = false;

    public delegate void OpenDoor_Hide();
    public static OpenDoor_Hide Open_Door_Hide;

    private ShowMessage pLayer;
    // Start is called before the first frame update
    void Start()
    {
        animator = Door.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Open_Door_Hide += Touch_TriggerOpenDoor;
    }

    private void OnDisable()
    {
        Open_Door_Hide -= Touch_TriggerOpenDoor;
    }

    void Update()
    {
        if (GameInstance.Ghost != null)
        {
            if (CharacterInside && CharacterEnter && GameInstance.CharacterHide == false &&
                animator.GetBool("IsDoorClose?") && !GameInstance.Ghost.GetComponent<Ai_Movement>().IsSeeCharacter)
            {
                GameInstance.CharacterHide = true;
                GameInstance.Player.GetComponent<Player_Movement>().ObjectHide = gameObject;
            }
        }
        else
        {
            if (CharacterInside && CharacterEnter && GameInstance.CharacterHide == false &&
                animator.GetBool("IsDoorClose?"))
            {
                GameInstance.CharacterHide = true;
                GameInstance.Player.GetComponent<Player_Movement>().ObjectHide = gameObject;
            }
        }
    }

    //â¤é´à»Ô´»ÃÐµÙ
    public void TriggerOpenDoor()
    {
        if (CharacterEnter == true)
        {
            OpenDoor();
        }
    }

    private void Touch_TriggerOpenDoor()
    {
        if (CharacterEnter == true)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        animator.SetBool("CharacterEnter?", !animator.GetBool("CharacterEnter?"));
        animator.SetBool("IsDoorOpen?", !animator.GetBool("IsDoorOpen?"));
        animator.SetBool("IsDoorClose?", !animator.GetBool("IsDoorClose?"));

        if (GameInstance.Ghost != null)
        {
            if (CharacterInside && animator.GetBool("IsDoorClose?") == true && CharacterInside &&
                animator.GetBool("IsDoorOpen?") == false && !GameInstance.Ghost.GetComponent<Ai_Movement>().IsSeeCharacter)
            {
                GameInstance.CharacterHide = true;
                GameInstance.Player.GetComponent<Player_Movement>().ObjectHide = gameObject;
            }
            else
            {
                GameInstance.CharacterHide = false;
                GameInstance.Player.GetComponent<Player_Movement>().ObjectHide = null;
            }
        }
        else
        {
            if (CharacterInside && animator.GetBool("IsDoorClose?") == true && CharacterInside &&
                animator.GetBool("IsDoorOpen?") == false)
            {
                GameInstance.CharacterHide = true;
                GameInstance.Player.GetComponent<Player_Movement>().ObjectHide = gameObject;
            }
            else
            {
                GameInstance.CharacterHide = false;
                GameInstance.Player.GetComponent<Player_Movement>().ObjectHide = null;
            }
        }

        if (!CharacterInside)
            GameInstance.CharacterHide = false;

        switch (animator.GetBool("IsDoorOpen?"))
        {
            case true:
                pLayer.Show_Message(HideMessage);
                break;
            case false:
                pLayer.Show_Message(InteractMessage);
                break;
        }

        print(CharacterEnter + "  " + GameInstance.CharacterHide + "  " + CharacterInside);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        { 
            CharacterEnter = true;
            collider.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message(InteractMessage);
            pLayer = collider.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>();
            //print("DDDDDDD");
            collider.GetComponent<Player_Movement>().Set_Block_Use_item(true);
        }
           
        
        if (GameInstance.CharacterHide && collider.gameObject.tag == "Player")
            GameInstance.CharacterHide = false;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            animator.SetBool("IsDoorOpen?", false);
            animator.SetBool("IsDoorClose?", true);
            CharacterEnter = false;
            collider.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Hide_Message();
            GameInstance.CharacterHide = false;
            collider.GetComponent<Player_Movement>().Set_Block_Use_item(false);
        }
    }
}
