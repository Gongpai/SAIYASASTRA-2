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
    [SerializeField] public GameObject Touch_screen_UI;
    [SerializeField] private Transform AimPoint;
    [SerializeField] private Vector2 OffsetCentorAim;

    private GameObject gameInstance;
    private InputManager inputManager;
    public bool IsAim = false;
    private int SelectNum  = 0, maxSelect, Old_Select = 0;
    private Camera cameraMian;
    private Quaternion lookat;
    private List<GameObject> inventory_Element_list = new List<GameObject>();
    private List<GameObject> Equip_Element_list = new List<GameObject>();
    private List<GameObject> Equip_System = new List<GameObject>();
    public Animator animator;
    private Joystick joystick;
    Vector2 inputPos = new Vector2();
    
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

        animator = Inventory.transform.parent.transform.parent.GetComponent<Animator>();

        try
        {
            gameInstance = GameObject.FindGameObjectWithTag("GameInstance").gameObject;
        }
        catch
        {
            print("Not found GameInstance");
        }
        
        joystick = Touch_screen_UI.transform.GetChild(0).GetComponent<Joystick>();

        /**
        Add_Item_Element(gameInstance.gameObject.GetComponent<Item_List_Data>().itemDatas[0]);
        Add_Item_Element(gameInstance.gameObject.GetComponent<Item_List_Data>().itemDatas[0]);
        Add_Item_Element(gameInstance.gameObject.GetComponent<Item_List_Data>().itemDatas[1]);
        **/
    }

    // Update is called once per frame
    [Obsolete]
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

    public void PlayAnim(bool IsPlayIn)
    {
        if (IsPlayIn)
        {
            animator.SetBool("IsIn", true);
            animator.SetBool("IsOut", false);
        }
        else
        {
            animator.SetBool("IsIn", false);
            animator.SetBool("IsOut", true);
        }
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

    public void ResetIndex_Item_Element()
    {
        int i_el = 0;

        foreach (Item_Data item_Data_element in GameInstance.ShowItemElementData.ToList())
        {
            int i_inv = 0;
            foreach (Item_Data item_Data_inventory in GameInstance.inventoryData)
            {
                if (item_Data_element.Item_Index == item_Data_inventory.Item_Index)
                {
                    print("Item Reset : " + item_Data_element.Name);
                    Equip_Element_list[i_el].GetComponent<Equip_Item_List_System>().IndexEquip = i_inv;

                    GameInstance.ShowItemElementData[i_el] = Make_Structs.makeItemData
                    (
                        GameInstance.ShowItemElementData[i_el].Item_Index,
                        GameInstance.ShowItemElementData[i_el].Name,
                        GameInstance.ShowItemElementData[i_el].Number,
                        GameInstance.ShowItemElementData[i_el].itemSprite,
                        GameInstance.ShowItemElementData[i_el].IsEquip,
                        i_inv,
                        GameInstance.ShowItemElementData[i_el].ItemPrefeb,
                        GameInstance.ShowItemElementData[i_el].useItemMode
                    );

                    Equip_Element_list[i_el].GetComponent<Equip_Item_List_System>().itemData = GameInstance.ShowItemElementData[i_el];
                }

                i_inv++;
            }

            i_el++;
        }

        /**
        int i = 0;
        bool is_found = false;
        IsAim = false;

        foreach (var Elementlist in Equip_Element_list)
        {
            print("Item Reset : " + Elementlist.name);
            Destroy(Elementlist);
        }
;
        Equip_Element_list.Clear();

        GameInstance.ShowItemElementData.Clear();

        if (Equip_Element_list.Count > 0)
            Equip_Element_list[0].GetComponent<Animator>().SetBool("Is_Play?", true);

        foreach (Item_Data item_Data in GameInstance.inventoryData)
        {
            /**
            foreach (Item_Data item_Data_el in GameInstance.ShowItemElementData)
            {
                if(item_Data.Item_Index == item_Data_el.Item_Index)
                    is_found = true;
            }
            if (is_found)
            {

            }
            else
            {
            }
            
            if (item_Data.IsEquip)
            {
                print(item_Data + " : " + item_Data.Name);

                GameInstance.Player.gameObject.GetComponent<Inventory_System>().Add_Item_Equip(item_Data, null, false); //ถ้าไอเทมบัคมาแก้ตรงนี้
            }

            is_found = false;
            i++;
        }
        Set_Item_Element();
            **/
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

    public void Add_Item_Equip(Structs_Libraly.Item_Data Item = default, GameObject Owner = null, bool is_set = true)
    {
        if (GameInstance.ShowItemElementData.Count < 4)
        {
            GameInstance.ShowItemElementData.Add(Item);

            if (Owner != null)
                Equip_System.Add(Owner);
            
            if(is_set)
                Set_Item_Element();

            SelectNum = 0;
            Old_Select = 0;
        }
    }

    public void Add_Item_Element(Structs_Libraly.Item_Data Item_data)
    {
        print(Item_data.Name + "Sand to char ------------- [" + Item_data.Number + "] ---------------");
        int i = 0;
        bool IsFoundItem = false;
        foreach (Structs_Libraly.Item_Data itemData in GameInstance.inventoryData)
        {
            if (itemData.Name == Item_data.Name && itemData.useItemMode != Use_Item_System.Puzzle)
            {
                IsFoundItem = true;
                //print("Founddddd---------------------------------------");
                break;
            }
            else
            {
                if (!IsFoundItem)
                {
                    IsFoundItem = false;
                    i++;
                }
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
            print(GameInstance.inventoryData[i].Name + " Number in char ------------- [" + GameInstance.inventoryData[i].Number + "] ---------------");
        }
        else
        {
            GameInstance.inventoryData.Add(Item_data);
        }

        //print("Addd Item-------------------------------------------");
    }

    [Obsolete]
    public void Remove_Equip_Item(int index)
    {
        if(GameInstance.ShowItemElementData[index].useItemMode == Use_Item_System.Use_Light && GetComponent<Item_System>().EquipSlot.transform.GetChildCount() > 0)
        {
            Destroy(GetComponent<Item_System>().ItemEquip);
        }
        Equip_System.RemoveAt(index);
        GameInstance.ShowItemElementData.RemoveAt(index);
        Set_Item_Element();
        SelectNum = 0;
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

    //รีเซ็ต index
    public void Reset_Select_Index(int i = 0, int Old_i = 0)
    {
        SelectNum = i;
        Old_Select = Old_i;
    }

    //ระบบเลือกไอเทม
    public void Select_Number_List(int Number, bool select_for_touch = false, int select_number = 0)
    {
        if (GameInstance.ShowItemElementData.Count > 0)
        {
            if (!select_for_touch)
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
            }
            else
            {
                if(SelectNum == select_number)
                {
                    Aim(!IsAim);
                    print(SelectNum + " Num : Sel " + select_number + " ISaIM : " + IsAim);
                } else
                {
                    IsAim = false;
                }
                SelectNum = select_number;
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

    [Obsolete]
    private void ArrowAim()
    {
        if (Input.touches.Length > 0)
        {
            if (!joystick.isPress || Input.touches.Length > 1)
            {
                inputPos = Input.GetTouch(Input.touches.Length - 1).position;
            }
        }
        
        if (IsAim)
        {
            if (Touch_screen_UI.active)
            {
                Vector2 screenSize = new Vector2(Screen.width, Screen.height) * 0.5f;
                Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(AimPoint.position);
                print($"player pixel X : {playerScreenPoint.x} | Y : {playerScreenPoint.y}");
                Vector2 input = new Vector2(inputPos.x - (playerScreenPoint.x + OffsetCentorAim.x), inputPos.y - (playerScreenPoint.y + OffsetCentorAim.y));
                print($"Aim X : {input.x} | Y : {input.y}");
                input.Normalize();

                if(input != Vector2.zero)
                {
                    Quaternion ArrowRot = Quaternion.LookRotation(Vector3.forward, input);
                    if (ArrowRot.w > 0.45f || ArrowRot.w < -0.45f)
                    {
                        lookat = ArrowRot;
                        Arrow_Aim.transform.rotation = lookat;
                    }
                }
            }
            else
            {
                Quaternion ArrowRot = Quaternion.Euler(0, 0, FuntionLibraly.Get2DLookAt(Arrow_Aim.transform.position, new Vector2(inputManager.horizontalLookAxis, inputManager.verticalLookAxis)) - 90);

                if (ArrowRot.w > 0.45f || ArrowRot.w < -0.45f)
                {
                    lookat = ArrowRot;
                    Arrow_Aim.transform.rotation = lookat;

                }
                //print("Rotation : " + ArrowRot + " --------------------");
            }
        }

        Arrow_Aim.SetActive(IsAim);
    }

    public void Aim(bool is_Aim)
    {
        if (GameInstance.ShowItemElementData.Count > 0 && Equip_Element_list[SelectNum].GetComponent<Equip_Item_List_System>().itemData.useItemMode == Use_Item_System.Shoot_Projectile)
        {
            IsAim = is_Aim;
        }
    }

    [Obsolete]
    public void Use_Item_Equip(bool is_remove = false)
    {
        if (GameInstance.ShowItemElementData.Count > 0)
        {
            Structs_Libraly.Item_Data itemData = Equip_Element_list[SelectNum].GetComponent<Equip_Item_List_System>().itemData;

            if (itemData.useItemMode != Use_Item_System.Shoot_Projectile && itemData.useItemMode != Use_Item_System.Shoot_horizontal)
                GetComponent<Item_System>().Use_Item(itemData.useItemMode, itemData.ItemPrefeb, IsAim);
        }
    }

    [Obsolete]
    public void Shoot_Item()
    {
        print(SelectNum);
        Use_Item_System use_Item_Equip = default;

        if (Equip_Element_list.Count > 0)
        {
            use_Item_Equip = Equip_Element_list[SelectNum].GetComponent<Equip_Item_List_System>().itemData.useItemMode;

            if (GameInstance.ShowItemElementData.Count > 0)
            {
                Structs_Libraly.Item_Data itemData = Equip_Element_list[SelectNum].GetComponent<Equip_Item_List_System>().itemData;
                //Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, gameObject.transform.position.z);
                //float force = Vector3.Distance(gameObject.transform.position, mousePos);

                switch (use_Item_Equip)
                {
                    case Use_Item_System.Shoot_Projectile:
                        if (IsAim)
                        {
                            gameObject.GetComponent<Item_System>().Use_Item(itemData.useItemMode, itemData.ItemPrefeb, IsAim, lookat);
                            Remove_Number_Item(itemData);
                        }
                        break;
                    case Use_Item_System.Shoot_horizontal:
                        gameObject.GetComponent<Item_System>().Use_Item(itemData.useItemMode, itemData.ItemPrefeb, IsAim, lookat);
                        Remove_Number_Item(itemData);
                        break;
                }
            }
        }
    }

    private void Remove_Number_Item(Item_Data itemData)
    {
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
