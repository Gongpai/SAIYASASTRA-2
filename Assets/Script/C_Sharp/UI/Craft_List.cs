using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Craft_List : MonoBehaviour
{
    [SerializeField] private GameObject Item;
    [SerializeField] private GameObject Image;
    [SerializeField] private TextMeshProUGUI NumbetItem;

    public Structs_Libraly.Item_Data itemData;
    private GameObject MainUI;
    private GameObject SpawnItem;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("อย่าลืมเปลี่ยน Getchild ถ้าเพิ่ม GameObject ใน Essential Menu");
        MainUI = GameObject.FindGameObjectWithTag("Essential_Menu").gameObject.transform.GetChild(3).gameObject;
        print(MainUI.gameObject.name);
    }

    private void OnDisable()
    {
        GameInstance.Player.GetComponent<Craft_System>().On_Craft_Disable();

        foreach (GameObject craft_slot in GameObject.FindGameObjectsWithTag("Craft_Slot"))
        {
            craft_slot.GetComponent<Craft_ItemSlot>().OnItem_Exit();
        }
    }

    // Update is called once per frame
    void Update()
    {
       NumbetItem.text = itemData.Number.ToString();
    }

    public void Spawn_Item()
    {
        if (itemData.Number > 0)
        {
            GameObject m_Item = Item;
            m_Item.GetComponent<Image>().sprite = itemData.itemSprite;
            m_Item.GetComponent<Drag_Drop_UI>().itemData = itemData;
            m_Item.GetComponent<Drag_Drop_UI>().Item_craft_list = gameObject;

            if (SpawnItem != null && !SpawnItem.GetComponent<Drag_Drop_UI>().IsDrag)
                Destroy(SpawnItem);

            SpawnItem = Instantiate(m_Item, MainUI.transform);
            SpawnItem.transform.position = Image.transform.position;
        }
    }

    public void Destroy_Spawn_Item()
    {
        if(SpawnItem != null)
            SpawnItem.GetComponent<Drag_Drop_UI>().Exit();
    }
}
