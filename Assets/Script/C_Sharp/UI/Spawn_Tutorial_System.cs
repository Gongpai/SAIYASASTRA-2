using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn_Tutorial_System : MonoBehaviour
{
    [SerializeField] Sprite Tutorial_PC;
    [SerializeField] Sprite Tutorial_Mobile;
    [SerializeField] Image Tutorial;
    [SerializeField] bool IsUseColliderEnter = false;
    [SerializeField] GameObject Tutorial_Widget;
    [SerializeField] Animator animator;

    [SerializeField] GameObject Prev_But;
    [SerializeField] GameObject Next_But;

    [SerializeField] float WidgetSpawnDelay = 0.0f;

    bool IsWidgetOpen = false;
    bool IsSpawn = false;
    int index = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Input.touchSupported && Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Prev_But.SetActive(true);
            Next_But.SetActive(true);
        }
    }

    void Start()
    {
        if (IsUseColliderEnter)
        {
            IsSpawn = true;
        }

        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
                //OpenTutorial();

                if (Tutorial_Mobile == null)
                {
                    Destroy(gameObject);
                }
                break;
            case RuntimePlatform.Android:
                if (Tutorial_Mobile == null)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsSpawn)
        {
            WidgetSpawnDelay -= Time.deltaTime;
            print("Show Tutorial In : " + WidgetSpawnDelay);
            if (WidgetSpawnDelay <= 0.0f)
            {
                Platform_Select();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && IsUseColliderEnter)
        {
            Platform_Select();
        }
    }

    private void Platform_Select()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                OpenTutorial(Tutorial_PC);
                break;
            case RuntimePlatform.WindowsEditor:
                //OpenTutorial();

                if (Tutorial_Mobile != null)
                {
                    OpenTutorial(Tutorial_Mobile);
                }
                break;
            case RuntimePlatform.Android:
                if (Tutorial_Mobile != null)
                {
                    OpenTutorial(Tutorial_Mobile);
                }
                break;
            case RuntimePlatform.WebGLPlayer:
                OpenTutorial(Tutorial_PC);
                break;
        }
    }

    public void Tutorial_Select(int i = 1)
    {
        index += i;
        if (index > 1)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = 1;
        }

        if (index == 0)
        {
            OpenTutorial(Tutorial_PC);
        }
        else
        {
            OpenTutorial(Tutorial_Mobile);
        }
    }

    private void OpenTutorial(Sprite sprite)
    {
        Game_State_Manager.Instance.Setstate(GameState.Pause);
        Tutorial.sprite = sprite;
        Tutorial_Widget.SetActive(true);
        animator.SetBool("IsPlayIn", true);
        animator.SetBool("IsPlayOut", false);
        IsSpawn = true;
        print("Show Tutorial");
    }

    public void OnOpen()
    {
        IsWidgetOpen = true;
        print("On Open");
    }

    public void OnClose()
    {
        print("On Close");
        animator.SetBool("IsPlayIn", false);
        animator.SetBool("IsPlayOut", true);
    }

    public void OnDestoryWidget()
    {
        if (IsWidgetOpen)
        {
            Game_State_Manager.Instance.Setstate(GameState.Play);
            Destroy(gameObject);
        }
    }
}
