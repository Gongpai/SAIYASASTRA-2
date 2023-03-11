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
    bool IsCharacterEnter = false;
    Switch_Scene switch_Scene;

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
}
