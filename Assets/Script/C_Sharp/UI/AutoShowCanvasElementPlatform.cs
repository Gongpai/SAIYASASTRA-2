using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShowCanvasElementPlatform : MonoBehaviour
{
    [SerializeField] private RuntimePlatform _platform;
    private void OnEnable()
    {
        gameObject.SetActive(_platform == Application.platform || Application.platform == RuntimePlatform.WindowsEditor);
    }
}
