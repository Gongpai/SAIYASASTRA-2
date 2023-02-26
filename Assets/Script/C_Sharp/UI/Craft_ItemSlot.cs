using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Structs_Libraly;

public class Craft_ItemSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private int Number_Slot = 0;
    
    public bool IsItemDrop = false;

    private GameObject Item_craft;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.LogWarning("Slot_OnDrop");
        GameObject drop = eventData.pointerDrag;
        Drag_Drop_UI dragDrop = drop.GetComponent<Drag_Drop_UI>();

        if (drop != null && dragDrop != null)
        {
            //ถ้าเปลี่ยนช่องให้ ช่องเก่าตั้งเป็น false
            if (dragDrop.CraftSlot != null)
            {
                dragDrop.CraftSlot.GetComponent<Craft_ItemSlot>().OnItem_Exit();
                print("Switch to new slot");
            }

            //ถ้าเอาไปวางช่องที่มีไอเทมแล้วก็จะให้มันไปที่ช่องเดิม
            if (IsItemDrop)
            {
                print("This slot have item");
                dragDrop.transform.position = dragDrop.CraftSlot.transform.GetChild(0).position;
            }

            //ถ้าว่างในช่องคราฟในลบจำนวนไอเทมไป 1 อัน
            Craft_List m_craft_list = drop.GetComponent<Drag_Drop_UI>().Item_craft_list.GetComponent<Craft_List>();
            if (!dragDrop.IsDrop)
            {
                m_craft_list.itemData = new Structs_Libraly.Item_Data
                (
                    m_craft_list.GetComponent<Craft_List>().itemData.Item_Index,
                    m_craft_list.GetComponent<Craft_List>().itemData.Name,
                    m_craft_list.GetComponent<Craft_List>().itemData.Number - 1,
                    m_craft_list.GetComponent<Craft_List>().itemData.itemSprite,
                    m_craft_list.GetComponent<Craft_List>().itemData.IsEquip,
                    m_craft_list.GetComponent<Craft_List>().itemData.Index,
                    m_craft_list.GetComponent<Craft_List>().itemData.ItemPrefeb,
                    m_craft_list.GetComponent<Craft_List>().itemData.useItemMode
                );
            }

            //วางช่องที่ยังไม่มีไอเทม
            if (!IsItemDrop)
            {
                drop.transform.position = gameObject.transform.GetChild(0).position;

                if (dragDrop.IsDrop)
                    GameInstance.Player.GetComponent<Craft_System>().Remove_Item_Code(dragDrop.old_number);

                Item_craft = drop;

                dragDrop.CraftSlot = gameObject;
                dragDrop.IsDrop = true;
                dragDrop.old_number = Number_Slot;
                GameInstance.Player.GetComponent<Craft_System>().Update_Item_Code((dragDrop.itemData.Item_Index + 1).ToString(), Number_Slot);

                IsItemDrop = true;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Remove_item();
        }
    }

    public void OnItem_Exit()
    {
        Item_craft = null;
        print("item leave");
        IsItemDrop = false;
        GameInstance.Player.GetComponent<Craft_System>().Remove_Item_Code(Number_Slot);
    }

    public void Remove_item()
    {
        if (Item_craft != null)
        {
            GameInstance.Player.GetComponent<Craft_System>().Remove_Item_Code(Number_Slot);

            Craft_List m_craft_list = Item_craft.GetComponent<Drag_Drop_UI>().Item_craft_list.GetComponent<Craft_List>();
            m_craft_list.itemData = new Structs_Libraly.Item_Data
            (
                m_craft_list.GetComponent<Craft_List>().itemData.Item_Index,
                m_craft_list.GetComponent<Craft_List>().itemData.Name,
                m_craft_list.GetComponent<Craft_List>().itemData.Number + 1,
                m_craft_list.GetComponent<Craft_List>().itemData.itemSprite,
                m_craft_list.GetComponent<Craft_List>().itemData.IsEquip,
                m_craft_list.GetComponent<Craft_List>().itemData.Index,
                m_craft_list.GetComponent<Craft_List>().itemData.ItemPrefeb,
                m_craft_list.GetComponent<Craft_List>().itemData.useItemMode
            );

            Destroy(Item_craft);

            Item_craft = null;
            print("item remove");
            IsItemDrop = false;
        }
        else
        {
            print("This slot not have item");
        }
    }

}
