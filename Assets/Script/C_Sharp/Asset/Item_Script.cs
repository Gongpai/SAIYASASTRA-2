using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Script : MonoBehaviour
{
    public void Destroy_Item()
    {
        //Playanim
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(gameObject.GetComponent<Item_Attack_System>() !=  null && !other.isTrigger && other.tag == "Untagged")
            Destroy_Item();
    }
}
