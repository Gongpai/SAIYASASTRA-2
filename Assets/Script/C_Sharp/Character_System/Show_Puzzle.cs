using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show_Puzzle : MonoBehaviour
{
    [SerializeField] private string PickUpMessage = "[E] เก็บบันทึก";
    [SerializeField] public GameObject puzzle_ui;
    [SerializeField] GameObject DoorUnlock;

    private Structs_Libraly.Note_Data NoteData;
    private ShowMessage pLayer;
    private bool CharacterEnter = false;
    public bool Can_Open_Puzzle = true;
    private GameObject Gameinstance;
    GameObject puzzleSpawn;
    // Start is called before the first frame update
    void Start()
    {
        Gameinstance = GameObject.FindGameObjectWithTag("GameInstance").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPuzzle_Ui()
    {
        if (CharacterEnter && Can_Open_Puzzle)
        {
            Game_State_Manager.Instance.Setstate(GameState.Pause);
            puzzleSpawn = Instantiate(puzzle_ui, GameObject.FindGameObjectWithTag("Game_Ui").transform);
            puzzleSpawn.GetComponent<Puzzle_System>().DoorUnlock = DoorUnlock;

            if (GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect != null)
                GameInstance.Player.GetComponent<Player_Movement>().Ghost_Effect.SetActive(false);

            puzzleSpawn.GetComponent<Puzzle_System>().ShowPuzzle = this;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && Can_Open_Puzzle)
        {
            CharacterEnter = true;
            collider.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Show_Message(PickUpMessage);
            pLayer = collider.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>();
            //print("DDDDDDD");
            collider.GetComponent<Player_Movement>().Set_Block_Use_item(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            CharacterEnter = false;
            collider.gameObject.GetComponent<Player_Movement>().showMessage.GetComponent<ShowMessage>().Hide_Message();
            collider.GetComponent<Player_Movement>().Set_Block_Use_item(false);
        }
    }
}
