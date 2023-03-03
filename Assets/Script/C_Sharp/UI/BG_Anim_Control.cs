using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Anim_Control : MonoBehaviour
{
    [SerializeField] GameObject BG;
    bool Is_Hide_BG = false;

    public void SetHide(bool hide)
    {
        Is_Hide_BG = hide;
    }

    public void OnHide()
    {
        if (Is_Hide_BG)
        {
            BG.SetActive(false);
            Is_Hide_BG = false;
        } else
        {
            
        }
    }
}
