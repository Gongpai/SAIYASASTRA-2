using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Ui : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreenWidget;
    [SerializeField] private GameObject Bg;

    bool Is_ReGame;
    [SerializeField]private Animator animator;
    private Animator animator_bg;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        animator_bg = Bg.GetComponent<Animator>();
        Bg.SetActive(true);
        animator_bg.SetBool("IsIn", true);
        animator_bg.SetBool("IsOut", false);
        PlayAnim(true);
    }
    private void OnDisable()
    {
        if (Is_ReGame)
        {
            GameInstance.Reset_Gameinstance();
        }
    }

    public void PlayAnim(bool IsPlayIn)
    {
        if (IsPlayIn)
        {
            animator.SetBool("IsIn", true);
            animator.SetBool("IsOut", false);
        }
        else
        {
            animator.SetBool("IsIn", false);
            animator.SetBool("IsOut", true);
        }
    }

    public void Play()
    {
        LoadingScreenWidget.GetComponent<LoadingSceneStstem>().LoadScene("Game_Level");
    }

    public void Resume()
    {
        animator_bg.SetBool("IsIn", false);
        animator_bg.SetBool("IsOut", true);
        PlayAnim(false);
        Game_State_Manager.Instance.Setstate(GameState.Play);
    }

    public void Back_To_MainMenu()
    {
        LoadingScreenWidget.GetComponent<LoadingSceneStstem>().LoadScene("MainMenu");
        Game_State_Manager.Reset_Game_State();
        Is_ReGame = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
