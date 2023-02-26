using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character_Hide : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject Character_Detact;
    private Cupboard_Hide cupboardHide;
    void Start()
    {
        cupboardHide = Character_Detact.GetComponent<Cupboard_Hide>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
            cupboardHide.CharacterInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
            cupboardHide.CharacterInside = false;
    }
}
