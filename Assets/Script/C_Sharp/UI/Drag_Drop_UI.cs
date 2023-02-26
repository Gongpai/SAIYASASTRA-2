using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Drag_Drop_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerClickHandler
{
    private RectTransform m_RectTransform;
    private Canvas m_Canvas;
    private CanvasGroup m_CanvasGroup;

    public bool IsDrag = false;
    public bool IsDrop = false;
    public int old_number = 0;

    private bool IsExit = false;
    private bool IsPointerEnter = false;

    public GameObject Item_craft_list;
    public GameObject CraftSlot = null;
    
    public Structs_Libraly.Item_Data itemData;
    // Start is called before the first frame update
    void Start()
    {
        m_Canvas = GameObject.FindGameObjectWithTag("Essential_Menu").GetComponent<Canvas>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsExit && !IsPointerEnter)
        {
            Destroy(this.gameObject);
        }
    }

    void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    public void Exit()
    {
        IsExit = true;
    }
    public void OnDrop(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
        IsDrag = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("OnPointerUp");
        IsDrag = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
        IsPointerEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (CraftSlot != null)
                CraftSlot.GetComponent<Craft_ItemSlot>().Remove_item();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        m_RectTransform.anchoredPosition += (eventData.delta / m_Canvas.scaleFactor);
        IsDrag = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        m_CanvasGroup.blocksRaycasts = false;
        IsDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        print("Drop----------------------------");
        print("Name : " + eventData.pointerEnter.gameObject.name + " Tag : " + eventData.pointerEnter.tag);


        if (eventData.pointerEnter.tag != "Craft_Slot")
        {
            //ถ้าย้ายไอเทมออกไปที่ว่างๆ ที่ไม่ใช่ช่องคราฟให้ย้ายไอเทมกลับไปที่เดิม
            if (CraftSlot != null)
            {
                gameObject.transform.position = CraftSlot.transform.GetChild(0).position;
                print("When move to exit item back to slot");
            }
            //ถ้าย้ายไอเทมจากช่องเก็บของไปวางที่ไม่ใช่ข่องคราฟก็จะ destroy ไอเทมนั้นไป
            else
            {
                Destroy(this.gameObject);
            }  
        }

        m_CanvasGroup.blocksRaycasts = true;
        IsDrag = false;
    }
}
