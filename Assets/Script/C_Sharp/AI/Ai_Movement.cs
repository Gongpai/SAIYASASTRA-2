using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Ai_Movement : FuntionLibraly
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
    [SerializeField] public float Damage = 50;
    [SerializeField] AudioSource audioSource;

    public delegate void Clear_All_Effect();
    public static Clear_All_Effect Clear_All;

    //TIME
    [Range(0.1f, 5.0f)] public float AttackSpeed = 0;
    private float timeattack;
    private float oldtimeattack;

    private bool IsplayAudio = false;
    private bool PlaylightBeginOnce = false;
    private bool PlaylightDeathOnce = false;

    void Awake()
    {
        Pos = transform.position;
        Game_State_Manager.Instance.OnGameStateChange += OnGamestateChanged;
    }
    void OnEnable()
    {
        GamePause_Component(gameObject, false);
    }

    void OnDisable()
    {
        GamePause_Component(gameObject, true);
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
        Puzzle_System.IsPuzzleSucceed = false;
        distanceVector3[0] = gameObject.transform.position;
        GameInstance.Ghost = gameObject;
        HP_Ghost = MaxHP;
        FuntionLibraly.ProgressBar_Fill(ProgressBar, HP_Ghost, MaxHP);
        timeattack = 0;
    }

    // Update is called once per frame
    [Obsolete]
    void Update()
    {
        if (IsAttackCharacter && !IsGhostStun)
        {
            timeattack += Time.deltaTime * AttackSpeed;

            if (timeattack >= 1)
            {
                print(timeattack + " : Old " + oldtimeattack);

                switch (GetComponent<Variables>().declarations.Get<AiGhost>("Ai_Ghost"))
                {
                    case AiGhost.Hungry_ghost:
                        GetComponent<Ai_Attack>().Attack(AiGhost.Hungry_ghost);
                        break;
                    case AiGhost.Home_ghost:

                        GetComponent<Ai_Attack>().Attack(AiGhost.Home_ghost);
                        PlaySound(true);
                        break;
                    case AiGhost.Guard_ghost:
                        break;
                    case AiGhost.Kid_ghost:

                        break;
                    case AiGhost.Mannequin_ghost:
                        break;
                    case AiGhost.Soi_Ju_ghost:
                        GetComponent<Ai_Attack>().Attack(AiGhost.Soi_Ju_ghost);
                        break;
                }

                timeattack = 0;
            }
        }
        else
        {
            timeattack = 0;
        }

        Ai_movement();
        
        distanceVector3[0] = gameObject.transform.position;

        if(GetComponent<Variables>().declarations.Get<AiGhost>("Ai_Ghost") != AiGhost.Home_ghost)
        {
            DetectCharacter();
            
        }

        AttackCharacter();

        //print("See    : " + IsSeeCharacter + " Location : " + (Camera.main.WorldToScreenPoint(GameInstance.Ghost.transform.position).x >= 0) + (Camera.main.WorldToScreenPoint(GameInstance.Ghost.transform.position).x <= Screen.width));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger && other.tag == "Attack_Item")
        {
            if(GetComponent<Variables>().declarations.Get<AiGhost>("Ai_Ghost") != AiGhost.Mannequin_ghost)
            {
                Flashing_Lights.playAnimLight?.Invoke(true, true);
            }
            
            if(other.GetComponent<Item_Attack_System>().ghost != gameObject)
                HP_System(other);
        }
    }

    public void PlaySound(bool IsPLay)
    {
        print("On Play Sound");
        if (IsPLay)
        {
            if (!IsplayAudio)
            {
                print("PlaySound");
                audioSource.Play();
                IsplayAudio = true;
            }
        } else
        {
            print("Stop Sound");
            audioSource.Stop();
        }
    }

    private void HP_System(Collider other)
    {
        if (other.GetComponent<Item_Attack_System>() != null)
        {
            float Damage = other.GetComponent<Variables>().declarations.Get<float>("Damage");
            print("Hp------- : " + Damage);

            if (HP_Ghost > 0)
            {
                HP_Ghost -= Damage;

                if(HP_Ghost <= 0)
                {
                    HP_Ghost = 0;
                    if (!PlaylightDeathOnce)
                    {
                        Flashing_Lights.playAnimLight?.Invoke(false, true);
                        PlaylightDeathOnce = true;
                    }
                }
                    
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
            if ((distanceVector3[1].x - distanceVector3[0].x) * 100 <= -1f)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            else if ((distanceVector3[1].x - distanceVector3[0].x) * 100 >= 1f)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        }
        else
        {
            if((gameObject.transform.position.x - Player.transform.position.x) * 100 <= -1f)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            else if ((gameObject.transform.position.x - Player.transform.position.x) * 100 >= 1f)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        //ตัวละครเดินขึ้นหน้าลงหลัง
        if (!IsSeeCharacter)
        {
            if ((distanceVector3[1].z - distanceVector3[0].z) * 100 <= -1f)
                this.GetComponent<Animator>().SetBool("IsWalkForward", false);
            else if ((distanceVector3[1].z - distanceVector3[0].z) * 100 >= 1f)
                this.GetComponent<Animator>().SetBool("IsWalkForward", true);
            
        }
        else
        {
            if ((gameObject.transform.position.z - Player.transform.position.z) * 10 <= -1f)
                this.GetComponent<Animator>().SetBool("IsWalkForward", true);
            else if ((gameObject.transform.position.z - Player.transform.position.z) * 10 >= 1f)
                this.GetComponent<Animator>().SetBool("IsWalkForward", false);
        }
        //print("Dis X" + (distanceVector3[1].x - distanceVector3[0].x));
        //print("Dis Z" + (distanceVector3[1].z - distanceVector3[0].z));
    }

    void DetectCharacter()
    {
        if (Camera.main.WorldToScreenPoint(transform.position).x >= 0 && Camera.main.WorldToScreenPoint(transform.position).x <= Screen.width && !GameInstance.CharacterHide)
        {
            Player = GameInstance.Player;

            if (GetComponent<Variables>().declarations.Get<AiGhost>("Ai_Ghost") != AiGhost.Mannequin_ghost)
            {
                IsSeeCharacter = true;
                if (!PlaylightBeginOnce)
                {
                    //print("Ghost See");
                    Flashing_Lights.playAnimLight?.Invoke(false, true);
                    PlaySound(true);
                }
            }
            else
            {
                if (!FindSeeAllCharacterGhost().Item1)
                {
                    //print("Ghost See Mannequin");
                    PlaySound(true);
                }
                IsSeeCharacter = true;
                Flashing_Lights.event_Light_On_Off?.Invoke(Flashing_Lights.Light_Mode.Turn_Off);
            }
        }
        else
        {
            if (IsSeeCharacter)
            {
                if (CountSeeAllCharacterGhost() <= 1)
                {
                    foreach(GameObject ghost in GameObject.FindGameObjectsWithTag("Ghost"))
                    {
                        if (ghost.GetComponent<AudioSource>().isPlaying)
                        {
                            ghost.GetComponent<Ai_Movement>().PlaySound(false);
                            break;
                        }
                    }
                }

                IsSeeCharacter = false;
            }

            //print("Ghost dont see");
            IsplayAudio = false;
            IsSeeCharacter = false;
        }
    }
    private Tuple<bool, GameObject> FindSeeAllCharacterGhost()
    {
        GameObject[] AllGhost = GameObject.FindGameObjectsWithTag("Ghost");
        bool IsSee = false;
        GameObject Ghost_IsSee = null;

        foreach (GameObject ghost in AllGhost)
        {
            if (ghost.GetComponent<Ai_Movement>().IsSeeCharacter)
            {
                IsSee = true;
                Ghost_IsSee = ghost;
                break;
            }
        }
        
        return new Tuple<bool, GameObject>(IsSee, Ghost_IsSee);
    }

    private int CountSeeAllCharacterGhost()
    {
        GameObject[] AllGhost = GameObject.FindGameObjectsWithTag("Ghost");
        int i = 0;

        foreach (GameObject ghost in AllGhost)
        {
            if (ghost.GetComponent<Ai_Movement>().IsSeeCharacter)
            {
                i++;
            }
        }

        return i;
    }

    public void AttackCharacter()
    {
        switch (GetComponent<Variables>().declarations.Get<AiGhost>("Ai_Ghost"))
        {
            case AiGhost.Hungry_ghost:
                CanAttackCHaracter();
                break;
            case AiGhost.Home_ghost:
                if (Puzzle_System.IsPuzzleSucceed != default && Puzzle_System.IsPuzzleSucceed)
                {
                    IsAttackCharacter = false;
                    print("Dont Play Sound");
                    PlaySound(false);
                    if (GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect != null)
                    {
                        GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect.GetComponent<Animator>().SetBool("IsPlay", false);
                        GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect.SetActive(false);
                    }
                }
                break;
            case AiGhost.Guard_ghost:
                CanAttackCHaracter();
                break;
            case AiGhost.Kid_ghost:
                break;
            case AiGhost.Mannequin_ghost:
                break;
            case AiGhost.Soi_Ju_ghost:
                CanAttackCHaracter();
                break;
        }
    }

    void CanAttackCHaracter()
    {
        if (Camera.main.WorldToScreenPoint(transform.position).x >= 150 && Camera.main.WorldToScreenPoint(transform.position).x <= (Screen.width - 150) && !GameInstance.CharacterHide)
        {
            IsAttackCharacter = true;
        }
        else
        {
            IsAttackCharacter = false;
        }
    }
    
    public void CanAttackGhostHome()
    {
        print("Can Attack");
        IsAttackCharacter = true;
        IsGhostStun = false;
    }
}
