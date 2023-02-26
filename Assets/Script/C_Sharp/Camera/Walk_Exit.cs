using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Walk_Exit : MonoBehaviour
{
    [SerializeField] private GameObject DoorCollider;
    [SerializeField] private bool IsWalkOut = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (IsWalkOut)
            {
                DoorCollider.GetComponent<MoveCameraToNewScene>().Walk_Out(false, false);
            }
            else
            {
                DoorCollider.GetComponent<MoveCameraToNewScene>().Walk_In(false, true);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && (GameInstance.Player.transform.position.x - transform.position.x) > 0)
        {
            if (IsWalkOut)
            {
                DoorCollider.GetComponent<MoveCameraToNewScene>().Walk_Out(true, false);
            }
            else
            {

            }
        }

        if (other.tag == "Player")
        {
            if (!IsWalkOut)
            {
                DoorCollider.GetComponent<MoveCameraToNewScene>().Walk_In(true, true);
            }
        }
    }
}
