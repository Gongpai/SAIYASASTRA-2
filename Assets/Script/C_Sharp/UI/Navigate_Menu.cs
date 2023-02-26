using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigate_Menu : MonoBehaviour
{
    [SerializeField] private List<GameObject> ButtonPage;

    public void OpenPage(int Page)
    {
        ButtonPage[Page].GetComponent<Button_Animation_Control>().IsActive = true;
        ButtonPage[Page].GetComponent<Button_Animation_Control>().ButtonClick();
        ButtonPage[Page].GetComponent<Button_Animation_Control>().PlayAnimation_In();
    }

    public void SetGamePause()
    {
        Game_State_Manager.Instance.Setstate(GameState.Play);
    }
}
