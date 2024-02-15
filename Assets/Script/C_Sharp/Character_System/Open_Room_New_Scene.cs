using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Room_New_Scene : MonoBehaviour
{
    [SerializeField] bool IsOpenScene = false;
    [SerializeField] string Scene = "Game_Level";
    [SerializeField] string m_showText = "เข้าไป";
    [SerializeField] GameObject ObjectSpawnlocation;
    [SerializeField] GameObject LoadingScreenWidget;
    [SerializeField] bool CanDestoryGameInstance = false;
    [SerializeField] bool Bypass = false;
    [SerializeField] bool Can_Open_Door_When_Ghost_Dead = false;

    public bool Cant_OpenDoor = false;
    bool IsCharacterEnter = false;
    Switch_Scene switch_Scene;
    bool Is_Backtomainmenu = false;
    private string ShowText
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
                return $"[E] {m_showText}";
            else
                return m_showText;
        }
    }

    private void Start()
    {
        if (IsOpenScene && !Bypass)
            Cant_OpenDoor = true;
    }

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
            //print("Error Not Component");
        }

        if (Can_Open_Door_When_Ghost_Dead && GameInstance.Ghost != null)
        {
            if(GameInstance.Ghost.GetComponent<Ai_Movement>().HP_Ghost <= 0)
            {
                Cant_OpenDoor = false;
            }
            else
            {
                Cant_OpenDoor = true;
            }
        }
    }

    public void Enter_Door()
    {
        if (IsCharacterEnter)
        {
            if (IsOpenScene)
            {
                if (!Cant_OpenDoor)
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
            if (!Cant_OpenDoor)
            {
                GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message(ShowText);
            }
            else
            {
                GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message("ประตูล็อค");
            }
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
