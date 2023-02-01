using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs_Libraly;

public class Make_Structs : MonoBehaviour
{
    public static XML_Data makeXmlData(string elementFirst = "Dialog", string elementSecond = "Scene", string elementThird = "Line", string supElementFirst = "name", string supElementSecond = "text")
    {
        XML_Data data;
        data = new XML_Data
        (
            elementFirst,
            elementSecond,
            elementThird,
            supElementFirst,
            supElementSecond
        );
        return data;
    }

    public static Item_Data makeItemData(int i_index = 0, string name = "null", int number = 0, Sprite sprite = default, bool isequip = false, int m_index = 0, GameObject ItemPrefeb = default, Use_Item_System useItemMode = default)
    {
        Item_Data data;
        data = new Item_Data
        (
            i_index,
            name,
            number,
            sprite,
            isequip,
            m_index,
            ItemPrefeb,
            useItemMode
        );

        return data;
;    }
}
