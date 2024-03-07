using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn_Tutorial_System : MonoBehaviour
{
    [SerializeField] List<Sprite> Tutorial_PC = new List<Sprite>();
    [SerializeField] List<Sprite> Tutorial_Mobile = new List<Sprite>();
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
            Platform_Select();
        }
    }

    void Start()
    {
        if (IsUseColliderEnter)
        {
            IsSpawn = true;
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Tutorial_Mobile.Count > 1)
            {
                Prev_But.SetActive(true);
                Next_But.SetActive(true);
            }
        }
        else
        {
            if (Tutorial_PC.Count > 1)
            {
                Prev_But.SetActive(true);
                Next_But.SetActive(true);
            }
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

    private void Platform_Select(int i = 0)
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                OpenTutorial(Tutorial_PC, i);
                break;
            case RuntimePlatform.WindowsEditor:
                //OpenTutorial();

                if (Tutorial_Mobile != null)
                {
                    OpenTutorial(Tutorial_Mobile, i);
                }
                break;
            case RuntimePlatform.Android:
                if (Tutorial_Mobile != null)
                {
                    OpenTutorial(Tutorial_Mobile, i);
                }
                break;
            case RuntimePlatform.WebGLPlayer:
                OpenTutorial(Tutorial_PC, i);
                break;
        }
    }

    public void Tutorial_Select(int i = 1)
    {
        int count;

        if (Application.platform == RuntimePlatform.Android)
            count = Tutorial_Mobile.Count;
        else
            count = Tutorial_PC.Count;
        
        if (i > 0 && index <= count)
        {
            index++;

            if (index >= count)
                index = 0;
        }
        else if (i < 0 && index >= 0)
        {
            index--;

            if (index < 0)
                index = count - 1;
        }

        Platform_Select(index);
    }

    private void OpenTutorial(List<Sprite> sprite, int index)
    {
        Game_State_Manager.Instance.Setstate(GameState.Pause);
        Tutorial.sprite = sprite[index];
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
