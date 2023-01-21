using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Note_List : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI text_note;

    public Structs_Libraly.Note_Data noteData;
    // Start is called before the first frame update
    void Start()
    {
        title.text = Dialog_Manager.Dialog_Text(default, default, SelectDialog.note_title, "Dialog/NoteText", noteData.Title);
        text_note.text = Dialog_Manager.Dialog_Text(default, default, SelectDialog.note_text, "Dialog/NoteText", noteData.Text);
    }

    public void Show_Note()
    {
        print("Showwww Noteeeeeeeeeeeeeeeeeeeeeeee");
    }
}
