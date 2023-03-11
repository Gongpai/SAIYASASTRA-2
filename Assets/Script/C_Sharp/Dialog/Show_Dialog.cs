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
    public ButtonEvent OnSpawnAi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (Can_Spawn_Ai_Ghost)
            {
                OnSpawnAi.Invoke();
            }
                
            DialogWidget.GetComponent<Dialog>().SceneNum = SceneNum;
            Instantiate(DialogWidget);
            Game_State_Manager.Instance.Setstate(GameState.Pause);
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
