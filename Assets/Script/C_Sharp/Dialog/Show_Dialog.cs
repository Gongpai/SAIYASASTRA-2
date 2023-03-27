using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Show_Dialog : MonoBehaviour
{
    [Header ("Dialog")]
    [SerializeField]
    private GameObject DialogWidget;

    [SerializeField] private int SceneNum;

    [Header("Spawn Ai")]
    [SerializeField] bool Can_Spawn_Ai_Ghost = false;
    [Serializable] public class ButtonEvent : UnityEvent { };
    bool IsSpawn = true;
    public ButtonEvent OnBeginDialog;
    public ButtonEvent OnSpawnAi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GameObject DialogSpawn;
            DialogWidget.GetComponent<Dialog>().SceneNum = SceneNum;
            DialogSpawn = Instantiate(DialogWidget);
            DialogSpawn.GetComponent<Dialog>().OnStart.AddListener(OnBegin);
            DialogSpawn.GetComponent<Dialog>().OnEnd.AddListener(SpawnAi);
            Game_State_Manager.Instance.Setstate(GameState.Pause);
        }
    }
    public void OnBegin()
    {
        OnBeginDialog.Invoke();
    }


    public void SpawnAi()
    {
        print("CanSpawn");
        if (Can_Spawn_Ai_Ghost && IsSpawn)
        {
            OnSpawnAi.Invoke();
            IsSpawn = false;
        }
        Destroy(this.gameObject);
    }

    public void TurnOffAllLight()
    {
        Flashing_Lights.event_Light_On_Off(Flashing_Lights.Light_Mode.Turn_Off);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
