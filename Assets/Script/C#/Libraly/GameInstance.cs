using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public void Start()
    {
        DontDestroyOnLoad(this);
    }

    public static void Reset_Gameinstance()
    {
        CharacterHide = false;
    }
    
    public static bool CharacterHide = false;

    public static GameObject Ghost;

    public static GameObject Player;
}
