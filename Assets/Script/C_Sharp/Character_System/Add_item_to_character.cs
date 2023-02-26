using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Add_item_to_character : FuntionLibraly
{
    [SerializeField] private int ItemIndex = 0;
    [SerializeField] public bool IsSpawn = false;
    [SerializeField] new Collider collider = new Collider();

    public GameObject ghost;
    public AiGhost ai_ghost;
    public bool isFlip;
    private Vector3 velocity;

    private Structs_Libraly.Item_Data itemData;

    private GameObject Gameinstance;

    public delegate void PauseGame();
    public static PauseGame OnPauseGame;
    public static PauseGame UnPauseGame;

    // Start is called before the first frame update
    private void Start()
    {
        Gameinstance = GameObject.FindGameObjectWithTag("GameInstance").gameObject;
        itemData = new Structs_Libraly.Item_Data
            (
                ItemIndex,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].Name,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].Number,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].itemSprite,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].IsEquip,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].Index,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].ItemPrefeb,
                Gameinstance.GetComponent<Item_List_Data>().itemDatas[ItemIndex].useItemMode
            );

        if (IsSpawn)
        {
            OnPauseGame += Set_PauseGame;
            UnPauseGame += Set_UnPauseGame;
        }
    }

    public void Set_PauseGame()
    {
        try
        {
            velocity = GetComponent<Rigidbody>().velocity;
            GamePause_Component(gameObject, true);
        }
        catch
        {

        }
    }
    public void Set_UnPauseGame()
    {
        try
        {
            GamePause_Component(gameObject, false);

            switch (ai_ghost)
            {
                case AiGhost.Hungry_ghost:
                    ghost.GetComponent<Ai_Attack>().Shoot_projectile(gameObject, false, velocity);
                    break;
                case AiGhost.Home_ghost:
                    ghost.GetComponent<Ai_Attack>().Shoot_horizontal(isFlip,gameObject, false);
                    break;
                default:
                    break;
            }
        }
        catch
        {

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !IsSpawn)
        {
            other.gameObject.GetComponent<Inventory_System>().Add_Item_Element(itemData);
            Destroy(this.gameObject);
            print("Adddddd-----------------------");
        }

        if (IsSpawn)
        {
            if (other.isTrigger && other.tag == "Player" && gameObject.tag == "Ghost_Attack")
            {
                print("HP------------------------------------- " + other);
                other.GetComponent<Player_Movement>().HP_System(collider, -1);
            }
            
            if (other.isTrigger && other.tag == "Player" && gameObject.tag == "Attack_Item" && other.gameObject == ghost && GetComponent<Rigidbody>().velocity.y <= 0)
            {
                Debug.LogWarning("HPP_ADDDD : " + other.tag + "Is : " + (other.gameObject == ghost) + "VVVV : " + GetComponent<Rigidbody>().velocity.y);
                other.GetComponent<Player_Movement>().HP_System(collider, 1);
            }
        }
    }
}
