using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

public class AssetScreenLocation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject objectSize;


    Vector3 MaxSizeObj;
    Vector3 ObjectPos;

    private bool IsAddHideObject = false;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInstance.CharacterHide)
        {
            MaxSizeObj = objectSize.GetComponent<Renderer>().bounds.max;
            ObjectPos = Camera.main.WorldToScreenPoint(transform.position);

            if ((ObjectPos.x > -MaxSizeObj.x && ObjectPos.x < (Screen.width + MaxSizeObj.x)) && (GameInstance.Ghost.GetComponent<Ai_Movement>().hideGameObject.FindIndex(i => i.gameObject == gameObject) < 0))
            {
                GameInstance.Ghost.GetComponent<Ai_Movement>().hideGameObject.Add(gameObject);
                print(gameObject.name + " Pos in Screen is : " + ObjectPos + " hideGameObject : " + GameInstance.Ghost.GetComponent<Ai_Movement>().hideGameObject.Count);
            }
        }
        else
        {
            if(GameInstance.Ghost.GetComponent<Ai_Movement>().hideGameObject != null)
                GameInstance.Ghost.GetComponent<Ai_Movement>().hideGameObject.Clear();
        }
    }
}
