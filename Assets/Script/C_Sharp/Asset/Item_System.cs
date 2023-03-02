using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Item_System : MonoBehaviour
{
    [SerializeField]public GameObject EquipSlot;
    private GameObject Player;
    private GameObject TTT;
    public GameObject ItemEquip;
    private void Start()
    {
        Player = gameObject;
    }

    private void Update()
    {
        /**
        if(TTT != null) 
            print("Holy T : " + TTT.GetComponent<Rigidbody>().velocity.y);
        **/
    }

    [System.Obsolete]
    public void Use_Item(Use_Item_System useItem_Mode, GameObject Item, bool IsAim = false, Quaternion rotation = default)
    {
        switch (useItem_Mode)
        {
            case Use_Item_System.Use_Self:
                break;
            case Use_Item_System.Use_Other:
                Equip_To_Character(Item);
                break;
            case Use_Item_System.Shoot_Projectile:
                if(IsAim == true)
                    ShootProjectile(Item, rotation);
                break;
            case Use_Item_System.Use_Light:
                Equip_To_Character(Item);
                break;
        }
    }

    [System.Obsolete]
    private void Equip_To_Character(GameObject ObjectEquip)
    {
        if (ObjectEquip.GetComponent<Variables>() != null)
        {
            if (EquipSlot.transform.GetChildCount() == 0)
            {
                ItemEquip = Instantiate(ObjectEquip, EquipSlot.transform);
                ItemEquip.GetComponent<Variables>().declarations.Set("Prefab_Original", ObjectEquip);
            }
            else
            {
                if (ItemEquip.GetComponent<Variables>().declarations.Get<GameObject>("Prefab_Original") == ObjectEquip)
                {
                    Destroy(ItemEquip);
                }
            }
        }
    }

    private void ShootProjectile(GameObject Item, Quaternion rotation)
    {
        Rigidbody rigidbody;

        if (Item != null)
        {
            GameObject spawnItem;
            spawnItem = Item;
            spawnItem.transform.rotation = rotation;
            spawnItem.transform.position = transform.position + (spawnItem.transform.up * 1.2f);
            print("Rota : " + rotation);

            GameObject spawn = Instantiate(spawnItem);
            spawn.GetComponent<Item_Attack_System>().ghost = gameObject;
            TTT = spawn;
            rigidbody = spawn.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            rigidbody.AddForce(spawn.transform.up * 7, ForceMode.Impulse);
        }
    }
}
