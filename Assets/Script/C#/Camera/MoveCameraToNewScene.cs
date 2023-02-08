using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class MoveCameraToNewScene : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Collider pointNextCamera;

    [SerializeField] private bool is_Walk_in = false;
    [SerializeField] private float SetColliderPosition = 0.0f;
    [SerializeField] private GameObject MovePoint;
    private Vector3 ScreenCenter;
    private bool IsWalkLeft;
    private float Duration = 0;
    private float offSet = 1;
    private float speed = 1;
    private bool IsMoveCamera = false;
    private bool IsMovetoCharacter = false;
    private bool IsSet_Centor = false;
    public bool IsCharacterEnter = false;
    private Vector3 start;
    private Vector3 end;
    private Transform endTransform;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        if (!is_Walk_in)
        {
            transform.parent.transform.GetChild(0).position = new Vector3(
                transform.parent.transform.GetChild(0).position.x + SetColliderPosition,
                transform.parent.transform.GetChild(0).position.y, transform.parent.transform.GetChild(0).position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Distance(transform.parent.transform.position) < 0)
        {
            IsWalkLeft = true;
        }
        else
        {
            IsWalkLeft = false;
        }

        if (IsSet_Centor)
        {
            if (IsMoveCamera)
            {
                MoveSmoothCamera();
            }
            else
            {
                GameObject Vcam = GameObject.FindGameObjectWithTag("Vcam");
                CinemachineTargetGroup.Target target = default;
                target.target = endTransform;
                target.weight = 1;
                Vcam.GetComponent<CinemachineTargetGroup>().m_Targets[0] = target;

                IsMovetoCharacter = false;
                Duration = 0;
                offSet = 1;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
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

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
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

            transform.parent.GetComponent<Door_Lawson_System>().OpenOrClose(false, true, true);
        }
    }

    private float Distance(Vector3 Target)
    {
        float distance = GameInstance.Player.transform.position.x - Target.x;
        return distance;
    }

    private void Walk_In(bool Is_In)
    {
        if (Is_In)
        {
            GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message("Open");
        }
        else
        {
            GameInstance.Player.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Hide_Message();
        }
        
    }

    public void Walk_Out(bool IsWalkIn, bool IsWalkOut = false)
    {
        if (IsWalkIn)
        {
            if (IsWalkOut)
            {
                FindLocationScreenZeroAndCentor();
            }
            else
            {
                GameObject Vcam = GameObject.FindGameObjectWithTag("Vcam");
                CinemachineTargetGroup.Target target = default;
                target.target = transform.parent.GetChild(0);
                target.weight = 1;
                Vcam.GetComponent<CinemachineTargetGroup>().m_Targets[0] = target;
                SetMoveStartEnd(GameInstance.Player.transform.position, transform.parent.GetChild(0).position, transform.parent.GetChild(0));
            }
            
        }
        else
        {
            if (IsWalkOut)
            {
                if (Distance(transform.parent.transform.GetChild(0).position) < 0)
                {
                    print("fyfyffyfy-------------------------");
                    SetMoveStartEnd(new Vector3(), GameInstance.Player.transform.position, GameInstance.Player.transform, true, true);
                }
            }
            else
            {
                if (Distance(transform.parent.transform.GetChild(0).position) >= 0)
                {
                    print("fyfyffyfy-------------------------");
                    SetMoveStartEnd(new Vector3(), GameInstance.Player.transform.position, GameInstance.Player.transform, true, true);
                }
            }
        }
    }

    private void FindLocationScreenZeroAndCentor()
    {

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
        Duration += (Time.deltaTime * offSet) * speed;
        offSet -= Time.deltaTime * (1 - Duration);
        //print(offSet + " : " + Duration);
        if (Duration >= 1)
        {
            Duration = 1;
            IsMoveCamera = false;
        }
            
        if (Duration <= 1)
        {
            Transform cameraPoint = MovePoint.transform;

            GameObject Vcam = GameObject.FindGameObjectWithTag("Vcam");
            cameraPoint.transform.position = Vector3.Lerp(start, end, Duration);
            CinemachineTargetGroup.Target target = default;
            target.target = cameraPoint;
            target.weight = 1;
            Vcam.GetComponent<CinemachineTargetGroup>().m_Targets[0] = target;
        }

        if (IsMovetoCharacter)
        {
            SetMoveStartEnd(new Vector3(), GameInstance.Player.transform.position, GameInstance.Player.transform, true, true);
            print(MovePoint.transform.position.x + " : " + GameInstance.Player.transform.position.x);
            speed = speed + (Time.deltaTime * 4);
            if (MovePoint.transform.position.x - 0.01 <= GameInstance.Player.transform.position.x && GameInstance.Player.transform.position.x <= MovePoint.transform.position.x + 0.01)
            {
                IsMovetoCharacter = false;
                print("EEEEEEEEEEEEEEE---------------");
                Duration = 1;
                speed = 1;
            }
                
        }
    }
}
