using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Eqip_Item_System : MonoBehaviour
{
    [SerializeField] private GameObject FG;

    public Structs_Libraly.Item_Data itemData;

    public int IndexInventory = 0;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FG.SetActive(GameInstance.inventoryData[IndexInventory].IsEquip);
    }

    public void Eqip_Item()
    {
        if (!GameInstance.inventoryData[IndexInventory].IsEquip)
        {
            itemData.Index = IndexInventory;
            GameInstance.Player.gameObject.GetComponent<Inventory_System>().Add_Item_Equip(itemData, this.gameObject);
            GameInstance.inventoryData[IndexInventory] = new Structs_Libraly.Item_Data
                (
                    GameInstance.inventoryData[IndexInventory].Item_Index,
                    GameInstance.inventoryData[IndexInventory].Name, 
                    GameInstance.inventoryData[IndexInventory].Number, 
                    GameInstance.inventoryData[IndexInventory].itemSprite, 
                    true, 
                    GameInstance.inventoryData[IndexInventory].Index,
                    GameInstance.inventoryData[IndexInventory].ItemPrefeb,
                    GameInstance.inventoryData[IndexInventory].useItemMode
                );
        }
        else
        {
            GameInstance.Player.gameObject.GetComponent<Inventory_System>().Remove_Equip_Item(GameInstance.inventoryData[IndexInventory].Index);
            GameInstance.inventoryData[IndexInventory] = new Structs_Libraly.Item_Data
                (
                    GameInstance.inventoryData[IndexInventory].Item_Index,
                    GameInstance.inventoryData[IndexInventory].Name, 
                    GameInstance.inventoryData[IndexInventory].Number, 
                    GameInstance.inventoryData[IndexInventory].itemSprite, 
                    false, 
                    GameInstance.inventoryData[IndexInventory].Index,
                    GameInstance.inventoryData[IndexInventory].ItemPrefeb,
                    GameInstance.inventoryData[IndexInventory].useItemMode
                );
           
        }
        
        print("ISSSSS ---- : " + GameInstance.inventoryData[IndexInventory].Index + " Name : " + itemData.Name);
    }

    public void ReEquip_Item()
    {
        itemData.Index = IndexInventory;
        print(itemData + " : " + itemData.Name);

        GameInstance.Player.gameObject.GetComponent<Inventory_System>().Add_Item_Equip(itemData, this.gameObject);
        GameInstance.inventoryData[IndexInventory] = new Structs_Libraly.Item_Data
        (
            GameInstance.inventoryData[IndexInventory].Item_Index,
            GameInstance.inventoryData[IndexInventory].Name,
            GameInstance.inventoryData[IndexInventory].Number,
            GameInstance.inventoryData[IndexInventory].itemSprite,
            true,
            GameInstance.inventoryData[IndexInventory].Index,
            GameInstance.inventoryData[IndexInventory].ItemPrefeb,
            GameInstance.inventoryData[IndexInventory].useItemMode
        );
    }

    public void Show_Info()
    {
        GameInstance.Player.GetComponent<Inventory_System>().Set_Inventory_Info(itemData);
    }
}
