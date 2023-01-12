using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class Button_Animation_Control : MonoBehaviour
{
    [SerializeField] private bool IsActive = false;
    [SerializeField] private Button button;

    private bool IsButActive;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();

        if (IsActive)
        {
            _animator.SetBool("Is_Play?", true);
            button.Select();
        }

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
        foreach (GameObject button_select in GameObject.FindGameObjectsWithTag("Button_Essential"))
        {
            Button_Animation_Control _buttonAnimationControl = button_select.GetComponent<Button_Animation_Control>();
            if (_buttonAnimationControl.IsButActive && (_buttonAnimationControl != this))
            {
                button_select.GetComponent<Button_Animation_Control>().De_Select();
            }
        }

        On_Select();
    }
}
