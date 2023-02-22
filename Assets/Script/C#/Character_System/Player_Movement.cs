using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Player_Movement : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float WalkSpeed = 1;

    [SerializeField] private float RunSpeed = 3;

    [SerializeField] private float JumpForce = 1;

    [SerializeField] private Vector3 BoxSize;

    [SerializeField] private float MaxDistance;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private MovementModes movementMode = MovementModes.FreeRoam;

    [SerializeField] private GameObject Playerinput;

    [SerializeField] public GameObject HeadPoint;

    [SerializeField] public GameObject EquipPoint;

    [SerializeField] public ShowMessage showMessage;

    [SerializeField] public TextMeshProUGUI TextDebug;

    [SerializeField] public GameObject Essential_Menu;

    [SerializeField] public GameObject Death_Ui;

    [SerializeField] private Image HP_ProgressBar;

    [Header("NotSet")]
    float moveSpeed = 0;
    public PlayerInput playerInput;
    private InputAction JumpAction, RunAction, InventoryAction, NoteAction, CraftAction, aimAction, useitemAction, shootAction;
    private InputManager inputManager;

    private Object_interact Ob_interact = Object_interact.Cupboard_Hide;

    private Vector3 velocity, Pos;

    public float HP = 100;

    public GameObject ObjectHide;
    private GameObject Object_Intaeract;
    private Rigidbody rb;

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
        UpdateHPWidget();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Character_movement();
        Ui_Control();
        EquipPoint_Turn();
        OnCharacterDeath();
        UpdateHPWidget();
    }

    void OnCharacterDeath()
    {
        if(HP <= 0)
        {
            GameObject.FindGameObjectWithTag("Camera_Setting").GetComponent<ZoomSmoothCameraSystem>().IsZoomCamera = true;
            Death_Ui.SetActive(true);
            Death_Ui.GetComponent<Animator>().SetBool("Is_Death", true);
        }
    }
    void EquipPoint_Turn()
    {
        if (GetComponent<SpriteRenderer>().flipX)
        {
            EquipPoint.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            EquipPoint.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger && other.tag == "Ghost_Attack")
        {
            HP_System(other, -1);
        }

        if (other.isTrigger && other.tag == "Attack_Item" && other.GetComponent<Add_item_to_character>() != null && other.GetComponent<Add_item_to_character>().IsSpawn && other.GetComponent<Rigidbody>().velocity.y <= 0)
        {
            HP_System(other, 1);
        }

        switch (other.tag)
        {
            case "Character_Hide":
                Ob_interact = Object_interact.Cupboard_Hide;
                Object_Intaeract = other.gameObject;
                break;
            case "Door_Lawson":
                Ob_interact = Object_interact.Lawson_Door;
                Object_Intaeract = other.gameObject;
                break;
            default:
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Character_Hide":
                Ob_interact = Object_interact.Cupboard_Hide;
                Object_Intaeract = null;
                break;
            case "Door_Lawson":
                Ob_interact = Object_interact.Lawson_Door;
                Object_Intaeract = null;
                break;
            default:
                break;
        }
    }

    private void HP_System(Collider other, float hp)
    {
        float Damage = other.GetComponent<Variables>().declarations.Get<float>("Damage");

        if (HP > 0)
        {
            HP += hp * Damage;

            if (HP <= 0)
                HP = 0;
        }
        print("Hp------- : " + HP);
        other.GetComponent<Item_Script>().Destroy_Item();
    }

    void UpdateHPWidget()
    {
        FuntionLibraly.ProgressBar_Fill(HP_ProgressBar, HP, 100);
    }

    void Character_movement()
    {
        //โค้ดส่วนของการเคลื่อนที่ตัวละคร
        if (movementMode == MovementModes.FreeRoam)
        {
            Vector3 movementVector = new Vector3(inputManager.horizontalMoveAxis, 0, inputManager.verticalMoveAxis);
            rb.transform.position = rb.transform.position + (movementVector * Time.deltaTime * moveSpeed);
        }

        //โค้ดกระโดด
        if (GroundCheck())
        {
            this.GetComponent<Animator>().SetBool("IsJump", false);
            //print("StopJump");
        }
        else
        {
            this.GetComponent<Animator>().SetBool("IsJump", true);
        }

        if (JumpAction.WasPressedThisFrame() && GroundCheck())
        {
            rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
            //print("Jump");
        }

        

        //โค้ดวิ่ง
        if (RunAction.IsPressed() == true)
        {
            moveSpeed = RunSpeed;
        }
        else
        {
            moveSpeed = WalkSpeed;
        }

        //ความเร็วตัวละคร
        velocity = (transform.position - Pos) / Time.deltaTime;
        this.GetComponent<Animator>().SetFloat("Speed", velocity.magnitude);
        //Debug.Log("Speed is : " + velocity.magnitude);
        Pos = transform.position;

        //ตัวละครหันซ้ายขวา
        if (inputManager.horizontalMoveAxis < 0)
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else if (inputManager.horizontalMoveAxis > 0)
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        //ตัวละครเดินขึ้นหน้าลงหลัง
        if (inputManager.verticalMoveAxis < 0)
            this.GetComponent<Animator>().SetBool("IsWalkForward", false);
        else if (inputManager.verticalMoveAxis > 0)
            this.GetComponent<Animator>().SetBool("IsWalkForward", true);
    }

    /**
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * MaxDistance, BoxSize);
    }
    **/

    private bool GroundCheck()
    {
        if (Physics.BoxCast(transform.position, BoxSize, -transform.up, transform.rotation, MaxDistance, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Ui_Control()
    {
        //เปิดหน้าช่องเก็บของ
        if (InventoryAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Inventory_System>().Set_Inventory_Element();
            gameObject.GetComponent<Inventory_System>().Set_Item_Element();
            Essential_Menu.SetActive(true);
            Essential_Menu.GetComponent<Navigate_Menu>().OpenPage(0);
            Game_State_Manager.Instance.Setstate(GameState.Pause);
        }

        //เปิดหน้าบันทึก
        if (NoteAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Note_System>().Set_Note_Element();
            Essential_Menu.SetActive(true);
            Essential_Menu.GetComponent<Navigate_Menu>().OpenPage(2);
            Game_State_Manager.Instance.Setstate(GameState.Pause);
        }

        //เปิดหน้า craft
        if (CraftAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Craft_System>().Set_Craft_Inventory_Element();
            Essential_Menu.SetActive(true);
            Essential_Menu.GetComponent<Navigate_Menu>().OpenPage(1);
            Game_State_Manager.Instance.Setstate(GameState.Pause);
        }

        //ปุ่มเล็งยิง
        if (aimAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Inventory_System>().Aim(!gameObject.GetComponent<Inventory_System>().IsAim);
            Flashing_Lights.event_Light_On_Off?.Invoke(Flashing_Lights.Light_Mode.Flashing);
        }

        //ใช้งานไอเทม
        if (useitemAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Inventory_System>().Use_Item_Equip();
            Flashing_Lights.event_Light_On_Off?.Invoke(Flashing_Lights.Light_Mode.Turn_Off);
        }

        //ปุ่มยิง/ปา
        if (shootAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Inventory_System>().Shoot_Item();
        }

        //เปิดประตู้และอื่นๆ
        if (useitemAction.WasPressedThisFrame() == true)
        {
            if (Object_Intaeract != null)
            {
                switch (Ob_interact)
                {
                    case Object_interact.Cupboard_Hide:
                        break;
                    case Object_interact.Lawson_Door:
                        Object_Intaeract.transform.parent.GetComponent<Door_Lawson_System>().OpenOrClose();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
