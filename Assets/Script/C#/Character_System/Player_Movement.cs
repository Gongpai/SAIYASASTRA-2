using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float WalkSpeed = 1;

    [SerializeField] private float RunSpeed = 3;

    [SerializeField] private MovementModes movementMode = MovementModes.FreeRoam;

    [SerializeField] private  GameObject Playerinput;

    [SerializeField] public ShowMessage showMessage;

    [SerializeField] public TextMeshProUGUI TextDebug;

    float moveSpeed = 0;
    private PlayerInput playerInput;
    private InputAction JumpAction, RunAction;
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
        GetComponent<Animator>().enabled = true;
    }

    void OnDisable()
    {
        JumpAction.Disable();
        RunAction.Disable();
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
    }

    void Character_movement()
    {
        //โค้ดส่วนของการเคลื่อนที่ตัวละคร
        if (movementMode == MovementModes.FreeRoam)
        {
            Vector3 movementVector = new Vector3(inputManager.horizontalMoveAxis, 0, inputManager.verticalMoveAxis);
            transform.position = transform.position + (movementVector * Time.deltaTime * moveSpeed);
        }

        //โค้ดกระโดด
        if (JumpAction.IsPressed() == true)
        {
            this.GetComponent<Animator>().SetBool("IsJump", true);
            //print("Jump");
        }
        else
        {
            this.GetComponent<Animator>().SetBool("IsJump", false);
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
}
