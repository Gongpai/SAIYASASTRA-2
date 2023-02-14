using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Ai_Movement : MonoBehaviour
{
    private InputManager inputManager;
    public bool IsSeeCharacter;
    public bool IsAttackCharacter;
    public bool IsGhostStun = false;

    private GameObject Player;
    public List<GameObject> hideGameObject;

    private Vector3 velocity, Pos;
    private Vector3[] distanceVector3 = new Vector3[2];

    [SerializeField] private Image ProgressBar;
    [SerializeField] private int MaxHP = 100;
    [SerializeField] public float HP_Ghost;

    //TIME
    private float timeattack = 1;
    private float oldtimeattack;

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
        //Debug.LogWarning(gameState + "----------------------------------------------------");

        gameObject.GetComponent<StateMachine>().enabled = gameState == GameState.Play;

        if(gameState == GameState.Pause)
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;

        if (gameState == GameState.Pause && gameObject.GetComponent<Variables>().declarations["IsRunFirstState"].Equals(true))
            gameObject.GetComponent<Variables>().declarations.Set("IsSkipFirstState", true);

        enabled = gameState == GameState.Play;
    }

    // Start is called before the first frame update
    void Start()
    {
        distanceVector3[0] = gameObject.transform.position;
        GameInstance.Ghost = gameObject;
        HP_Ghost = MaxHP;
        FuntionLibraly.ProgressBar_Fill(ProgressBar, HP_Ghost, MaxHP);
        oldtimeattack = DateTime.Now.Second;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAttackCharacter && !IsGhostStun)
        {
            timeattack = DateTime.Now.Second;
            if ((timeattack - oldtimeattack) == 1)
            {
                print(timeattack + " : Old " + oldtimeattack);
                GetComponent<Ai_Attack>().Attack(AiGhost.Guard_ghost);
            }

            oldtimeattack = DateTime.Now.Second;
        }
        else
        {
            oldtimeattack = DateTime.Now.Second;
        }

        Ai_movement();
        
        distanceVector3[0] = gameObject.transform.position;

        DetectCharacter();

        AttackCharacter();

        //print("See    : " + IsSeeCharacter + " Location : " + (Camera.main.WorldToScreenPoint(GameInstance.Ghost.transform.position).x >= 0) + (Camera.main.WorldToScreenPoint(GameInstance.Ghost.transform.position).x <= Screen.width));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger && other.tag == "Attack_Item")
        {
            HP_System(other);
        }
    }

    private void HP_System(Collider other)
    {
        if (other.GetComponent<Add_item_to_character>() != null && other.GetComponent<Add_item_to_character>().IsSpawn)
        {
            float Damage = other.GetComponent<Variables>().declarations.Get<float>("Damage");
            print("Hp------- : " + Damage);

            if (HP_Ghost > 0)
            {
                HP_Ghost -= Damage;

                if(HP_Ghost <= 0)
                    HP_Ghost = 0;
            }
            
            FuntionLibraly.ProgressBar_Fill(ProgressBar, HP_Ghost, MaxHP);
            other.GetComponent<Item_Script>().Destroy_Item();
        }

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

    void AttackCharacter()
    {
        if (Camera.main.WorldToScreenPoint(GameInstance.Ghost.transform.position).x >= 150 && Camera.main.WorldToScreenPoint(GameInstance.Ghost.transform.position).x <= (Screen.width - 150) && !GameInstance.CharacterHide)
        {
            IsAttackCharacter = true;
        }
        else
        {
            IsAttackCharacter = false;
        }
    }
}
