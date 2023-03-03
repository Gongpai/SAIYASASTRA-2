using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class Button_Animation_Control : MonoBehaviour
{
    [SerializeField] public bool IsActive;
    [SerializeField] private Button button;
    [SerializeField] private GameObject Page;
    [SerializeField] private Essential_Menu PageSelect;

    private bool IsButActive;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        if (IsActive)
        {
            _animator.SetBool("Is_Play?", true);
            button.Select();
        }
    }

    void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        IsButActive = IsActive;
    }

    public void On_Select()
    {
        IsButActive = true;
    }

    public void De_Select()
    {
        IsButActive = false;
        _animator.SetBool("Is_Play?", false);
    }

    public void PlayAnimation_In()
    {
        _animator.SetBool("Is_Play?", true);
    }

    public void PlayAnimation_Out()
    {
        if(!IsButActive)
            _animator.SetBool("Is_Play?", false);
    }

    public void ButtonClick()
    {
        Page.SetActive(true);

        foreach (GameObject button_select in GameObject.FindGameObjectsWithTag("Button_Essential"))
        {
            Button_Animation_Control _buttonAnimationControl = button_select.GetComponent<Button_Animation_Control>();
            if (_buttonAnimationControl.IsButActive && (_buttonAnimationControl != this))
            {
                button_select.GetComponent<Button_Animation_Control>().De_Select();
                button_select.GetComponent<Button_Animation_Control>().Page.SetActive(false);
            }
        }

        switch (PageSelect)
        {
            case Essential_Menu.Inventory:
                GameInstance.Player.GetComponent<Inventory_System>().Set_Inventory_Element();
                GameInstance.Player.GetComponent<Inventory_System>().Set_Item_Element();
                GameInstance.Player.GetComponent<Note_System>().PlayAnim(false);
                GameInstance.Player.GetComponent<Inventory_System>().PlayAnim(true);
                break;
            case Essential_Menu.Craft:
                GameInstance.Player.GetComponent<Craft_System>().Set_Craft_Inventory_Element();
                GameInstance.Player.GetComponent<Note_System>().PlayAnim(false);
                GameInstance.Player.GetComponent<Craft_System>().PlayAnim(true);
                break;
            case Essential_Menu.Note:
                GameInstance.Player.GetComponent<Note_System>().Set_Note_Element();
                GameInstance.Player.GetComponent<Note_System>().PlayAnim(true);
                break;
        }

        On_Select();
    }
}
