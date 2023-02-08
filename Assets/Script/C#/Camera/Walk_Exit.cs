using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Walk_Exit : MonoBehaviour
{
    [SerializeField] private GameObject DoorCollider;
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
            DoorCollider.GetComponent<MoveCameraToNewScene>().Walk_Out(false, false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && (GameInstance.Player.transform.position.x - transform.position.x) > 0)
        {
            DoorCollider.GetComponent<MoveCameraToNewScene>().Walk_Out(true, false);
        }
    }
}
