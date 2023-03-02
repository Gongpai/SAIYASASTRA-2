using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Ui : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreenWidget;
    [SerializeField] private GameObject Bg;

    bool Is_ReGame;
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
        if(Bg != null)
            Bg.SetActive(true);
    }
    private void OnDisable()
    {
        if (Is_ReGame)
            GameInstance.Reset_Gameinstance();
    }

    public void Play()
    {
        LoadingScreenWidget.GetComponent<LoadingSceneStstem>().LoadScene("Game_Level");
    }

    public void Resume()
    {
        Bg.SetActive(false);
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
