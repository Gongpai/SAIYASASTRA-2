using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Item_List_Data : MonoBehaviour
{
    [SerializeField] public List<Structs_Libraly.Item_Data> itemDatas;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
