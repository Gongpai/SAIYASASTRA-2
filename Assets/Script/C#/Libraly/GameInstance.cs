using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public void Start()
    {
        
    }

    public static void Reset_Gameinstance()
    {
        CharacterHide = false;
    }
    
    public static bool CharacterHide = false;

    public static GameObject Ghost;

    public static GameObject Player;
    public static List<Structs_Libraly.Item_Data> inventoryData = new List<Structs_Libraly.Item_Data>();
    public static List<Structs_Libraly.Item_Data> ShowItemElementData = new List<Structs_Libraly.Item_Data>();
    public static List<Structs_Libraly.Note_Data> noteData = new List<Structs_Libraly.Note_Data>();
}
