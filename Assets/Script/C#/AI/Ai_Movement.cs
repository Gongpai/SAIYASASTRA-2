using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Ai_Movement : MonoBehaviour
{
    private InputManager inputManager;
    private bool IsSeeCharacter;
    private GameObject Player;
    private Vector3 velocity, Pos;
    private Vector3[] distanceVector3 = new Vector3[2];

    void Awake()
    {
        Pos = transform.position;
    }
    void OnEnable()
    {
        GetComponent<Animator>().enabled = true;
    }

    void OnDisable()
    {
        GetComponent<Animator>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        distanceVector3[0] = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Ai_movement();
        
        distanceVector3[0] = gameObject.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsSeeCharacter = true;
            Player = other.gameObject;
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            IsSeeCharacter = false;
    }

    void Ai_movement()
    {//�������ǵ���Ф�
        velocity = (transform.position - Pos) / Time.deltaTime;
        this.GetComponent<Animator>().SetFloat("Speed", velocity.magnitude);
        //Debug.Log("Speed is : " + velocity.magnitude);
        Pos = transform.position;

        //����Ф��ѹ���¢��
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

        //����Ф��Թ���˹��ŧ��ѧ
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
}
