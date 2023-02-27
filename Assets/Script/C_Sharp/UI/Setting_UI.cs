using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Setting_UI : MonoBehaviour
{
    [SerializeField] Toggle Fullscreen;
    [SerializeField] TextMeshProUGUI textFullscreen;
    [SerializeField] Button Fullscreen_Button;
    [SerializeField] Toggle Auto_JoyStick_Enable;
    [SerializeField] TextMeshProUGUI textAuto_JoyStick;
    [SerializeField] Button Auto_JoyStick_Button;
    [SerializeField] Slider Music;
    [SerializeField] TextMeshProUGUI textMusic;
    [SerializeField] Slider SFX;
    [SerializeField] TextMeshProUGUI textSFX;
    [SerializeField] AudioMixer MusicMixer;
    [SerializeField] AudioMixer SFXMixer;

    // Start is called before the first frame update
    void Start()
    {
        if (!Input.touchSupported || Application.platform == RuntimePlatform.Android)
        {
            Auto_JoyStick_Enable.interactable = false;
            Auto_JoyStick_Enable.isOn = false;
            Game_Setting.Is_Auto_JoyStick_Enable = false;
            Auto_JoyStick_Button.interactable = false;
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                textAuto_JoyStick.text = "อุปกรณ์ไม่รองรับ";
            }
            else
            {
                textFullscreen.text = "ปิดใช้งาน";
                textAuto_JoyStick.text = "ปิดใช้งาน";
                Fullscreen.interactable = false;
                Fullscreen.isOn = false;
                Fullscreen_Button.interactable = false;
            }
        }
        if (Input.touchSupported)
            Update_Setting_Toggle(true);

        Update_Setting_Slider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Update_Setting_Toggle(bool Is_Start = false)
    {
        if (!Is_Start)
        {
            if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
                textFullscreen.text = "ปิด";
                Fullscreen.isOn = false;
            }
            else
            {

                Screen.SetResolution(Screen.resolutions[Screen.resolutions.Length - 1].width, Screen.resolutions[Screen.resolutions.Length - 1].height, FullScreenMode.FullScreenWindow);
                textFullscreen.text = "เปิด";
                Fullscreen.isOn = true;
            }
        }

        if(Input.touchSupported && Application.platform != RuntimePlatform.Android)
        {
            Auto_JoyStick_Enable.isOn = Game_Setting.Is_Auto_JoyStick_Enable;
            if (Game_Setting.Is_Auto_JoyStick_Enable)
            {
                textAuto_JoyStick.text = "เปิด";
            }
            else
            {
                textAuto_JoyStick.text = "ปิด";
            }
        }
    }

    public void Update_Setting_Slider()
    {
        MusicMixer.GetFloat("MusicVol", out float musicoutput);
        Music.value = musicoutput;
        SFXMixer.GetFloat("SFXVol", out float SFXoutput);
        SFX.value = SFXoutput;
    }

    public void Set_Music()
    {
        float value = Music.value;
        print(value);
        MusicMixer.SetFloat("MusicVol", value);
        textMusic.text = (int)(((value + 80) / 80) * 100) +"%";
    }

    public void Set_SFX()
    {
        float value = SFX.value;
        print(value);
        SFXMixer.SetFloat("SFXVol", value);
        textSFX.text = (int)(((value + 80) / 80) * 100) + "%";
    }

    public void Set_Fullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Update_Setting_Toggle();
    }

    public void Set_Auto_JoyStick_Enable()
    {
        Game_Setting.Is_Auto_JoyStick_Enable = !Game_Setting.Is_Auto_JoyStick_Enable;
        Update_Setting_Toggle();
    }
}
