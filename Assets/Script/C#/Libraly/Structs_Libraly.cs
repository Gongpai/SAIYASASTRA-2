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
        public GameObject Owner;

        public Item_Data(string name, int number, Sprite sprite, bool isequip, int index, GameObject owner)
        {
            this.Name = name;
            this.Number = number;
            this.itemSprite = sprite;
            this.IsEquip = isequip;
            this.Index = index;
            this.Owner = owner;
        }
    }
}
