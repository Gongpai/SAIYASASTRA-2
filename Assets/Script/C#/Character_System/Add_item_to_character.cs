using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Add_item_to_character : MonoBehaviour
{
    [SerializeField] private int ItemIndex = 0;
    [SerializeField] public bool IsSpawn = false;
    [SerializeField] new Collider collider = new Collider();

    private Structs_Libraly.Item_Data itemData;

    private GameObject Gameinstance;
    // Start is called before the first frame update
    void Start()
    {
        Gameinstance = GameObject.FindGameObjectWithTag("GameInstance").gameObject;
        itemData = new Structs_Libraly.Item_Data
            (
                ItemIndex,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].Name,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].Number,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].itemSprite,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].IsEquip,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].Index,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].ItemPrefeb,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].useItemMode
            );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !IsSpawn)
        {
            other.gameObject.GetComponent<Inventory_System>().Add_Item_Element(itemData);
            Destroy(this.gameObject);
            print("Adddddd-----------------------");
        }

        if (IsSpawn)
        {
            if (other.isTrigger && other.tag == "Player" && gameObject.tag == "Ghost_Attack")
            {
                print("HP------------------------------------- " + other);
                other.GetComponent<Player_Movement>().HP_System(collider, -1);
            }

            if (other.isTrigger && other.tag == "Player" && gameObject.tag == "Ghost_Attack" && other.GetComponent<Add_item_to_character>() != null && other.GetComponent<Add_item_to_character>().IsSpawn && other.GetComponent<Rigidbody>().velocity.y <= 0)
            {
                other.GetComponent<Player_Movement>().HP_System(collider, 1);
            }
        }
    }
}
