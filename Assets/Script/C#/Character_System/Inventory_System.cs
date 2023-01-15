using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Structs_Libraly;

public class Inventory_System : MonoBehaviour
{
    [SerializeField] private GameObject List_Item;
    [SerializeField] private List<GameObject> Element;
    [SerializeField] private GameObject List_Grid;
    [SerializeField] private GameObject List_Grid_Element;
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject Inventory_Element;

    private GameObject gameInstance;
    private InputManager inputManager;
    private int SelectNum  = 0, maxSelect, Old_Select = 0;
    private List<GameObject> inventory_Element_list = new List<GameObject>();
    private List<GameObject> Equip_Element_list = new List<GameObject>();
    private List<GameObject> Equip_System = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (inputManager == null)
        {
            inputManager = InputManager.instance;
        }

        if (inputManager == null)
        {
            Debug.LogWarning("No player");
        }

        gameInstance = GameObject.FindGameObjectWithTag("GameInstance").gameObject;

        Add_Item_Element(gameInstance.gameObject.GetComponent<Item_List_Data>().itemDatas[0]);
        Add_Item_Element(gameInstance.gameObject.GetComponent<Item_List_Data>().itemDatas[0]);
        Add_Item_Element(gameInstance.gameObject.GetComponent<Item_List_Data>().itemDatas[1]);
    }

    // Update is called once per frame
    void Update()
    {
        Select_Item();
    }

    void Awake()
    {
        Game_State_Manager.Instance.OnGameStateChange += OnGamestateChanged;
    }

    void OnDestroy()
    {
        Game_State_Manager.Instance.OnGameStateChange -= OnGamestateChanged;
    }

    private void OnGamestateChanged(GameState gameState)
    {
        enabled = gameState == GameState.Play;
    }

    //ระบบเลื่อนเมาส์กลางขึ้น-ลง
    public void Select_Item()
    {
        if (inputManager.AxisScroll > 0)
        {
            Select_Number_List(-1);

        }
        else if (inputManager.AxisScroll < 0)
        {
            Select_Number_List(1);
        }
    }

    //ระบบเซตไอเทม
    public void Set_Item_Element()
    {
        int i = 0;

        GameObject List_Grid_Item;
        List_Grid_Item = Inventory.transform.GetChild(0).GetChild(0).gameObject;

        if (Equip_Element_list.Count > 0)
        {
            foreach (GameObject list in Equip_Element_list)
            {
                Destroy(list.gameObject);
            }

            Equip_Element_list.Clear();
        }

        foreach (Structs_Libraly.Item_Data Item in GameInstance.ShowItemElementData)
        {
            GameObject element_Item = List_Grid_Element;
            GameObject item_Inventory_list;

            element_Item.GetComponent<Equip_Item_List_System>().IndexEquip = Item.Index;
            element_Item.GetComponent<Equip_Item_List_System>().itemData = Item;
            element_Item.transform.GetChild(3).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(Item.Name);
            element_Item.transform.GetChild(4).GetComponent<Image>().sprite = Item.itemSprite;
            element_Item.transform.GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(Item.Number.ToString());

            GameInstance.inventoryData[Item.Index] = new Structs_Libraly.Item_Data
            (
                GameInstance.inventoryData[Item.Index].Name,
                GameInstance.inventoryData[Item.Index].Number,
                GameInstance.inventoryData[Item.Index].itemSprite,
                GameInstance.inventoryData[Item.Index].IsEquip,
                i,
                GameInstance.inventoryData[Item.Index].Owner
            );

            item_Inventory_list = Instantiate(element_Item, List_Grid.transform);
            Instantiate(item_Inventory_list);

            print("Item Name : " + Item.Name);
            
            Equip_Element_list.Add(item_Inventory_list);

            i++;
        }

        Get_Item_Element();
    }

    public void Set_Inventory_Element()
    {
        int i = 0;
        GameObject List_Grid_Item;
        List_Grid_Item = Inventory.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        Equip_System.Clear();

        if (inventory_Element_list.Count > 0)
        {
            foreach (GameObject list in inventory_Element_list)
            {
                Destroy(list.gameObject);
            }
        }
        
        foreach (Structs_Libraly.Item_Data item in GameInstance.inventoryData)
        {
            GameObject item_Inventory = Inventory_Element;
            GameObject item_Inventory_list;

            item_Inventory.GetComponent<Eqip_Item_System>().itemData = item;
            item_Inventory.GetComponent<Eqip_Item_System>().IndexInventory = i;
            item_Inventory.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(item.Name);
            item_Inventory.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(item.Number.ToString());
            item_Inventory.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = item.itemSprite;



            item_Inventory_list = Instantiate(item_Inventory, List_Grid_Item.transform);
            Instantiate(item_Inventory_list);

            inventory_Element_list.Add(item_Inventory_list);
            Equip_System.Add(item_Inventory_list);
            //print("Workkkkkkkkkkkkkkkkkk-------------------------------------------");

            i++;
        }
        
    }

    public void Add_Item_Equip(Structs_Libraly.Item_Data Item, GameObject Owner = null)
    {
        GameInstance.ShowItemElementData.Add(Item);

        if(Owner != null)
            Equip_System.Add(Owner);

        Set_Item_Element();
    }

    public void Add_Item_Element(Structs_Libraly.Item_Data Item)
    {
        int i = 0;
        bool IsFoundItem = false;
        foreach (Structs_Libraly.Item_Data itemData in GameInstance.inventoryData)
        {
            if (itemData.Name == Item.Name)
            {
                IsFoundItem = true;
                //print("Founddddd---------------------------------------");
                break;
            }
            else
            {
                IsFoundItem = false;
                i++;
            }

        }

        if (IsFoundItem)
        {
            GameInstance.inventoryData[i] = new Structs_Libraly.Item_Data
                (
                    GameInstance.inventoryData[i].Name, 
                    GameInstance.inventoryData[i].Number + Item.Number, 
                    GameInstance.inventoryData[i].itemSprite, 
                    GameInstance.inventoryData[i].IsEquip, 
                    GameInstance.inventoryData[i].Index,
                    GameInstance.inventoryData[i].Owner
                );
        }
        else
        {
            GameInstance.inventoryData.Add(Item);
        }

        //print("Addd Item-------------------------------------------");
    }

    public void Remove_Equip_Item(int index)
    {
        Equip_System.RemoveAt(index);
        GameInstance.ShowItemElementData.RemoveAt(index);
        Set_Item_Element();
    }

    //ระบบดูว่า item list มี element กี่อัน
    private void Get_Item_Element()
    {
        maxSelect = Equip_Element_list.Count - 1;
        //print("Max Select : " + maxSelect);

        if(GameInstance.ShowItemElementData.Count > 0)
            Equip_Element_list[0].GetComponent<Animator>().SetBool("Is_Play?", true);
    }

    //ระบบเลือกไอเทม
    void Select_Number_List(int Number)
    {
        if (GameInstance.ShowItemElementData.Count > 0)
        {
            SelectNum += Number;
            if (SelectNum > maxSelect)
            {
                SelectNum = 0;
            }
            else if (SelectNum < 0)
            {
                SelectNum = maxSelect;
            }

            //print("Select is : " + SelectNum);
            Equip_Element_list[SelectNum].GetComponent<Animator>().SetBool("Is_Play?", true);

            if (Old_Select != SelectNum && maxSelect > 0)
            {
                Equip_Element_list[Old_Select].GetComponent<Animator>().SetBool("Is_Play?", false);
                //print("Old Select is : " + SelectNum);
            }

            Old_Select = SelectNum;
        }
        else
        {
            SelectNum = 0;
            Old_Select = 0;
        }
    }
}
