using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Attack_System : FuntionLibraly
{
    [SerializeField] new Collider collider = new Collider();
    [SerializeField] GameObject m_parent_or_child; 

    public GameObject ghost;
    public AiGhost ai_ghost;
    public bool isFlip;
    public float Scale_Item = 1.0f;
    private Vector3 velocity;

    public delegate void PauseGame();
    public static PauseGame OnPauseGame;
    public static PauseGame UnPauseGame;

    // Start is called before the first frame update
    private void Start()
    {
        OnPauseGame += Set_PauseGame;
        UnPauseGame += Set_UnPauseGame;

        m_parent_or_child.GetComponent<SpriteRenderer>().flipX = isFlip;
        transform.localScale = Vector3.one * Scale_Item;
    }

    private void Update()
    {
        if(transform.position.y <= -70)
        {
            Destroy(gameObject);
        }
    }

    public void Set_PauseGame()
    {
        try
        {
            velocity = m_parent_or_child.GetComponent<Rigidbody>().velocity;
            GamePause_Component(gameObject, true);
        }
        catch
        {

        }
    }
    public void Set_UnPauseGame()
    {
        try
        {
            GamePause_Component(gameObject, false);

            switch (ai_ghost)
            {
                case AiGhost.Hungry_ghost:
                    ghost.GetComponent<Ai_Attack>().Shoot_projectile(gameObject, false, velocity);
                    break;
                case AiGhost.Soi_Ju_ghost:
                    ghost.GetComponent<Ai_Attack>().Shoot_horizontal(isFlip, Scale_Item,gameObject, false);
                    break;
                default:
                    break;
            }
        }
        catch
        {

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger && other.tag == "Player" && ghost.tag == "Ghost")
        {
            print("HP------------------------------------- " + other);
            other.GetComponent<Player_Movement>().HP_System(collider, -1);
        }

        if (other.isTrigger && other.tag == "Player" && other.gameObject == ghost && GetComponent<Rigidbody>().velocity.y <= 0)
        {
            Debug.LogWarning("HPP_ADDDD : " + other.tag + "Is : " + (other.gameObject == ghost) + "VVVV : " + GetComponent<Rigidbody>().velocity.y);
            other.GetComponent<Player_Movement>().HP_System(collider, 1);
        }
    }
}
