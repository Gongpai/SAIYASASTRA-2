using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Script : MonoBehaviour
{
    [SerializeField]public GameObject Destory_Effect;
    [SerializeField] AudioClip audioClip;
    [SerializeField] Color color;

    public void Destroy_Item()
    {
        //Playanim
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        Destory_Effect.transform.position = transform.position;
        Destory_Effect.GetComponent<Bom_Effect_Animation>().SetSound_Bom(audioClip);
        GameObject Effect = Instantiate(Destory_Effect);
        Effect.GetComponent<SpriteRenderer>().color = color;
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(gameObject.GetComponent<Item_Attack_System>() !=  null && !other.isTrigger && other.tag == "Untagged")
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            Destory_Effect.transform.position = transform.position;
            Destory_Effect.GetComponent<Bom_Effect_Animation>().SetSound_Bom(audioClip);
            GameObject Effect = Instantiate(Destory_Effect);
            Effect.GetComponent<SpriteRenderer>().color = color;
            Destroy(gameObject);
        }
    }
}
