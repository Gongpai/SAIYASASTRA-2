using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventoly_System : MonoBehaviour
{
    [SerializeField] private GameObject List_Item;
    [SerializeField] private List<GameObject> Element;
    [SerializeField] private GameObject gameInstance;
    [SerializeField] private GameObject List_Grid;
    [SerializeField] private GameObject List_Grid_Element;

    private InputManager inputManager;
    private int SelectNum  = 0, maxSelect, Old_Select = 0;
    private List<Structs_Libraly.Item_Data> inventoryData = new List<Structs_Libraly.Item_Data>();

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

        inventoryData.Add(gameInstance.GetComponent<Item_List_Data>().itemDatas[0]);
        inventoryData.Add(gameInstance.GetComponent<Item_List_Data>().itemDatas[1]);
        inventoryData.Add(gameInstance.GetComponent<Item_List_Data>().itemDatas[2]);
        inventoryData.Add(gameInstance.GetComponent<Item_List_Data>().itemDatas[2]);

        Set_Item_Element();

        Get_Item_Element();
    }

    // Update is called once per frame
    void Update()
    {
        Select_Item();
    }

    void Awake()
    {
        Game_State_Manager.Instance.OnGameStateChange += OnGamestateChanged;
    }

    void OnDestroy()
    {
        Game_State_Manager.Instance.OnGameStateChange -= OnGamestateChanged;
    }

    private void OnGamestateChanged(GameState gameState)
    {
        enabled = gameState == GameState.Play;
    }

    //ระบบเลื่อนเมาส์กลางขึ้น-ลง
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

    //ระบบเซตไอเทม
    private void Set_Item_Element()
    {
        foreach (Structs_Libraly.Item_Data Item in inventoryData)
        {
            GameObject element_Item = List_Grid_Element;
            element_Item.transform.GetChild(3).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(Item.Name);
            element_Item.transform.GetChild(4).GetComponent<Image>().sprite = Item.itemSprite;
            element_Item.transform.GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(Item.Number.ToString());
            Instantiate(element_Item, List_Grid.transform);
        }
    }

    //ระบบดูว่า item list มี element กี่อัน
    private void Get_Item_Element()
    {
        print(List_Item.transform.GetChild(0).GetChild(0).childCount);

        maxSelect = List_Item.transform.GetChild(0).GetChild(0).childCount - 1;
        print("Max Select : " + maxSelect);

        for (int i = 0; i <= List_Item.transform.GetChild(0).GetChild(0).childCount - 1; i++)
        {
            Element.Add(List_Item.transform.GetChild(0).GetChild(0).GetChild(i).gameObject);

            print(List_Item.transform.GetChild(0).GetChild(0).GetChild(i).name);
        }

        Element.ElementAt(0).GetComponent<Animator>().SetBool("Is_Play?", true);
    }

    //ระบบเลือกไอเทม
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
