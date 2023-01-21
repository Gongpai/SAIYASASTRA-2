using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialog : MonoBehaviour
{
    [SerializeField] private string pathXML;
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textDialog;
    [SerializeField] private GameObject NextText;
    [SerializeField] private GameObject CanvasObject;
    [SerializeField] public int SceneNum = 1;
    [SerializeField] private float TimeUiFadeOut = 0.5f;
    [SerializeField] private float typingSpeed = 0.04f;

    private int DialogPage = 1;
    private string dialog;
    Coroutine dialogCoroutine = null;
    private bool withEffect = false;

    public void DialogControlNext(InputAction.CallbackContext context)
    {
        if (context.action.triggered && withEffect == true)
        {
            if (dialogCoroutine != null)
                StopCoroutine(dialogCoroutine);

            if (DialogPage >= Dialog_Manager.NumAllDialog(SceneNum, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")))
            {
                Typing_Text(false, "><");

                if (DialogPage >= Dialog_Manager.NumAllDialog(SceneNum, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")) + 1)
                {
                    dialog = "กดอีกรอบเพื่อออกจากบทสนทนานี้";
                }
                else
                {
                    dialog = Dialog_Manager.Dialog_Text(SceneNum, DialogPage, SelectDialog.dialog, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text"));
                }   
            }  
            else
            {
                dialog = Dialog_Manager.Dialog_Text(SceneNum, DialogPage, SelectDialog.dialog, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text"));
                Typing_Text(false, ">");
                    
            }

            withEffect = false;
            print("DDDDDDDDDD");
        }
        else
        {
            if (context.action.triggered && withEffect == false)
            {
                if (DialogPage < Dialog_Manager.NumAllDialog(SceneNum, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")) + 2)
                    DialogPage++;
                
                if (DialogPage >= Dialog_Manager.NumAllDialog(SceneNum, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")) + 1)
                {
                    textName.SetText("ต้องการออกจากบทสนทนานี้หรือไม่ ?");
                    if (DialogPage == Dialog_Manager.NumAllDialog(SceneNum, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")) + 1)
                        TypingEffect("กดอีกรอบเพื่อออกจากบทสนทนานี้");
                }
                else
                {

                    Typing_Text(true, ">");
                    GetDialog(SceneNum, DialogPage);
                    print(DialogPage);
                }

                if (DialogPage >= Dialog_Manager.NumAllDialog(SceneNum, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")) && DialogPage < Dialog_Manager.NumAllDialog(SceneNum, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")) + 2)
                {
                    Typing_Text(true, ">");
                }
            }
        }

        if (DialogPage == Dialog_Manager.NumAllDialog(SceneNum, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")))
        {
            NextText.GetComponent<Animator>().SetBool("IsStop?", true);
        }

        if (DialogPage == Dialog_Manager.NumAllDialog(SceneNum, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")) + 2)
        {
            Game_State_Manager.Instance.Setstate(GameState.Play);
            FuntionLibraly.DestroyWidget(this.gameObject, CanvasObject, TimeUiFadeOut);
        }
    }

    public void DialogControlBack(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            if (DialogPage > 1)
                DialogPage--;

            GetDialog(SceneNum, DialogPage);
            print(DialogPage);

            Typing_Text(true, ">");

            if (DialogPage != Dialog_Manager.NumAllDialog(SceneNum, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")))
            {
                NextText.GetComponent<Animator>().SetBool("IsStop?", false);
            }
        }
    }

    void Start()
    {
        GetDialog(SceneNum, 1);
        
    }

    void Update()
    {
        textDialog.SetText(dialog);
    }

    void GetDialog(int SceneNum, int LineNum)
    {
        textName.SetText(Dialog_Manager.Dialog_Text(SceneNum, LineNum, SelectDialog.name, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")));
        TypingEffect(Dialog_Manager.Dialog_Text(SceneNum, LineNum, SelectDialog.dialog, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")));
    }
    private void TypingEffect(string dialog)
    {
        if (dialogCoroutine != null)
            StopCoroutine(dialogCoroutine);
        dialogCoroutine = StartCoroutine(DisplayLine(dialog));
    }

    private IEnumerator DisplayLine(string line)
    {
        dialog = "";
        withEffect = true;
        foreach (char letter in line.ToCharArray())
        {
            dialog += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        withEffect = false;

        if (DialogPage >= Dialog_Manager.NumAllDialog(SceneNum, pathXML, new Structs_Libraly.XML_Data("Dialog", "Scene", "Line", "name", "text")))
        {
            Typing_Text(false, "><");
        }
        else
        {
            Typing_Text(false, ">");
        }
    }

    private void Typing_Text(bool IsPlay, string nextText)
    {
        NextText.gameObject.GetComponent<Animator>().SetBool("IsPlayTyping", IsPlay);
        NextText.GetComponent<TextMeshProUGUI>().SetText(nextText);

    }
}
