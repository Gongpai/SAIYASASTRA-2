using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class Note_System : MonoBehaviour
{
    [SerializeField] private GameObject Note;
    [SerializeField] private GameObject Note_Element;

    private GameObject gameInstance;
    private List<GameObject> Note_Element_list = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        gameInstance = GameObject.FindGameObjectWithTag("GameInstance").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set_Note_Element()
    {
        GameObject Note_Grid_Item = Note.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        foreach (GameObject note_obj in Note_Element_list)
        {
            Destroy(note_obj.gameObject);
        }

        Note_Element_list.Clear();

        foreach (Structs_Libraly.Note_Data note in GameInstance.noteData)
        {
            GameObject note_element = Note_Element;
            GameObject note_element_list;

            note_element.GetComponent<Note_List>().noteData = note;

            note_element_list = Instantiate(note_element, Note_Grid_Item.transform);

            Note_Element_list.Add(note_element_list);
        }
    }

    public void Add_Note_Element(Structs_Libraly.Note_Data noteData)
    {
        GameInstance.noteData.Add(noteData);
    }
}
