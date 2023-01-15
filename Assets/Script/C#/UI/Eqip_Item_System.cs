using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Eqip_Item_System : MonoBehaviour
{
    public Structs_Libraly.Item_Data itemData;
    
    public int IndexInventory = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Eqip_Item()
    {
        if (!GameInstance.inventoryData[IndexInventory].IsEquip)
        {
            itemData.Index = IndexInventory;
            GameInstance.Player.gameObject.GetComponent<Inventory_System>().Add_Item_Equip(itemData, this.gameObject);
            GameInstance.inventoryData[IndexInventory] = new Structs_Libraly.Item_Data
                (
                    GameInstance.inventoryData[IndexInventory].Name, 
                    GameInstance.inventoryData[IndexInventory].Number, 
                    GameInstance.inventoryData[IndexInventory].itemSprite, 
                    true, 
                    GameInstance.inventoryData[IndexInventory].Index,
                    GameInstance.inventoryData[IndexInventory].Owner
                );
        }
        else
        {
            GameInstance.Player.gameObject.GetComponent<Inventory_System>().Remove_Equip_Item(GameInstance.inventoryData[IndexInventory].Index);
            GameInstance.inventoryData[IndexInventory] = new Structs_Libraly.Item_Data
                (
                    GameInstance.inventoryData[IndexInventory].Name, 
                    GameInstance.inventoryData[IndexInventory].Number, 
                    GameInstance.inventoryData[IndexInventory].itemSprite, 
                    false, 
                    GameInstance.inventoryData[IndexInventory].Index,
                    GameInstance.inventoryData[IndexInventory].Owner
                );
           
        }
        
        print("ISSSSS ---- : " + GameInstance.inventoryData[IndexInventory].Index + " Name : " + itemData.Name);
    }
}
