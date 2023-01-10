using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Use_Item_System : MonoBehaviour
{
    [SerializeField] private GameObject List_Item;
    [SerializeField] private List<GameObject> Element;

    private InputManager inputManager;
    private int SelectNum  = 0, maxSelect, Old_Select = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (inputManager == null)
        {
            inputManager = InputManager.instance;
        }

        if (inputManager == null)
        {
            Debug.LogWarning("No player");
        }

        print(List_Item.transform.GetChild(0).GetChild(0).childCount);

        maxSelect = List_Item.transform.GetChild(0).GetChild(0).childCount - 1;
        print("Max Select : " + maxSelect);

        for (int  i = 0;  i <= List_Item.transform.GetChild(0).GetChild(0).childCount - 1;  i++)
        {
            Element.Add(List_Item.transform.GetChild(0).GetChild(0).GetChild(i).gameObject);

            print(List_Item.transform.GetChild(0).GetChild(0).GetChild(i).name);
        }

        Element.ElementAt(0).GetComponent<Animator>().SetBool("Is_Play?", true);
    }

    // Update is called once per frame
    void Update()
    {
        Select_Item();
    }

    public void Select_Item()
    {
        if (inputManager.AxisScroll > 0)
        {
            Select_Number_List(-1);

        }
        else if (inputManager.AxisScroll < 0)
        {
            Select_Number_List(1);
        }
    }

    void Select_Number_List(int Number)
    {
        SelectNum += Number;
        if (SelectNum > maxSelect)
        {
            SelectNum = 0;
        }
        else if (SelectNum < 0)
        {
            SelectNum = maxSelect;
        }


        print("Select is : " + SelectNum);
        Element.ElementAt(SelectNum).GetComponent<Animator>().SetBool("Is_Play?", true);

        if (Old_Select != SelectNum)
        {
            Element.ElementAt(Old_Select).GetComponent<Animator>().SetBool("Is_Play?", false);
            print("Old Select is : " + SelectNum);
        }

        Old_Select = SelectNum;
    }
}
