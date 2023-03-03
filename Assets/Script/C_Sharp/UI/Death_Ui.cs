using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death_Ui : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreenWidget;
    [SerializeField] private GameObject bg;

    Animator animator;
    bool Is_ReGame;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void Re_Game()
    {
        Game_State_Manager.Instance.Setstate(GameState.Play);
        LoadingScreenWidget.GetComponent<LoadingSceneStstem>().LoadScene("Game_Level");
        Is_ReGame = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenSetting()
    {
        PlayAnim(false);
    }
    public void DeathOpen()
    {
        bg.GetComponent<Animator>().SetBool("IsIn", true);
        bg.GetComponent<Animator>().SetBool("IsOut", false);
    }

    public void PauseGame()
    {
        Game_State_Manager.Instance.Setstate(GameState.Pause);
    }

    public void Back_To_MainMenu()
    {
        LoadingScreenWidget.GetComponent<LoadingSceneStstem>().LoadScene("MainMenu");
        Game_State_Manager.Reset_Game_State();
        Is_ReGame = true;
    }

    public void PlayAnim(bool IsPlayIn)
    {
        if (IsPlayIn)
        {
            animator.SetBool("Is_In", true);
            animator.SetBool("Is_Out", false);
        }
        else
        {
            animator.SetBool("Is_In", false);
            animator.SetBool("Is_Out", true);
            animator.SetBool("Is_Death", false);
        }
    }

    private void OnEnable()
    {
        Game_State_Manager.Instance.Setstate(GameState.Pause);
        GetComponent<AudioSource>().Play();
    }

    private void OnDisable()
    {
        if(Is_ReGame)
            GameInstance.Reset_Gameinstance();
    }
}
