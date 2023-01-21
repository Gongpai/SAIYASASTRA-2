using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Structs_Libraly;

public class Add_Note_to_character : MonoBehaviour
{
    [SerializeField] private int NoteIndex = 0;

    private Structs_Libraly.Note_Data NoteData;

    private GameObject Gameinstance;
    // Start is called before the first frame update
    void Start()
    {
        Gameinstance = GameObject.FindGameObjectWithTag("GameInstance").gameObject;
        NoteData = Gameinstance.GetComponent<Item_List_Data>().noteDatas[NoteIndex];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Note_System>().Add_Note_Element(NoteData);
            Destroy(this.gameObject);
        }
    }
}
