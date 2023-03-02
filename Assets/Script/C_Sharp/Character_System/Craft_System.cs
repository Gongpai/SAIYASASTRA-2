using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Structs_Libraly;

public class Craft_System : MonoBehaviour
{
    [SerializeField] private GameObject Craft;
    [SerializeField] private GameObject Craft_Element;
    [SerializeField] private TextMeshProUGUI Craft_Debug;
    [SerializeField] public GameObject Overlay;

    private List<GameObject> inventory_Element_list = new List<GameObject>();

    private List<string> Code = new List<string>();

    private string Item_Code;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 9;)
        {
            Code.Add("0");
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void On_Craft_Disable()
    {
        print("OnDisabla---------------------------------");

        if (Overlay.transform.childCount > 0)
        {
            foreach (Transform Child in Overlay.transform)
            {
                GameObject.Destroy(Child.gameObject);
            }
        }

        for (int i = 0; i < 9;)
        {
            Code[i] = "0";
            i++;
        }
    }

    public void Craft_Item()
    {
        Item_Data itemData;

        Set_Item_Code();
        print(Item_Code + " : Craftttttttttttttttttttttttt-------------------");

        if (Decode_Item(Item_Code).Item2)
        {
            itemData = Decode_Item(Item_Code).Item1;

            for (int i = (inventory_Element_list.Count - 1); i >= 0;)
            {
                GameObject inventory_element = inventory_Element_list[i];
                print("Index : " + i);

                GameInstance.inventoryData[i] = new Item_Data
                (
                    GameInstance.inventoryData[i].Item_Index,
                    GameInstance.inventoryData[i].Name,
                    GameInstance.inventoryData[i].Number - (GameInstance.inventoryData[i].Number - inventory_element.GetComponent<Craft_List>().itemData.Number),
                    GameInstance.inventoryData[i].itemSprite,
                    GameInstance.inventoryData[i].IsEquip,
                    GameInstance.inventoryData[i].Index,
                    GameInstance.inventoryData[i].ItemPrefeb,
                    GameInstance.inventoryData[i].useItemMode
                );
                //print(GameInstance.inventoryData[i].Name + " Number : " + GameInstance.inventoryData[i].Number);
                gameObject.GetComponent<Inventory_System>().RemoveItemFromInventory(inventory_element.GetComponent<Craft_List>().itemData);
                //print(GameInstance.inventoryData[i].Name + " Total_Number : " + GameInstance.inventoryData[i].Number);
                i--;
            }

            gameObject.GetComponent<Inventory_System>().Add_Item_Element(itemData);
            On_Craft_Disable();
            Set_Craft_Inventory_Element();
            print("Craft!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }

    public void Set_Craft_Inventory_Element()
    {
        int i = 0;
        GameObject List_Grid_Item = List_Grid_Item = Craft.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;

        if (inventory_Element_list.Count > 0)
        {
            foreach (GameObject list in inventory_Element_list)
            {
                Destroy(list.gameObject);
            }
        }

        inventory_Element_list.Clear();

        foreach (Structs_Libraly.Item_Data item in GameInstance.inventoryData)
        {
            if (item.useItemMode != Use_Item_System.Puzzle)
            {
                GameObject item_Inventory = Craft_Element;
                GameObject item_Inventory_list;

                item_Inventory.GetComponent<Craft_List>().itemData = new Structs_Libraly.Item_Data
                    (
                        item.Item_Index,
                        item.Name,
                        item.Number,
                        item.itemSprite,
                        item.IsEquip,
                        i,
                        item.ItemPrefeb,
                        item.useItemMode
                    );

                item_Inventory.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(item.Name);
                item_Inventory.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>()
                    .SetText(item.Number.ToString());
                item_Inventory.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = item.itemSprite;

                item_Inventory_list = Instantiate(item_Inventory, List_Grid_Item.transform);

                inventory_Element_list.Add(item_Inventory_list);

                i++;
            }
        }
    }

    public void Update_Item_Code(string code, int number)
    {
        print("num : " + number + " code : " + code);
        Code[number] = code;

        Set_Item_Code();
    }

    public Tuple<Item_Data, bool> Decode_Item(string code)
    {
        switch (code)
        {
            case "000010000":
                print("§√“ø¢«¥πÈ”¡πµÏ---------------------------");
                return Tuple.Create(Get_item(0), true);
            case "000131000":
                print("§√“øÀ¡ÈÕº’---------------------------");
                return Tuple.Create(Get_item(1), true);
            case "000212000":
                print("§√“ø¬—πµÏ---------------------------");
                return Tuple.Create(Get_item(2), true);
            case "010101010":
                print("§√“ø¢«¥πÈ”¡πµÏ2--------------------------");
                return Tuple.Create(Get_item(0), true);
        }

        return Tuple.Create(Get_item(0), false);
    }

    private Item_Data Get_item(int ItemIndex)
    {
        GameObject Gameinstance = GameObject.FindGameObjectWithTag("GameInstance").gameObject;
        Item_Data data = new Item_Data(
            ItemIndex,
            Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].Name,
            Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].Number,
            Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].itemSprite,
            Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].IsEquip,
            Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].Index,
            Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].ItemPrefeb,
            Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].useItemMode
        );

        return data;
    }

    private void Set_Item_Code()
    {
        string textcode = null;
        foreach (string text in Code)
        {
            textcode += text;
        }

        Craft_Debug.text = textcode;
        Item_Code = textcode;
    }

    public void Remove_Item_Code(int index)
    {
        Code[index] = "0";

        Set_Item_Code();
    }

    public void Set_Item_Craft_Inventory(int add)
    {
        int i = 0;
        List<int> Number = new List<int>();
        foreach (GameObject inventory_element in inventory_Element_list)
        {
            Craft_List mCraftList = inventory_element.GetComponent<Craft_List>();

            int Item_count = GameInstance.inventoryData[i].Number - mCraftList.itemData.Number;
            //Number.Add();

            i++;
        }
    }
}
