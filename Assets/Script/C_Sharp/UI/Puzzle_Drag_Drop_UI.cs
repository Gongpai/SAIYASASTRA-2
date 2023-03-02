using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Puzzle_Drag_Drop_UI : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas m_Canvas;
    [SerializeField] public int Index;

    private RectTransform m_RectTransform;
    private Vector2 Default_transform;
    private CanvasGroup m_CanvasGroup;

    public bool CanDrag = true;

    // Start is called before the first frame update
    void Start()
    {
        Default_transform = GetComponent<RectTransform>().anchoredPosition;
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    public void Set_Enable_Puzzle_Element()
    {
        GetComponent<Image>().raycastTarget = true;
        GetComponent<Image>().color = new Color(255, 255, 255, 255);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        if (CanDrag)
        {
            transform.SetAsLastSibling();
            animator.SetBool("IsPlayIn", true);
            animator.SetBool("IsPlayOut", false);
        }
    }
    public void OnPointerUp(PointerEventData eventData) 
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData) 
    {
        if (CanDrag)
        {
            m_CanvasGroup.blocksRaycasts = false;
        }
    }
    public void OnEndDrag(PointerEventData eventData) 
    {
        if (CanDrag)
        {
            m_CanvasGroup.blocksRaycasts = true;
            animator.SetBool("IsPlayIn", false);
            animator.SetBool("IsPlayOut", true);
        }
        if(m_RectTransform.anchoredPosition.x <= 5 || m_RectTransform.anchoredPosition.y <= 0 || m_RectTransform.anchoredPosition.x > 600 || m_RectTransform.anchoredPosition.y > 380)
        {
            m_RectTransform.anchoredPosition = Default_transform;
        }
    }
    public void OnDrag(PointerEventData eventData) 
    {
        if (CanDrag)
        {
            m_RectTransform.anchoredPosition += (eventData.delta / m_Canvas.scaleFactor);
            print(m_RectTransform.anchoredPosition);
        }
    }
    public void OnDrop(PointerEventData eventData) 
    {

    }
    public void OnPointerClick(PointerEventData eventData) 
    {
        
    }
}
