using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Setting : MonoBehaviour
{
    public static bool Is_Auto_JoyStick_Enable = true;
    public static bool Is_Spawn_Game_Setting = false;
    void Start()
    {
        if (!Is_Spawn_Game_Setting)
        {
            DontDestroyOnLoad(gameObject);
            Is_Spawn_Game_Setting = true;
        }
    }
}
