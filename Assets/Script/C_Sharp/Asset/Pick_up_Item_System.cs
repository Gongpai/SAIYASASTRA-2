using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick_up_Item_System : MonoBehaviour
{
    [Header("For Item Drop")]
    [SerializeField] private string PickUpMessage;
    [SerializeField] private int ItemIndex = 0;

    [Header("For Puzzle")]
    [SerializeField] private int PuzzleIndex = 0;

    private Structs_Libraly.Item_Data itemData;
    private GameObject Gameinstance;
    private ShowMessage pLayer;
    private bool CharacterEnter = false;
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
                PuzzleIndex,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].ItemPrefeb,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].useItemMode
            );
    }

    // Update is called once per frame
    void Update()
    {
        /**
        4646466465
        **/
    }

    public void PickUp_Item()
    {
        if (CharacterEnter)
        {
            GameInstance.Player.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Hide_Message();
            GameInstance.Player.GetComponent<Player_Movement>().Set_Block_Use_item(false);
            GameInstance.Player.GetComponent<Inventory_System>().Add_Item_Element(itemData);
            Destroy(gameObject);
            print(itemData.Name + "Drop Item Add - [" + itemData.Number + "]");
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            CharacterEnter = true;
            collider.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message(PickUpMessage);
            pLayer = collider.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>();
            //print("DDDDDDD");
            collider.GetComponent<Player_Movement>().Set_Block_Use_item(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            CharacterEnter = false;
            collider.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Hide_Message();
            collider.GetComponent<Player_Movement>().Set_Block_Use_item(false);
        }
    }
}
