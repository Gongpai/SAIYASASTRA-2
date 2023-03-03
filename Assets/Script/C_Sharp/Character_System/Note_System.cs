using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Structs_Libraly;

public class Note_System : MonoBehaviour
{
    [SerializeField] private GameObject Note_List;
    [SerializeField] private GameObject Note_Element;
    [SerializeField] private GameObject ShowAllPage;
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private GameObject Exit;

    private GameObject gameInstance;

    private List<GameObject> Note_Element_list = new List<GameObject>();
    public Animator animator;

    private int note_Page = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameInstance = GameObject.FindGameObjectWithTag("GameInstance").gameObject;
        animator = Note_List.transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void PlayAnim(bool IsPlayIn)
    {
        if (IsPlayIn)
        {
            animator.SetBool("IsIn", true);
            animator.SetBool("IsOut", false);
        }
        else
        {
            animator.SetBool("IsIn", false);
            animator.SetBool("IsOut", true);
        }
    }

    public void Set_Note_Element()
    {
        int i = 0;

        GameObject Note_Grid_Item = Note_List.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
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
            note_element.GetComponent<Note_List>().index = i;

            note_element_list = Instantiate(note_element, Note_Grid_Item.transform);

            Note_Element_list.Add(note_element_list);

            i++;
        }
    }

    public void Set_Note_Show_All(int page = 0)
    {
        ShowAllPage.SetActive(true);
        ShowAllPage.transform.GetChild(4).GetComponent<Button_Event>().DontPlayIn();
        Note_List.SetActive(false);
        Exit.SetActive(false);
        Title.text = Dialog_Manager.Dialog_Text(default, default, SelectDialog.note_title, "Dialog/NoteText", GameInstance.noteData[page].Title);
        Text.text = Dialog_Manager.Dialog_Text(default, default, SelectDialog.note_text, "Dialog/NoteText", GameInstance.noteData[page].Text);
        note_Page = page;
    }

    public void Select_Page(int page)
    {
        note_Page += page;
        if (note_Page > GameInstance.noteData.Count - 1)
        {
            note_Page = 0;
        }
        else if (note_Page < 0)
        {
            note_Page = GameInstance.noteData.Count - 1;
        }

        Title.text = Dialog_Manager.Dialog_Text(default, default, SelectDialog.note_title, "Dialog/NoteText", GameInstance.noteData[note_Page].Title);
        Text.text = Dialog_Manager.Dialog_Text(default, default, SelectDialog.note_text, "Dialog/NoteText", GameInstance.noteData[note_Page].Text);
    }

    public void Add_Note_Element(Structs_Libraly.Note_Data noteData)
    {
        GameInstance.noteData.Add(noteData);
    }
}
