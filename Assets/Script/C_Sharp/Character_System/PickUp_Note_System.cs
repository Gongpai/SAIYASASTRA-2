using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Structs_Libraly;

public class PickUp_Note_System : MonoBehaviour
{
    [SerializeField] private string PickUpMessage = "[E] เก็บบันทึก";
    [SerializeField] private int NoteIndex = 0;

    private Structs_Libraly.Note_Data NoteData;
    private ShowMessage pLayer;
    private bool CharacterEnter = false;
    private GameObject Gameinstance;
    // Start is called before the first frame update
    void Start()
    {
        Gameinstance = GameObject.FindGameObjectWithTag("GameInstance").gameObject;
        NoteData = Gameinstance.GetComponent<Item_List_Data>().noteDatas[NoteIndex];
    }

    public void PickUp_Note()
    {
        if (CharacterEnter)
        {
            GameInstance.Player.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Hide_Message();
            GameInstance.Player.GetComponent<Player_Movement>().Set_Block_Use_item(false);
            GameInstance.Player.GetComponent<Note_System>().Add_Note_Element(NoteData);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            CharacterEnter = true;
            collider.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message(PickUpMessage);
            pLayer = collider.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>();
            //print("DDDDDDD");
            collider.GetComponent<Player_Movement>().Set_Block_Use_item(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            CharacterEnter = false;
            collider.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Hide_Message();
            collider.GetComponent<Player_Movement>().Set_Block_Use_item(false);
        }
    }
}
