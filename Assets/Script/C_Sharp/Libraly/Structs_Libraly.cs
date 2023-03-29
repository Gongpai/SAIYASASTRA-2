using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structs_Libraly : MonoBehaviour
{
    [System.Serializable]
    public struct Item_Data
    {
        public int Item_Index;
        public string Name;
        public int Number;
        public Sprite itemSprite;
        public bool IsEquip;
        public int Index;
        public GameObject ItemPrefeb;
        public Use_Item_System useItemMode;

        public Item_Data(int i_index, string name, int number, Sprite sprite, bool isequip, int m_index, GameObject ItemPrefeb = default, Use_Item_System useItemMode = default)
        {
            this.Item_Index = i_index;
            this.Name = name;
            this.Number = number;
            this.itemSprite = sprite;
            this.IsEquip = isequip;
            this.Index = m_index;
            this.ItemPrefeb = ItemPrefeb;
            this.useItemMode = useItemMode;
        }
    }

    [System.Serializable]
    public struct Note_Data
    {
        public XML_Data Title;
        public XML_Data Text;
        public Sprite sprite_note;

        public Note_Data(XML_Data title, XML_Data text, Sprite images)
        {
            this.Title = title;
            this.Text = text;
            this.sprite_note = images;
        }
    }

    [System.Serializable]
    public struct XML_Data
    {
        public string Element_First;
        public string Element_Second;
        public string Element_Third;
        public string Sup_Element_First;
        public string Sup_Element_Second;

        public XML_Data(string elementFirst = "Dialog", string elementSecond = "Scene", string elementThird = "Line", string supElementFirst = "name", string supElementSecond = "text")
        {
            this.Element_First = elementFirst;
            this.Element_Second = elementSecond;
            this.Element_Third = elementThird;
            this.Sup_Element_First = supElementFirst;
            this.Sup_Element_Second = supElementSecond;
        }
    }
}
