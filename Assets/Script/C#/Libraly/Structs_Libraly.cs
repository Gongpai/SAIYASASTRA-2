using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structs_Libraly : MonoBehaviour
{
    [System.Serializable]
    public struct Item_Data
    {
        public string Name;
        public int Number;
        public Sprite itemSprite;
        public bool IsEquip;
        public int Index;
        public GameObject ItemPrefeb;
        public Use_Item_System useItemMode;

        public Item_Data(string name, int number, Sprite sprite, bool isequip, int m_index, GameObject ItemPrefeb = default, Use_Item_System useItemMode = default)
        {
            this.Name = name;
            this.Number = number;
            this.itemSprite = sprite;
            this.IsEquip = isequip;
            this.Index = m_index;
            this.ItemPrefeb = ItemPrefeb;
            this.useItemMode = useItemMode; 
        }
    }
}
