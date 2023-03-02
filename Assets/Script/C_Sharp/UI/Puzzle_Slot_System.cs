using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Puzzle_Slot_System : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField]public int index;

    public static List<GameObject> puzzle = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject puzzle_drop = eventData.pointerDrag;
        if(puzzle_drop.GetComponent<Puzzle_Drag_Drop_UI>().Index == index)
        {
            puzzle_drop.GetComponent<Puzzle_Drag_Drop_UI>().CanDrag = false;
            puzzle.Add(puzzle_drop);
            puzzle_drop.transform.position = transform.GetChild(0).transform.position;

        }
        print("Slot_OnDrop " + puzzle.Count);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //print("Slot_OnDrop " + eventData.pointerDrag);
    }

    public void OnEnter()
    {
        transform.SetAsLastSibling();
    }

    public void OnExit()
    {
        transform.SetAsFirstSibling();
    }
}
