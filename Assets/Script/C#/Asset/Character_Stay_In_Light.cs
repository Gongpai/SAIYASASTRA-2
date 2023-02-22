using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Stay_In_Light : MonoBehaviour
{
    public GameObject Ghost;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Ghost")
        {
            Ghost = other.gameObject;
            //print("SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ghost")
        {
            Ghost = null;
            print("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
        }

    }
}
