using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Image = UnityEngine.UI.Image;

public class Player_Movement : FuntionLibraly
{
    [Header("Setting")]
    [SerializeField] private bool Is_Spawn_In_Room = false;

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

    [SerializeField] public GameObject PauseGame_Ui;

    [SerializeField] public GameObject Death_Ui;

    [SerializeField] public GameObject Touch_screen_UI;

    [SerializeField] public GameObject Switch_Scene;

    [SerializeField] private Image HP_ProgressBar;

    [Header("NotSet")]
    float moveSpeed = 0;
    public PlayerInput playerInput;
    private InputAction JumpAction, RunAction, InventoryAction, NoteAction, CraftAction, aimAction, useitemAction, shootAction, PauseMenuAction;
    private InputManager inputManager;

    private Object_interact Ob_interact = Object_interact.Cupboard_Hide;

    private Vector3 velocity, Pos;

    public float HP = 100;

    public GameObject ObjectHide;
    private GameObject Object_Intaeract;
    private Rigidbody rb;
    private bool Is_Run = false;

    private List<bool> Can_Press_Use_item = new List<bool>();

    private enum MovementModes
    {
        MoveHorizontally,
        MoveVertically,
        FreeRoam
    };

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

        moveSpeed = WalkSpeed;
        GameInstance.Player = gameObject;
        UpdateHPWidget();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        Character_movement();
        Ui_Control();
        EquipPoint_Turn();
        OnCharacterDeath();
        UpdateHPWidget();
    }

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
        PauseMenuAction = playerInput.actions["PauseMenu"];
        Game_State_Manager.Instance.OnGameStateChange += OnGamestateChanged;
    }

    void OnDestroy()
    {
        Game_State_Manager.Instance.OnGameStateChange -= OnGamestateChanged;
    }

    void OnEnable()
    {
        Set_Platform();
        JumpAction.Enable();
        RunAction.Enable();
        InventoryAction.Enable();
        NoteAction.Enable();
        CraftAction.Enable();
        aimAction.Enable();
        useitemAction.Enable();
        shootAction.Enable();
        PauseMenuAction.Enable();
        OnGamePause(false);
        Set_Platform();
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
        OnGamePause(true);
        Touch_screen_UI.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        PauseMenuAction.Disable();
    }

    public void Set_Platform()
    {
        switch (SlateModeDetect.currentMode)
        {
            case ConvertibleMode.SlateTabletMode:
                Touch_screen_UI.SetActive(true);
                //Debug.LogWarning("SlateTabletMode");
                break;
            case ConvertibleMode.LaptopDockedMode:
                //Debug.LogWarning("LaptopDockedMode");
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsPlayer:
                        //print("Windowssssssssssssssssss");
                        Touch_screen_UI.SetActive(false);
                        break;
                    case RuntimePlatform.WindowsEditor:
                        //print("Windowssssssssssssssssss");
                        Touch_screen_UI.SetActive(true);
                        break;
                    case RuntimePlatform.Android:
                        //print("Androiddddddddddddddddddddd");
                        Touch_screen_UI.SetActive(true);
                        break;
                    case RuntimePlatform.WebGLPlayer:
                        Touch_screen_UI.SetActive(false);
                        break;
                }
                break;
        }
    }

    private void OnGamePause(bool isPause)
    {
        if (isPause)
        {
            GamePause_Component(gameObject, true);
            Item_Attack_System.OnPauseGame?.Invoke();
        }
        else
        {
            GamePause_Component(gameObject, false);
            Item_Attack_System.UnPauseGame?.Invoke();
        }
    }

    private void OnGamestateChanged(GameState gameState)
    {
        enabled = gameState == GameState.Play;
    }

    public void OnSwitchScene()
    {
        Switch_Scene.SetActive(true);
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

    public void Set_Block_Use_item(bool Is_Block)
    {
        if (Is_Block)
        {
            Can_Press_Use_item.Add(true);
        }
        else
        {
            if (Can_Press_Use_item.Count > 0)
                Can_Press_Use_item.RemoveAt(0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
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
            case "PickUpItem":
                Ob_interact = Object_interact.PickUp_Item;
                Object_Intaeract = other.gameObject;
                break;
            case "PickUpNote":
                Ob_interact = Object_interact.PickUp_Note;
                Object_Intaeract = other.gameObject;
                break;
            case "Untagged":
                if(other.GetComponent<Show_Puzzle>() != null)
                {
                    Ob_interact = Object_interact.Puzzle;
                    Object_Intaeract = other.gameObject;
                }
                if(other.GetComponent<Open_Room_New_Scene>() != null)
                {
                    Ob_interact = Object_interact.Room_Door;
                    Object_Intaeract = other.gameObject;
                }
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
            case "PickUpItem":
                Ob_interact = Object_interact.PickUp_Item;
                Object_Intaeract = null;
                break;
            case "PickUpNote":
                Ob_interact = Object_interact.PickUp_Note;
                Object_Intaeract = null;
                break;
            case "Untagged":
                if (other.GetComponent<Show_Puzzle>() != null)
                {
                    Ob_interact = Object_interact.Puzzle;
                    Object_Intaeract = other.gameObject;
                }
                if (other.GetComponent<Open_Room_New_Scene>() != null)
                {
                    Ob_interact = Object_interact.Room_Door;
                    Object_Intaeract = other.gameObject;
                }
                break;
            default:
                break;
        }
    }

    public void HP_System(Collider other, float hp)
    {
        float Damage = other.GetComponent<Variables>().declarations.Get<float>("Damage");

        if (HP > 0)
        {
            HP += hp * Damage;

            if (HP <= 0)
                HP = 0;
            if (HP >= 100)
                HP = 100;
        }
        print("Hp------- : " + HP);
        other.GetComponent<Item_Script>().Destroy_Item();
    }

    void UpdateHPWidget()
    {
        FuntionLibraly.ProgressBar_Fill(HP_ProgressBar, HP, 100);
    }

    [System.Obsolete]
    void Character_movement()
    {
        //โค้ดส่วนของการเคลื่อนที่ตัวละคร
        if (movementMode == MovementModes.FreeRoam)
        {
            Vector3 movementVector = new Vector3(inputManager.horizontalMoveAxis, 0, inputManager.verticalMoveAxis);
            rb.transform.position = rb.transform.position + (movementVector * Time.deltaTime * moveSpeed);
        }
        if (Touch_screen_UI.active)
        {
            FloatingJoystick floatingJoystick = Touch_screen_UI.transform.GetChild(0).GetComponent<FloatingJoystick>();
            Vector3 movementVector = new Vector3(floatingJoystick.Horizontal, 0, floatingJoystick.Vertical);
            rb.transform.position = rb.transform.position + (movementVector * Time.deltaTime * moveSpeed);
            if (floatingJoystick.Horizontal < 0)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            else if (floatingJoystick.Horizontal > 0)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
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
        else if (RunAction.WasReleasedThisFrame())
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


    //ปุ่มสัมผัส - กดวิ่ง
    public void Touch_Run_Button()
    {
        if (!Is_Run)
        {
            print("Runnnnnnnnnnnnnnnnnnnnnnnn");
            moveSpeed = RunSpeed;
            Is_Run = true;
        }
        else
        {
            print("Walkkkkkkkkkkkkkkkkkkkkkkk");
            moveSpeed = WalkSpeed;
            Is_Run = false;
        }
    }

    //ปุ่มสัมผัส - กดกระโดด
    public void Touch_Jump_Button()
    {
        if (GroundCheck())
        {
            rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
            print("Jump");
        }
    }

    //ปุ่มสัมผัส - กดยิง
    public void Touch_Shoot_Button()
    {
        gameObject.GetComponent<Inventory_System>().Shoot_Item();
        gameObject.GetComponent<Inventory_System>().Use_Item_Equip();
    }

    //ปุ่มสัมผัส - กดตอบสนองของภายในฉาก
    public void Touch_Interact_Button()
    {
        if (Object_Intaeract != null)
        {
            switch (Ob_interact)
            {
                case Object_interact.Cupboard_Hide:
                    Object_Intaeract.GetComponent<Cupboard_Hide>().TriggerOpenDoor();
                    break;
                case Object_interact.Lawson_Door:
                    Object_Intaeract.transform.parent.GetComponent<Door_Lawson_System>().OpenOrClose();
                    break;
                case Object_interact.PickUp_Item:
                    Object_Intaeract.GetComponent<Pick_up_Item_System>().PickUp_Item();
                    break;
                case Object_interact.PickUp_Note:
                    Object_Intaeract.GetComponent<PickUp_Note_System>().PickUp_Note();
                    break;
                case Object_interact.Puzzle:
                    Object_Intaeract.GetComponent<Show_Puzzle>().OpenPuzzle_Ui();
                    break;
                case Object_interact.Room_Door:
                    Object_Intaeract.GetComponent<Open_Room_New_Scene>().Enter_Door();
                    break;
                default:
                    break;
            }
        }
    }

    //ปุ่มสัมผัส - กดเปิดช่องเก็บของ
    public void Touch_OpenInventoryUi()
    {
        gameObject.GetComponent<Inventory_System>().Set_Inventory_Element();
        gameObject.GetComponent<Inventory_System>().Set_Item_Element();
        Essential_Menu.SetActive(true);
        Essential_Menu.GetComponent<Navigate_Menu>().OpenPage(0);
        Game_State_Manager.Instance.Setstate(GameState.Pause);
    }

    [System.Obsolete]
    private bool Check_PauseMenu()
    {
        bool check = !PauseGame_Ui.active && !Death_Ui.active;
        return check;
    }

    //ปุ่มสัมผัส - กดหยุดเกม
    public void Touch_OpenPauseMenu()
    {
        if (Check_PauseMenu())
        {
            PauseGame_Ui.SetActive(true);
            Game_State_Manager.Instance.Setstate(GameState.Pause);
        }
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

    [System.Obsolete]
    void Ui_Control()
    {
        //เปิดหน้า PauseMenu
        if(PauseMenuAction.WasPerformedThisFrame())
        {
            if(!PauseGame_Ui.active && !Death_Ui.active)
            {
                PauseGame_Ui.SetActive(true);
                Game_State_Manager.Instance.Setstate(GameState.Pause);
            }
        }

        //เปิดหน้าช่องเก็บของ
        if (InventoryAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Inventory_System>().Set_Inventory_Element();
            gameObject.GetComponent<Inventory_System>().Set_Item_Element();
            Essential_Menu.SetActive(true);
            Essential_Menu.GetComponent<Navigate_Menu>().OpenPage(0);
            GetComponent<Inventory_System>().PlayAnim(true);
            Game_State_Manager.Instance.Setstate(GameState.Pause);
        }

        //เปิดหน้าบันทึก
        if (NoteAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Note_System>().Set_Note_Element();
            Essential_Menu.SetActive(true);
            Essential_Menu.GetComponent<Navigate_Menu>().OpenPage(2);
            GetComponent<Note_System>().PlayAnim(true);
            Game_State_Manager.Instance.Setstate(GameState.Pause);
        }

        //เปิดหน้า craft
        if (CraftAction.WasPressedThisFrame() == true)
        {
            gameObject.GetComponent<Craft_System>().Set_Craft_Inventory_Element();
            Essential_Menu.SetActive(true);
            Essential_Menu.GetComponent<Navigate_Menu>().OpenPage(1);
            GetComponent<Craft_System>().PlayAnim(true);
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
            if (Can_Press_Use_item.Count <= 0)
            {
                gameObject.GetComponent<Inventory_System>().Use_Item_Equip();
            }
            
            Flashing_Lights.event_Light_On_Off?.Invoke(Flashing_Lights.Light_Mode.Turn_Off);
        }

        //ปุ่มยิง/ปา
        if (shootAction.WasPressedThisFrame() == true)
        {
            if (!Touch_screen_UI.active)
            {
                gameObject.GetComponent<Inventory_System>().Shoot_Item();
                gameObject.GetComponent<Inventory_System>().Use_Item_Equip();
            }
        }

        //เปิดประตู้และอื่นๆ
        if (useitemAction.WasPressedThisFrame() == true)
        {
            if (Object_Intaeract != null)
            {
                switch (Ob_interact)
                {
                    case Object_interact.Cupboard_Hide:
                        Object_Intaeract.GetComponent<Cupboard_Hide>().TriggerOpenDoor();
                        break;
                    case Object_interact.Lawson_Door:
                        Object_Intaeract.transform.parent.GetComponent<Door_Lawson_System>().OpenOrClose();
                        Object_Intaeract.transform.parent.GetComponent<Door_Lawson_System>().Cant_Open_Door();
                        break;
                    case Object_interact.PickUp_Item:
                        Object_Intaeract.GetComponent<Pick_up_Item_System>().PickUp_Item();
                        break;
                    case Object_interact.PickUp_Note:
                        Object_Intaeract.GetComponent<PickUp_Note_System>().PickUp_Note();
                        break;
                    case Object_interact.Puzzle:
                        Object_Intaeract.GetComponent<Show_Puzzle>().OpenPuzzle_Ui();
                        break;
                    case Object_interact.Room_Door:
                        Object_Intaeract.GetComponent<Open_Room_New_Scene>().Enter_Door();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
