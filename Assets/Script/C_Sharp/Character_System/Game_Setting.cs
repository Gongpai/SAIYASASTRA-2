using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Setting : MonoBehaviour
{
    public static bool Is_Auto_JoyStick_Enable = true;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
