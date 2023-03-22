using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Ui : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreenWidget;
    [SerializeField] private GameObject Bg;

    bool Is_ReGame;
    [SerializeField]private Animator animator;
    [SerializeField]private Animator animator_bg;
    [SerializeField] bool CanSetActive = true;

    bool Is_Backtomainmenu = false;

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
        if (CanSetActive)
        {
            Bg.SetActive(true);
            animator_bg.SetBool("IsIn", true);
            animator_bg.SetBool("IsOut", false);
            PlayAnim(true);
        }
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
            print("PlayyyyyyyyyyyyyAnimmmmmmm");
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
        if (GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect != null)
        {
            if (GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect.GetComponent<Animator>().GetBool("IsPlay"))
                GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect.SetActive(true);
        }

        Game_State_Manager.Instance.Setstate(GameState.Play);
    }

    public void Back_To_MainMenu()
    {
        Is_Backtomainmenu = true;
        LoadingScreenWidget.GetComponent<LoadingSceneStstem>().LoadScene("MainMenu");
        Game_State_Manager.Reset_Game_State();
        Is_ReGame = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (Is_Backtomainmenu)
        {
            Make_DontDestroyOnLoad.Destroy_GameInstance();
        }

    }
}
