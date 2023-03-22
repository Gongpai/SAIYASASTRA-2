using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigate_Menu : MonoBehaviour
{
    [SerializeField] private List<GameObject> ButtonPage;
    public GameObject Menu;

    public void OpenPage(int Page)
    {
        ButtonPage[Page].GetComponent<Button_Animation_Control>().IsActive = true;
        ButtonPage[Page].GetComponent<Button_Animation_Control>().ButtonClick();
        ButtonPage[Page].GetComponent<Button_Animation_Control>().PlayAnimation_In();
    }

    public void SetGamePause()
    {
        if (GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect != null)
        {
            if(GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect.GetComponent<Animator>().GetBool("IsPlay"))
                GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect.SetActive(true);
        }
            
        Game_State_Manager.Instance.Setstate(GameState.Play);
    }

    public void Back_To_Menu(bool SetActive)
    {
        gameObject.SetActive(!SetActive);
        Menu.SetActive(SetActive);
        if (Menu.name == "UI")
        {
            Debug.Log("UI : ", Menu);
            Menu.transform.parent.GetComponent<Death_Ui>().PlayAnim(true);
        }
    }

    public void Set_Back_To_Menu(GameObject menu)
    {
        gameObject.SetActive(true);
        Menu = menu;
    }
}
