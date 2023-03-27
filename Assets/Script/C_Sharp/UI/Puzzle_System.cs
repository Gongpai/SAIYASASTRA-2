using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_System : MonoBehaviour
{
    [SerializeField] List<Puzzle_Drag_Drop_UI> puzzle_element = new List<Puzzle_Drag_Drop_UI>();
    [SerializeField] private int Inventory_iten_index;
    [SerializeField] private int Number_Puzzle = 5;

    public GameObject DoorUnlock;
    public Show_Puzzle ShowPuzzle;
    public static bool IsPuzzleSucceed = false;
    // Start is called before the first frame update
    void Start()
    {
        if (puzzle_element.Count > 0)
        {
            foreach (Structs_Libraly.Item_Data item in GameInstance.inventoryData)
            {
                if (item.useItemMode == Use_Item_System.Puzzle && item.Item_Index == Inventory_iten_index)
                {
                    foreach (Puzzle_Drag_Drop_UI puzzle_ in puzzle_element)
                    {
                        print("Number index : " + item.Index + " : " + puzzle_.Index);
                        if (item.Index == puzzle_.Index)
                        {
                            puzzle_.Set_Enable_Puzzle_Element();
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Puzzle_Slot_System.puzzle.Count >= Number_Puzzle)
        {
            IsPuzzleSucceed = true;
        }
    }

    public void Close()
    {
        if (IsPuzzleSucceed)
        {
            Remove_Puzzle_Item();
            ShowPuzzle.Can_Open_Puzzle = false;
            Flashing_Lights.event_Light_On_Off(Flashing_Lights.Light_Mode.Turn_On);
            Flashing_Lights.event_Light_On_Off(Flashing_Lights.Light_Mode.Flashing);
            DoorUnlock.GetComponent<Open_Room_New_Scene>().Cant_OpenDoor = false;
        }

        if (GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect != null)
        {
            if (GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect.GetComponent<Animator>().GetBool("IsPlay"))
                GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect.SetActive(true);
        }

        GameInstance.Player.GetComponent<Inventory_System>().ResetIndex_Item_Element();
        Game_State_Manager.Instance.Setstate(GameState.Play);
        Puzzle_Slot_System.puzzle.Clear();
        Destroy(gameObject);
    }

    private void Remove_Puzzle_Item()
    {
        int count = GameInstance.inventoryData.Count;
        int index = count - 1;
        for (int i = 0; i < count;)
        {
            try
            {
                if (GameInstance.inventoryData[index - i].useItemMode == Use_Item_System.Puzzle && GameInstance.inventoryData[index - i].Item_Index == Inventory_iten_index)
                {
                    GameInstance.inventoryData.RemoveAt(index - i);
                }
            }
            catch
            {
                print("End Item");
            }

            i++;
        }

        GameInstance.Player.GetComponent<Inventory_System>().Reset_Select_Index();

        /**
        GameInstance.inventoryData.RemoveAll(i  GameInstance.inventoryData[0].useItemMode == Use_Item_System.Puzzle);
        foreach (Structs_Libraly.Item_Data item in GameInstance.inventoryData)
        {
            if (item.useItemMode == Use_Item_System.Puzzle && item.Item_Index == Inventory_iten_index)
            {
                GameInstance.inventoryData.RemoveAt(i);
            }
            i++;
        }
        **/
    }
}
 
