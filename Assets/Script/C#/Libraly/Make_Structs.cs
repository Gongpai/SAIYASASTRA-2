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
}
