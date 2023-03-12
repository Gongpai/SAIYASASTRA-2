using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Room_New_Scene : MonoBehaviour
{
    [SerializeField] bool IsOpenScene = false;
    [SerializeField] string Scene = "Game_Level";
    [SerializeField] string ShowText = "[E] เข้าไป";
    [SerializeField] GameObject ObjectSpawnlocation;
    [SerializeField] GameObject LoadingScreenWidget;
    [SerializeField] bool CanDestoryGameInstance = false;

    bool IsCharacterEnter = false;
    Switch_Scene switch_Scene;
    bool Is_Backtomainmenu = false;

    private void Update()
    {
        try
        {
            if (GameInstance.Player.GetComponent<Player_Movement>().Switch_Scene.GetComponent<Switch_Scene>() != null)
            {
                switch_Scene = GameInstance.Player.GetComponent<Player_Movement>().Switch_Scene.GetComponent<Switch_Scene>();
            }
        }
        catch
        {
            print("Error Not Component");
        }
    }

    public void Enter_Door()
    {
        if (IsCharacterEnter)
        {
            if (IsOpenScene)
            {
                GameInstance.Player.GetComponent<Player_Movement>().OnSwitchScene();
                switch_Scene.Switch += SetNewScene;
                switch_Scene.OnSwitchScene();

                if (CanDestoryGameInstance)
                {
                    Is_Backtomainmenu = true;
                    Game_State_Manager.Reset_Game_State();
                }
            }
            else
            {
                GameInstance.Player.GetComponent<Player_Movement>().OnSwitchScene();
                switch_Scene.Switch += SetNewLocation;
                switch_Scene.OnSwitchScene();
            }
        }
    }

    public void SetNewLocation()
    {
        GameInstance.SpawnLocation = ObjectSpawnlocation.transform.position;
        GameInstance.Player.transform.position = GameInstance.SpawnLocation;

    }

    public void SetNewScene()
    {
        LoadingScreenWidget.GetComponent<LoadingSceneStstem>().LoadScene(Scene);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message(ShowText);
            IsCharacterEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Hide_Message();
            IsCharacterEnter = false;
        }
    }

    private void OnDestroy()
    {
        if (Is_Backtomainmenu)
        {
            GameInstance.Reset_Gameinstance();
            Make_DontDestroyOnLoad.Destroy_GameInstance();
        }

    }
}
