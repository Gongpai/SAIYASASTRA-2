using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death_Ui : MonoBehaviour
{
    public void Re_Game()
    {
        Game_State_Manager.Instance.Setstate(GameState.Play);
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Game_State_Manager.Instance.Setstate(GameState.Pause);
    }
}
