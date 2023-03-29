using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Note_List : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI text_note;
    [SerializeField] private Image Bg_image;
    [SerializeField] private Image Fg_image;

    public Structs_Libraly.Note_Data noteData;

    public int index;
    // Start is called before the first frame update
    void Start()
    {
        if (noteData.sprite_note == null)
        {
            title.text = Dialog_Manager.Dialog_Text(default, default, SelectDialog.note_title, "Dialog/NoteText", noteData.Title);
            text_note.text = Dialog_Manager.Dialog_Text(default, default, SelectDialog.note_text, "Dialog/NoteText", noteData.Text);
        }
        else
        {
            Fg_image.color = new Color(255, 255, 255, 0);
            title.text = "";
            text_note.text = "";
            Bg_image.sprite = noteData.sprite_note;
        }
    }


    public void Show_Note()
    {
        GameInstance.Player.GetComponent<Note_System>().Set_Note_Show_All(index);
    }
}
