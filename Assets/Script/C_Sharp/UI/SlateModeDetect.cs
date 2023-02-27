using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class SlateModeDetect : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(SystemMetric smIndex);
    public static ConvertibleMode currentMode;
    private bool is_Update = true;
    public void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (GetSystemMetrics(SystemMetric.SM_CONVERTABLESLATEMODE) == 0)
            {
                if (Input.touchSupported && Game_Setting.Is_Auto_JoyStick_Enable)
                {
                    currentMode = ConvertibleMode.SlateTabletMode;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    //Debug.Log("slate/tablet mode | SystemMetrics :" + GetSystemMetrics(SystemMetric.SM_CONVERTABLESLATEMODE) + " | TouchScreen : " + Input.touchSupported);
                    if (is_Update && GameInstance.Player != null)
                    {
                        is_Update = false;
                        GameInstance.Player.GetComponent<Player_Movement>().Set_Platform();
                    }
                } 
                else
                {
                    currentMode = ConvertibleMode.LaptopDockedMode;
                }
            }
            else
            {
                currentMode = ConvertibleMode.LaptopDockedMode;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                //Debug.Log("docked/laptop mode | system :" + GetSystemMetrics(SystemMetric.SM_CONVERTABLESLATEMODE) + " | TouchScreen : " + Input.touchSupported);

                is_Update = true;
            }
        }
    }
}
