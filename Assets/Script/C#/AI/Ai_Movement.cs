using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Ai_Movement : MonoBehaviour
{
    private InputManager inputManager;
    public bool IsSeeCharacter;

    private GameObject Player;
    public List<GameObject> hideGameObject;

    private Vector3 velocity, Pos;
    private Vector3[] distanceVector3 = new Vector3[2];

    void Awake()
    {
        Pos = transform.position;
        Game_State_Manager.Instance.OnGameStateChange += OnGamestateChanged;
    }
    void OnEnable()
    {
        GetComponent<Animator>().enabled = true;
    }

    void OnDisable()
    {
        GetComponent<Animator>().enabled = false;
        
    }

    void OnDestroy()
    {
        Game_State_Manager.Instance.OnGameStateChange -= OnGamestateChanged;

        if(GameInstance.Ghost == gameObject)
            GameInstance.Ghost = null;
    }

    private void OnGamestateChanged(GameState gameState)
    {
        Debug.LogWarning(gameState + "----------------------------------------------------");

        gameObject.GetComponent<StateMachine>().enabled = gameState == GameState.Play;

        if(gameState == GameState.Pause && gameObject.GetComponent<Variables>().declarations["IsRunFirstState"].Equals(true))
            gameObject.GetComponent<Variables>().declarations.Set("IsSkipFirstState", true);

        enabled = gameState == GameState.Play;
    }

    // Start is called before the first frame update
    void Start()
    {
        distanceVector3[0] = gameObject.transform.position;
        GameInstance.Ghost = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Ai_movement();
        
        distanceVector3[0] = gameObject.transform.position;

        DetectCharacter();
        print("See    : " + IsSeeCharacter + " Location : " + (Camera.main.WorldToScreenPoint(GameInstance.Ghost.transform.position).x >= 0) + (Camera.main.WorldToScreenPoint(GameInstance.Ghost.transform.position).x <= Screen.width));
    }

    void Ai_movement()
    {//ความเร็วตัวละคร
        velocity = (transform.position - Pos) / Time.deltaTime;
        this.GetComponent<Animator>().SetFloat("Speed", velocity.magnitude);
        //Debug.Log("Speed is : " + velocity.magnitude);
        Pos = transform.position;

        //ตัวละครหันซ้ายขวา
        distanceVector3[1] = gameObject.transform.position;

        if (!IsSeeCharacter)
        {
            if ((distanceVector3[1].x - distanceVector3[0].x) < 0)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            else if ((distanceVector3[1].x - distanceVector3[0].x) > 0)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        }
        else
        {
            if((gameObject.transform.position.x - Player.transform.position.x) < 0)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            else if ((gameObject.transform.position.x - Player.transform.position.x) > 0)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        //ตัวละครเดินขึ้นหน้าลงหลัง
        if (!IsSeeCharacter)
        {
            if ((distanceVector3[1].z - distanceVector3[0].z) < 0)
                this.GetComponent<Animator>().SetBool("IsWalkForward", false);
            else if ((distanceVector3[1].z - distanceVector3[0].z) > 0)
                this.GetComponent<Animator>().SetBool("IsWalkForward", true);
            
        }
        else
        {
            if ((gameObject.transform.position.z - Player.transform.position.z) < 0)
                this.GetComponent<Animator>().SetBool("IsWalkForward", true);
            else if ((gameObject.transform.position.z - Player.transform.position.z) > 0)
                this.GetComponent<Animator>().SetBool("IsWalkForward", false);
        }
        //print("Dis X" + (distanceVector3[1].x - distanceVector3[0].x));
        //print("Dis Z" + (distanceVector3[1].z - distanceVector3[0].z));
    }

    void DetectCharacter()
    {
        if (Camera.main.WorldToScreenPoint(GameInstance.Ghost.transform.position).x >= 0 && Camera.main.WorldToScreenPoint(GameInstance.Ghost.transform.position).x <= Screen.width && !GameInstance.CharacterHide)
        {
            IsSeeCharacter = true;
            Player = GameInstance.Player;
        }
        else
        {
            IsSeeCharacter = false;
        }
    }
}
