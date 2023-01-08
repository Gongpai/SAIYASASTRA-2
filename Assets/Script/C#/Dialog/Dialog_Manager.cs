using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using Mono.Cecil.Cil;
using UnityEditor;
using UnityEngine;

public class Dialog_Manager : MonoBehaviour
{
    public static string Dialog_Text(int SceneNum ,int LineNum, SelectDialog selectDialog, string pathxml)
    {
        print(pathxml);
        TextAsset textAsset = Resources.Load<TextAsset>(pathxml);
        var Xdoc = XDocument.Parse(textAsset.text);
        var DialogOutput = Xdoc.Element("Dialog");

        switch (selectDialog)
        {
            case SelectDialog.name:
                DialogOutput = Xdoc.Element("Dialog").Element("Scene" + SceneNum).Element("Line" + LineNum).Element("name");
                break;

            case SelectDialog.dialog:
                DialogOutput = Xdoc.Element("Dialog").Element("Scene" + SceneNum).Element("Line" + LineNum).Element("text");
                break;
            default:
                print("null");
                break;
        }
        
        return DialogOutput.Value;
    }

    public static int NumAllDialog(int SceneNum, string pathxml)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(pathxml);
        var Xdoc = XDocument.Parse(textAsset.text);
        var AllDialog = Xdoc.Element("Dialog").Elements("Scene" + SceneNum);
        string[] arrayDialog = AllDialog.ElementAt(0).ToString().Split("<name>");
        List<String> dialog = new List<string>();

        foreach (string dialogText in arrayDialog)
        {
            dialog.Add(dialogText);
        }

        dialog.RemoveAt(0);
        return dialog.Count;
    }
}
