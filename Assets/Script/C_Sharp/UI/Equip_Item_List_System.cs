using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Equip_Item_List_System : MonoBehaviour
{
    [SerializeField] public Structs_Libraly.Item_Data itemData;
    private TextMeshProUGUI TextNumber;

    public int IndexEquip = 0;

    // Start is called before the first frame update
    void Start()
    {
        //print("------ 11 ----- Name : " + gameObject);
        TextNumber = gameObject.transform.GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        TextNumber.SetText(GameInstance.inventoryData[IndexEquip].Number.ToString());
    }

    public void Touch_Select_Item()
    {
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor || SlateModeDetect.currentMode == ConvertibleMode.SlateTabletMode)
            GameInstance.Player.GetComponent<Inventory_System>().Select_Number_List(0, true, GameInstance.inventoryData[IndexEquip].Index);
    }
}
