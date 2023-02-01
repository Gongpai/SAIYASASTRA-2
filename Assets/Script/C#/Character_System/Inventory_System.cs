using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
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
    [SerializeField] private GameObject Inventory_Info;
    [SerializeField] private GameObject Note;
    [SerializeField] public GameObject Arrow_Aim;

    private GameObject gameInstance;
    private InputManager inputManager;
    public bool IsAim = false;
    private int SelectNum  = 0, maxSelect, Old_Select = 0;
    private Camera cameraMian;
    private Quaternion lookat;
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

        /**
        Add_Item_Element(gameInstance.gameObject.GetComponent<Item_List_Data>().itemDatas[0]);
        Add_Item_Element(gameInstance.gameObject.GetComponent<Item_List_Data>().itemDatas[0]);
        Add_Item_Element(gameInstance.gameObject.GetComponent<Item_List_Data>().itemDatas[1]);
        **/
    }

    // Update is called once per frame
    void Update()
    {
        Select_Item();

        ArrowAim();
    }

    void Awake()
    {
        cameraMian = Camera.main;
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

    public void ResetIndex(int index, int re_index)
    {
        GameInstance.ShowItemElementData[index] = new Structs_Libraly.Item_Data
        (
            GameInstance.ShowItemElementData[index].Item_Index,
            GameInstance.ShowItemElementData[index].Name,
            GameInstance.ShowItemElementData[index].Number,
            GameInstance.ShowItemElementData[index].itemSprite,
            GameInstance.ShowItemElementData[index].IsEquip,
            re_index,
            GameInstance.ShowItemElementData[index].ItemPrefeb,
            GameInstance.ShowItemElementData[index].useItemMode
        );

        Equip_Element_list[index].GetComponent<Equip_Item_List_System>().IndexEquip = re_index;
    }

    //ระบบเซตไอเทม
    public void Set_Item_Element()
    {
        int i = 0;
        IsAim = false;

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
                GameInstance.inventoryData[Item.Index].Item_Index,
                GameInstance.inventoryData[Item.Index].Name,
                GameInstance.inventoryData[Item.Index].Number,
                GameInstance.inventoryData[Item.Index].itemSprite,
                GameInstance.inventoryData[Item.Index].IsEquip,
                i,
                GameInstance.inventoryData[Item.Index].ItemPrefeb,
                GameInstance.inventoryData[Item.Index].useItemMode
            );

            item_Inventory_list = Instantiate(element_Item, List_Grid.transform);

            Equip_Element_list.Add(item_Inventory_list);

            i++;
        }

        Get_Item_Element();
    }

    //ระบบเชตช่องเก็บของ
    public void Set_Inventory_Element()
    {
        int i = 0;
        GameObject List_Grid_Item = List_Grid_Item = Inventory.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        
        Equip_System.Clear();

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
            GameObject item_Inventory = Inventory_Element;
            GameObject item_Inventory_list;

            item_Inventory.GetComponent<Eqip_Item_System>().itemData = item;
            item_Inventory.GetComponent<Eqip_Item_System>().IndexInventory = i;
            item_Inventory.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(item.Name);
            item_Inventory.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>()
                .SetText(item.Number.ToString());
            item_Inventory.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = item.itemSprite;

            item_Inventory_list = Instantiate(item_Inventory, List_Grid_Item.transform);

            inventory_Element_list.Add(item_Inventory_list);
            Equip_System.Add(item_Inventory_list);
            //print("Workkkkkkkkkkkkkkkkkk-------------------------------------------");



            i++;
        }
    }

    public void Add_Item_Equip(Structs_Libraly.Item_Data Item = default, GameObject Owner = null)
    {
        if (GameInstance.ShowItemElementData.Count < 4)
        {
            GameInstance.ShowItemElementData.Add(Item);

            if (Owner != null)
                Equip_System.Add(Owner);

            Set_Item_Element();

            SelectNum = 0;
            Old_Select = 0;
        }
    }

    public void Add_Item_Element(Structs_Libraly.Item_Data Item_data)
    {
        print("Adddddddddddddd------------------------------");
        int i = 0;
        bool IsFoundItem = false;
        foreach (Structs_Libraly.Item_Data itemData in GameInstance.inventoryData)
        {
            if (itemData.Name == Item_data.Name)
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
                    GameInstance.inventoryData[i].Item_Index,
                    GameInstance.inventoryData[i].Name, 
                    GameInstance.inventoryData[i].Number + Item_data.Number, 
                    GameInstance.inventoryData[i].itemSprite, 
                    GameInstance.inventoryData[i].IsEquip, 
                    GameInstance.inventoryData[i].Index,
                    GameInstance.inventoryData[i].ItemPrefeb,
                    GameInstance.inventoryData[i].useItemMode
                );
        }
        else
        {
            GameInstance.inventoryData.Add(Item_data);
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

    public void Set_Inventory_Info(Structs_Libraly.Item_Data itemData)
    {
        Inventory_Info.SetActive(true);
        Image image = Inventory_Info.transform.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI text = Inventory_Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        image.sprite = itemData.itemSprite;
        print("Item" + itemData.Item_Index);
        text.text = Dialog_Manager.Dialog_Text(0, 0, SelectDialog.note_title, "Item/ItemText", new XML_Data("Text", "Item", "Item" + itemData.Item_Index, "text", "text"));
        
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

            IsAim = false;
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

    private void ArrowAim()
    {
        if (IsAim)
        {
            Quaternion ArrowRot = Quaternion.Euler(0, 0, FuntionLibraly.Get2DLookAt(Arrow_Aim.transform.position, new Vector2(inputManager.horizontalLookAxis, inputManager.verticalLookAxis)) - 90);

            if (ArrowRot.w > 0.45f || ArrowRot.w < -0.45f)
            {
                lookat = ArrowRot;
                Arrow_Aim.transform.rotation = lookat;
                
            }
            //print("Rotation : " + ArrowRot + " --------------------");
        }


        Arrow_Aim.SetActive(IsAim);
    }

    public void Aim(bool is_Aim)
    {
        if (GameInstance.ShowItemElementData.Count > 0)
        {
            Structs_Libraly.Item_Data itemData = Equip_Element_list[SelectNum].GetComponent<Equip_Item_List_System>().itemData;
            if (itemData.useItemMode == Use_Item_System.Shoot_Projectile)
            {
                IsAim = is_Aim;
            }

        }
    }

    public void Use_Item_Equip()
    {
        if (GameInstance.ShowItemElementData.Count > 0)
        {
            Structs_Libraly.Item_Data itemData =
                Equip_Element_list[SelectNum].GetComponent<Equip_Item_List_System>().itemData;
            if (itemData.useItemMode != Use_Item_System.Shoot_Projectile)
                gameObject.GetComponent<Item_System>().Use_Item(itemData.useItemMode, itemData.ItemPrefeb, IsAim);
        }
    }

    public void Shoot_Item()
    {
        if (GameInstance.ShowItemElementData.Count > 0)
        {
            Structs_Libraly.Item_Data itemData = Equip_Element_list[SelectNum].GetComponent<Equip_Item_List_System>().itemData;
            if (itemData.useItemMode == Use_Item_System.Shoot_Projectile && IsAim)
            {
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, gameObject.transform.position.z);
                float force = Vector3.Distance(gameObject.transform.position, mousePos);
                gameObject.GetComponent<Item_System>().Use_Item(itemData.useItemMode, itemData.ItemPrefeb, IsAim, lookat);

                if (GameInstance.inventoryData[itemData.Index].Number > 0)
                {
                    //print("Count Data : " + Equip_Element_list.Count + " | Index Count" + GameInstance.inventoryData.Count + " | Last Index : " + GameInstance.inventoryData.LastIndexOf(GameInstance.inventoryData.Last()));

                    GameInstance.inventoryData[itemData.Index] = Make_Structs.makeItemData
                        (
                            GameInstance.inventoryData[itemData.Index].Item_Index,
                            GameInstance.inventoryData[itemData.Index].Name,
                            GameInstance.inventoryData[itemData.Index].Number - 1,
                            GameInstance.inventoryData[itemData.Index].itemSprite,
                            GameInstance.inventoryData[itemData.Index].IsEquip,
                            GameInstance.inventoryData[itemData.Index].Index,
                            GameInstance.inventoryData[itemData.Index].ItemPrefeb,
                            GameInstance.inventoryData[itemData.Index].useItemMode
                        );

                    GameInstance.ShowItemElementData[SelectNum] = GameInstance.inventoryData[itemData.Index];
                }

                RemoveItemFromInventory(itemData);
            }
        }
    }

    public void RemoveItemFromInventory(Structs_Libraly.Item_Data itemData)
    {
        if (GameInstance.inventoryData[itemData.Index].Number == 0)
        {
            IsAim = false;

            if (Equip_Element_list.Count > 0)
            {
                Equip_Element_list[Old_Select].GetComponent<Animator>().SetBool("Is_Play?", false);
                Destroy(Equip_Element_list[SelectNum]);
                Equip_Element_list.RemoveAt(SelectNum);
            }

            if (GameInstance.inventoryData[itemData.Index].Number <= 0)
            {
                foreach (var Elementlist in Equip_Element_list)
                {

                    Destroy(Elementlist);
                }
;
                Equip_Element_list.Clear();
            }

            GameInstance.ShowItemElementData.Clear();
            GameInstance.inventoryData.RemoveAt(itemData.Index);

            if (Equip_Element_list.Count > 0)
                Equip_Element_list[0].GetComponent<Animator>().SetBool("Is_Play?", true);

            maxSelect = Equip_Element_list.Count - 1;
            SelectNum = 0;
            Old_Select = 0;


            Set_Inventory_Element();
            foreach (GameObject inventory_Element in inventory_Element_list)
            {
                if (inventory_Element.GetComponent<Eqip_Item_System>().itemData.IsEquip)
                    inventory_Element.GetComponent<Eqip_Item_System>().ReEquip_Item();
            }

            //print(GameInstance.inventoryData[itemData.Index].Name + " Total_Number : " + GameInstance.inventoryData[itemData.Index].Number);
            Set_Item_Element();
        }
    }
}
