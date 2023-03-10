using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Scene : MonoBehaviour
{
    [SerializeField]Animator animator;
    public delegate void NewLocation();
    public event NewLocation Switch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnSwitchScene()
    {
        animator.SetBool("IsPlay", true);
        Game_State_Manager.Instance.Setstate(GameState.Pause);
    }

    public void OnSwitch()
    {
        Switch?.Invoke();
        Switch = null;
    }

    public void OnEnd()
    {
        animator.SetBool("IsPlay", false);
        gameObject.SetActive(false);
        Game_State_Manager.Instance.Setstate(GameState.Play);
    }
}
