using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death_Ui : MonoBehaviour
{
    public void Re_Game()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
