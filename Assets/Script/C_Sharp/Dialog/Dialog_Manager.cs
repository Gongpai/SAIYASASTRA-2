using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class Dialog_Manager : MonoBehaviour
{
    public static string Dialog_Text(int SceneNum = 1 ,int LineNum = 1, SelectDialog selectDialog = default, string pathxml = default, Structs_Libraly.XML_Data xmlData = default)
    {
        //print(xmlData.Element_First);
        TextAsset textAsset = Resources.Load<TextAsset>(pathxml);
        var Xdoc = XDocument.Parse(textAsset.text);
        var DialogOutput = Xdoc.Element(xmlData.Element_First);

        switch (selectDialog)
        {
            case SelectDialog.name:
                DialogOutput = Xdoc.Element(xmlData.Element_First).Element(xmlData.Element_Second + SceneNum).Element(xmlData.Element_Third + LineNum).Element(xmlData.Sup_Element_First);
                break;

            case SelectDialog.dialog:
                DialogOutput = Xdoc.Element(xmlData.Element_First).Element(xmlData.Element_Second + SceneNum).Element(xmlData.Element_Third + LineNum).Element(xmlData.Sup_Element_Second);
                break;
            case SelectDialog.note_title:
                DialogOutput = Xdoc.Element(xmlData.Element_First).Element(xmlData.Element_Second).Element(xmlData.Element_Third).Element(xmlData.Sup_Element_First);
                break;
            case SelectDialog.note_text:
                DialogOutput = Xdoc.Element(xmlData.Element_First).Element(xmlData.Element_Second).Element(xmlData.Element_Third).Element(xmlData.Sup_Element_Second);
                break;
            default:
                print("null");
                break;
        }
        
        return DialogOutput.Value;
    }

    public static int NumAllDialog(int SceneNum, string pathxml, Structs_Libraly.XML_Data xmlData = default)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(pathxml);
        var Xdoc = XDocument.Parse(textAsset.text);
        var AllDialog = Xdoc.Element(xmlData.Element_First).Elements(xmlData.Element_Second + SceneNum);
        string[] arrayDialog = AllDialog.ElementAt(0).ToString().Split("<" + xmlData.Sup_Element_First + ">");
        List<String> dialog = new List<string>();

        foreach (string dialogText in arrayDialog)
        {
            dialog.Add(dialogText);
        }

        dialog.RemoveAt(0);
        return dialog.Count;
    }
}
