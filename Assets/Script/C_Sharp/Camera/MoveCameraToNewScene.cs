using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class MoveCameraToNewScene : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Collider pointNextCamera;

    [SerializeField] private bool IsBlock = false;
    [SerializeField] private bool MoveBeginPlay = false;
    [SerializeField] private bool is_Walk_in = false;
    [SerializeField] private float SetColliderPosition = 0.0f;
    [SerializeField] private GameObject MovePoint;
    [SerializeField] private GameObject CameraPointL;
    [SerializeField] private GameObject CameraPointR;
    private Vector3 ScreenCenter;
    private Vector3 ScreenLeft;
    private bool IsWalkLeft;

    //MoveSmooth
    private float Duration = 0;
    private float offSet = 1;
    private float speed = 1;
    private float timeupCam = 0;
    private bool IsMoveCamera = false;
    private bool IsMovetoCharacter = false;
    private bool IsSet_CamCanMove = false;
    public bool IsCharacterEnter = false;
    private bool IsMoveto_Door = false;
    public bool IsPlayerCantOpenDoor = false;
    private Vector3 start;
    private Vector3 end;
    private Transform endTransform;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        if (!is_Walk_in)
        {
            CameraPointR.transform.position = new Vector3(CameraPointR.transform.position.x + SetColliderPosition,CameraPointR.transform.position.y, CameraPointR.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MakeCameraPointLeft();

        if (Distance(transform.parent.transform.position) < 0)
        {
            IsWalkLeft = true;
        }
        else
        {
            IsWalkLeft = false;
        }

        if (IsSet_CamCanMove)
        {
            if (IsMoveCamera)
            {
                MoveSmoothCamera();
            }
        }

        if (MoveBeginPlay)
        {
            IsMoveto_Door = true;
            SetMoveStartEnd(GameInstance.Player.transform.position, CameraPointR.transform.position, CameraPointR.transform);
            MoveBeginPlay = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsBlock)
        {
            if (other.tag == "Player")
            {
                IsCharacterEnter = true;
                if (is_Walk_in)
                {

                    Walk_In(true);
                    print("INNNNNNNNNN11111");
                }
                else
                {
                    if (Distance(transform.parent.transform.position) < 0.5f)
                    {
                        Walk_Out(true, true);
                    }
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!IsBlock)
        {
            if (other.tag == "Player")
            {
                IsCharacterEnter = true;
                other.GetComponent<Player_Movement>().Set_Block_Use_item(true);

                if (!IsPlayerCantOpenDoor)
                {
                    if (transform.parent.GetComponent<Animator>().GetBool("IsDoorOpen?"))
                    {
                        GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message("[E] ª‘¥ª√–µŸ");
                    }
                    else
                    {
                        GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message("[E] ‡ª‘¥ª√–µŸ");
                    }
                }
                else
                {
                    GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message("ª√–µŸ≈ÁÕ§");
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsBlock)
        {
            if (other.tag == "Player")
            {
                IsCharacterEnter = false;
                if (is_Walk_in)
                {

                    Walk_In(false);
                    print("INNNNNNNNNN222222");
                }
                else
                {
                    if (Distance(transform.parent.transform.position) < 0)
                    {
                        Walk_Out(false, true);
                    }
                }

                other.GetComponent<Player_Movement>().Set_Block_Use_item(false);
                transform.parent.GetComponent<Door_Lawson_System>().OpenOrClose(false, true, true);
                GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Hide_Message();
            }
        }
    }

    private void MakeCameraPointLeft()
    {
        if ((GameInstance.Player.transform.position.x - (transform.parent.position.x - GameInstance.LeftDistanceCam)) >= -2 && !IsSet_CamCanMove)
        {
            IsSet_CamCanMove = true;
            Transform cameraPointL = CameraPointL.transform;
            ScreenLeft = new Vector3(transform.parent.position.x - GameInstance.LeftDistanceCam, transform.parent.position.y, transform.parent.position.z);
            cameraPointL.position = new Vector3(ScreenLeft.x - 0.05f, transform.position.y, ScreenLeft.z);

            Transform cameraPointR = CameraPointR.transform;
            ScreenCenter = new Vector3(transform.parent.position.x + GameInstance.rightDistanceCam, transform.parent.position.y, transform.parent.position.z);
            print("-------------1315-----------------");
            cameraPointR.position = new Vector3(ScreenCenter.x + 0.05f, transform.position.y, ScreenCenter.z);
        }
    }

    private float Distance(Vector3 Target)
    {
        float distance = GameInstance.Player.transform.position.x - Target.x;
        return distance;
    }

    public void Walk_In(bool Is_In, bool IsWalkin = false)
    {
        if (IsWalkin)
        {
            
            if (Distance(CameraPointL.transform.position) < 0)
            {
                if (Is_In)
                {
                    print("Innnnnn----------------------------");
                    SetMoveStartEnd(GameInstance.Player.transform.position, CameraPointL.transform.position, CameraPointL.transform);
                    IsMoveto_Door = true;
                }
                else
                {
                    print("Outttttt----------------------------");
                    SetMoveStartEnd(new Vector3(), GameInstance.Player.transform.position, GameInstance.Player.transform, true, true);
                }
            }
        }
    }

    public void Walk_Out(bool IsWalkIn, bool IsWalkOut = false)
    {
        if (IsWalkIn)
        {
            if (IsWalkOut)
            {
                SetMoveStartEnd(CameraPointL.transform.position, CameraPointR.transform.position, CameraPointR.transform);

            }
            else
            {
                GameObject Vcam = GameObject.FindGameObjectWithTag("Vcam");
                CinemachineTargetGroup.Target target = default;
                target.target = CameraPointR.transform;
                target.weight = 1;
                Vcam.GetComponent<CinemachineTargetGroup>().m_Targets[0] = target;
                print("121212131313-------------");
                SetMoveStartEnd(GameInstance.Player.transform.position, CameraPointR.transform.position, CameraPointR.transform);
                IsMoveto_Door = true;
            }
            
        }
        else
        {
            if (IsWalkOut)
            {
                if (Distance(CameraPointR.transform.position) < 0)
                {
                    print("BackTOPPPPPPPPPPPPPP----------------");
                    SetMoveStartEnd(CameraPointR.transform.position, CameraPointL.transform.position, CameraPointL.transform);
                }
            }
            else
            {
                if (Distance(CameraPointR.transform.position) >= 0)
                {
                    print("NextTOPPPPPPPPPPPPPP----------------");
                    SetMoveStartEnd(new Vector3(), GameInstance.Player.transform.position, GameInstance.Player.transform, true, true);
                }
            }
        }
    }

    private void FindLocationScreenZeroAndCenter()
    {
        /**
        if (!IsSet_Centor)
        {
            IsSet_Centor = true;
            Transform cameraPoint = transform.parent.transform.GetChild(0).transform;
            Vector3 screenpoint = new Vector3(Screen.width, (Screen.height / 2), 0);
            screenpoint.z = Camera.main.nearClipPlane + 12.0f;
            ScreenCenter = mainCamera.ScreenToWorldPoint(screenpoint);
            print((ScreenCenter) + " : " + (Screen.height / 2));
            cameraPoint.position = new Vector3(ScreenCenter.x + 0.05f, transform.position.y, ScreenCenter.z);
            SetMoveStartEnd(GameInstance.Player.transform.position, cameraPoint.transform.position, cameraPoint.transform);
        }
        else
        {
            SetMoveStartEnd(GameInstance.Player.transform.position, transform.parent.GetChild(0).position, transform.parent.GetChild(0));
        }
        **/
    }


    private void SetMoveStartEnd(Vector3 MoveStart = default, Vector3 MoveEnd = default, Transform endtran = default, bool IsStartFromCameraPoint = false, bool IsMoveChar = false)
    {
        Duration = 0;
        offSet = 1;
        if (IsMoveCamera || IsStartFromCameraPoint)
        {
            start = MovePoint.transform.position;
        }
        else
        {
            start = MoveStart;
        }

        IsMovetoCharacter = IsMoveChar;
        end = MoveEnd;
        endTransform = endtran;
        IsMoveCamera = true;
    }

    private void MoveSmoothCamera()
    {
        Duration += (offSet * Time.deltaTime) * speed;
        offSet -= (1 - Duration) * Time.deltaTime;

        if (Duration > 1)
        {
            Duration = 1;
            IsMoveCamera = false;
        }
            
        if (Duration < 1)
        {
            Transform cameraPoint = MovePoint.transform;

            GameObject Vcam = GameObject.FindGameObjectWithTag("Vcam");
            if (IsMovetoCharacter || IsMoveto_Door)
            {
                cameraPoint.transform.position = Vector3.Lerp(start, end, Duration);
            }
            else
            {
                cameraPoint.transform.position = Vector3.Lerp(start, end, math.smoothstep(0, 1, Duration));
            }

            CinemachineTargetGroup.Target target = default;
            target.target = cameraPoint;
            target.weight = 1;
            Vcam.GetComponent<CinemachineTargetGroup>().m_Targets[0] = target;
        }

        if (IsMovetoCharacter)
        {
            SetMoveStartEnd(new Vector3(), GameInstance.Player.transform.position, GameInstance.Player.transform, true, true);
            //print(MovePoint.transform.position.x + " : " + GameInstance.Player.transform.position.x);
            speed = speed + (Time.deltaTime * 8);

            timeupCam += Time.deltaTime;
            if ((MovePoint.transform.position.x - 0.01 <= GameInstance.Player.transform.position.x && GameInstance.Player.transform.position.x <= MovePoint.transform.position.x + 0.01) || timeupCam >= 3)
            {
                IsMovetoCharacter = false;
                print("EEEEEEEEEEEEEEE---------------");
                Duration = 1;
                speed = 1;
                timeupCam = 0;
            }
                
        }

        if (!IsMoveCamera)
        {
            GameObject Vcam = GameObject.FindGameObjectWithTag("Vcam");
            CinemachineTargetGroup.Target target = default;
            target.target = endTransform;
            target.weight = 1;
            Vcam.GetComponent<CinemachineTargetGroup>().m_Targets[0] = target;

            print("StopCammmmmmmmmmmmm");
            IsMovetoCharacter = false;
            IsMoveto_Door = false;
            Duration = 0;
            speed = 1;
            offSet = 1;
        }
    }

    
}
