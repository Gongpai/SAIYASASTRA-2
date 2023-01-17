using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float WalkSpeed = 1;

    [SerializeField] private float RunSpeed = 3;

    [SerializeField] private MovementModes movementMode = MovementModes.FreeRoam;

    [SerializeField] private  GameObject Playerinput;

    [SerializeField] public ShowMessage showMessage;

    [SerializeField] public TextMeshProUGUI TextDebug;

    [SerializeField] public GameObject Essential_Menu;

    [Header("NotSet")]
    float moveSpeed = 0;
    public PlayerInput playerInput;
    private InputAction JumpAction, RunAction, InventoryAction, NoteAction, CraftAction, aimAction, useitemAction, shootAction;
    private InputManager inputManager;

    private Vector3 velocity, Pos;

    public GameObject ObjectHide;

    private enum MovementModes
    {
        MoveHorizontally,
        MoveVertically,
        FreeRoam
    };

    void Awake()
    {
        Pos = transform.position;
        playerInput = Playerinput.GetComponent<PlayerInput>();
        JumpAction = playerInput.actions["Jump"];
        RunAction = playerInput.actions["Run"];
        InventoryAction = playerInput.actions["Inventory"];
        NoteAction = playerInput.actions["Note"];
        CraftAction = playerInput.actions["Craft"];
        aimAction = playerInput.actions["Aim"];
        useitemAction = playerInput.actions["Use_Item"];
        shootAction = playerInput.actions["Shoot"]; 
        Game_State_Manager.Instance.OnGameStateChange += OnGamestateChanged;
    }

    void OnDestroy()
    {
        Game_State_Manager.Instance.OnGameStateChange -= OnGamestateChanged;
    }

    void OnEnable()
    {
        JumpAction.Enable();
        RunAction.Enable();
        InventoryAction.Enable();
        NoteAction.Enable();
        CraftAction.Enable();
        aimAction.Enable();
        useitemAction.Enable();
        shootAction.Enable();
        GetComponent<Animator>().enabled = true;
    }

    void OnDisable()
    {
        JumpAction.Disable();
        RunAction.Disable();
        InventoryAction.Disable();
        NoteAction.Disable();
        CraftAction.Disable();
        aimAction.Disable();
        useitemAction.Disable();
        shootAction.Disable();
        GetComponent<Animator>().enabled = false;
    }

    private void OnGamestateChanged(GameState gameState)
    {
        enabled = gameState == GameState.Play;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (inputManager == null)
        {
            inputManager = InputManager.instance;
        }

        if (inputManager == null)
        {
            Debug.LogWarning("No player");
        }

        GameInstance.Player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Character_movement();

        Ui_Control();
    }

    void Character_movement()
    {
        //����ǹ�ͧ�������͹������Ф�
        if (movementMode == MovementModes.FreeRoam)
        {
            Vector3 movementVector = new Vector3(inputManager.horizontalMoveAxis, 0, inputManager.verticalMoveAxis);
            transform.position = transform.position + (movementVector * Time.deltaTime * moveSpeed);
        }

        //�鴡��ⴴ
        if (JumpAction.IsPressed() == true)
        {
            this.GetComponent<Animator>().SetBool("IsJump", true);
            //print("Jump");
        }
        else
        {
            this.GetComponent<Animator>().SetBool("IsJump", false);
        }

        //�����
        if (RunAction.IsPressed() == true)
        {
            moveSpeed = RunSpeed;
        }
        else
        {
            moveSpeed = WalkSpeed;
        }

        //�������ǵ���Ф�
        velocity = (transform.position - Pos) / Time.deltaTime;
        this.GetComponent<Animator>().SetFloat("Speed", velocity.magnitude);
        //Debug.Log("Speed is : " + velocity.magnitude);
        Pos = transform.position;

        //����Ф��ѹ���¢��
        if (inputManager.horizontalMoveAxis < 0)
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else if (inputManager.horizontalMoveAxis > 0)
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        //����Ф��Թ���˹��ŧ��ѧ
        if (inputManager.verticalMoveAxis < 0)
            this.GetComponent<Animator>().SetBool("IsWalkForward", false);
        else if (inputManager.verticalMoveAxis > 0)
            this.GetComponent<Animator>().SetBool("IsWalkForward", true);
    }

    void Ui_Control()
    {
        //�Դ˹�Ҫ�ͧ�红ͧ
        if (InventoryAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Inventory_System>().Set_Inventory_Element();
            gameObject.GetComponent<Inventory_System>().Set_Item_Element();
            Essential_Menu.SetActive(true);
            Essential_Menu.GetComponent<Navigate_Menu>().OpenPage(0);
            Game_State_Manager.Instance.Setstate(GameState.Pause);
        }

        //�Դ˹�Һѹ�֡
        if (NoteAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Inventory_System>().Set_Inventory_Element();
            Essential_Menu.SetActive(true);
            Essential_Menu.GetComponent<Navigate_Menu>().OpenPage(2);
            Game_State_Manager.Instance.Setstate(GameState.Pause);
        }

        //�Դ˹�� craft
        if (CraftAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Inventory_System>().Set_Inventory_Element();
            Essential_Menu.SetActive(true);
            Essential_Menu.GetComponent<Navigate_Menu>().OpenPage(1);
            Game_State_Manager.Instance.Setstate(GameState.Pause);
        }

        //��������ԧ
        if (aimAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Inventory_System>().Aim(!gameObject.GetComponent<Inventory_System>().IsAim);
        }

        //��ҹ����
        if (useitemAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Inventory_System>().Use_Item_Equip();
        }

        //�����ԧ/��
        if (shootAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Inventory_System>().Shoot_Item();
        }
    }
}
