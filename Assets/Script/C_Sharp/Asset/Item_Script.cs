using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Script : MonoBehaviour
{
    public void Destroy_Item()
    {
        //Playanim
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(gameObject.GetComponent<Add_item_to_character>().IsSpawn && !other.isTrigger && other.tag == "Untagged")
            Destroy_Item();
    }
}
