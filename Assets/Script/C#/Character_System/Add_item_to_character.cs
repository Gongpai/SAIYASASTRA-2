using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Add_item_to_character : MonoBehaviour
{
    [SerializeField] private int ItemIndex = 0;
    [SerializeField] public bool IsSpawn = false;

    private Structs_Libraly.Item_Data itemData;

    private GameObject Gameinstance;
    // Start is called before the first frame update
    void Start()
    {
        Gameinstance = GameObject.FindGameObjectWithTag("GameInstance").gameObject;
        itemData = Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !IsSpawn)
        {
            other.gameObject.GetComponent<Inventory_System>().Add_Item_Element(itemData);
            Destroy(this.gameObject);
        }
    }
}
