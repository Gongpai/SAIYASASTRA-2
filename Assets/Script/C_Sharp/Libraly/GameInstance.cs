using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public static bool CharacterHide = false;

    public static GameObject Ghost;

    public static GameObject Player;
    public static List<Structs_Libraly.Item_Data> inventoryData = new List<Structs_Libraly.Item_Data>();
    public static List<Structs_Libraly.Item_Data> ShowItemElementData = new List<Structs_Libraly.Item_Data>();
    public static List<Structs_Libraly.Note_Data> noteData = new List<Structs_Libraly.Note_Data>();
    public static float LeftDistanceCam;
    public static float rightDistanceCam;

    public void Start()
    {
        Player = GameObject.FindWithTag("Player");
        LeftDistanceCam = FindLocationScreenZeroAndCenter(true);
        rightDistanceCam = FindLocationScreenZeroAndCenter(false);
    }

    public static void Reset_Gameinstance()
    {
        CharacterHide = false;
        Ghost = null;
        Player = null;
        inventoryData = new List<Structs_Libraly.Item_Data>();
        ShowItemElementData = new List<Structs_Libraly.Item_Data>();
        noteData = new List<Structs_Libraly.Note_Data>();
}

    public float FindLocationScreenZeroAndCenter(bool Iszero)
    {
        Vector3 ScreenCenter;
        Camera mainCamera = Camera.main;
        Transform cameraPoint = Instantiate(gameObject.transform.GetChild(0)).transform;
        Vector3 screenpoint;

        if (!Iszero)
        {
            screenpoint = new Vector3(Screen.width, (Screen.height / 2), 0);
        }
        else
        {
            screenpoint = new Vector3(0, (Screen.height / 2), 0);
        }

        screenpoint.z = Camera.main.nearClipPlane + 12;
        ScreenCenter = mainCamera.ScreenToWorldPoint(screenpoint);
        print((ScreenCenter) + " : " + (Screen.height / 2));

        if (!Iszero)
        {
            cameraPoint.position = new Vector3(ScreenCenter.x - 1f, Player.transform.position.y, Player.transform.position.z);
        }
        else
        {
            cameraPoint.position = new Vector3(ScreenCenter.x + 1f, Player.transform.position.y, Player.transform.position.z);
        }

        return Vector3.Distance(cameraPoint.position, Player.transform.position);
    }
}
