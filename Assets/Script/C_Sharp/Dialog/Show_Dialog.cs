using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Show_Dialog : MonoBehaviour
{
    [SerializeField]
    private GameObject DialogWidget;

    [SerializeField] private int SceneNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            DialogWidget.GetComponent<Dialog>().SceneNum = SceneNum;
            Instantiate(DialogWidget);
            Game_State_Manager.Instance.Setstate(GameState.Pause);
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
