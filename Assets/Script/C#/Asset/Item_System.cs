using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Item_System : MonoBehaviour
{
    private GameObject Player;
    private void Start()
    {
        Player = gameObject;
    }

    public void Use_Item(Use_Item_System useItem_Mode, GameObject Item, bool IsAim = false, Quaternion rotation = default)
    {
        switch (useItem_Mode)
        {
            case Use_Item_System.Use_Self:
                UseSelf();
                break;
            case Use_Item_System.Use_Other:
                UseOther();
                break;
            case Use_Item_System.Shoot_Projectile:
                if(IsAim == true)
                    ShootProjectile(Item, rotation);
                break;
        }
    }

    private void UseSelf()
    {

    }

    private void UseOther()
    {

    }

    private void ShootProjectile(GameObject Item, Quaternion rotation)
    {
        Rigidbody rigidbody;

        if (Item != null)
        {
            GameObject spawnItem;
            spawnItem = Item;
            spawnItem.GetComponent<Add_item_to_character>().IsSpawn = true;
            spawnItem.transform.rotation = rotation;
            spawnItem.transform.position = transform.position + (spawnItem.transform.up * 1.2f);
            print("Rota : " + rotation);

            GameObject spawn = Instantiate(spawnItem);

            rigidbody = spawn.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(spawn.transform.up * 7, ForceMode.Impulse);
        }
    }
}
