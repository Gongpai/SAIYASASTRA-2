using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ai_Attack : MonoBehaviour
{
    [SerializeField] private GameObject ObjectAttack;

    private Rigidbody testtt;

    void Update()
    {
        //if (testtt != null)
            //print("Speed : " + Vector3.Magnitude(testtt.velocity));
    }

    public void Attack(AiGhost selectAiGhost)
    {
        switch (selectAiGhost)
        {
            case AiGhost.Hungry_ghost:
                Shoot_projectile();
                break;
            case AiGhost.Home_ghost:
                Shoot_horizontal(GetComponent<SpriteRenderer>().flipX);
                break;
            case AiGhost.Mannequin_ghost:
                PlayAnimCombat(true);
                break;
            default:
                break;
        }
    }

    public void PlayAnimCombat(bool IsPlay)
    {
        GetComponent<Animator>().SetBool("IsAttack", IsPlay);
    }

    public void Attack_Combat()
    {
        GameInstance.Player.GetComponent<Player_Movement>().HP -= GetComponent<Ai_Movement>().Damage;
    }

    public void Shoot_projectile(GameObject _gameObject = default, bool is_spawn = true, Vector3 velocity = default)
    {
        Rigidbody rigidbody;
        Quaternion Rot = Quaternion.Euler(0, 0, FuntionLibraly.Get2DLookAt(gameObject.transform.position, GameInstance.Player.GetComponent<Player_Movement>().HeadPoint.transform.position) - CulMath() + 55);
        

        if (ObjectAttack != null)
        {
            GameObject spawnItem;
            spawnItem = ObjectAttack;
            spawnItem.GetComponent<Add_item_to_character>().IsSpawn = true;
            spawnItem.transform.rotation = Rot;
            spawnItem.transform.position = transform.position + (spawnItem.transform.up * 1.1f);
            print("Rota : " + Rot);

            GameObject spawn = new GameObject();
            if (is_spawn)
            {
                spawn = Instantiate(spawnItem);
                spawn.GetComponent<Add_item_to_character>().ai_ghost = GetComponent<Variables>().declarations.Get<AiGhost>("Ai_Ghost");
                spawn.GetComponent<Add_item_to_character>().ghost = gameObject;
                rigidbody = spawn.GetComponent<Rigidbody>();
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                rigidbody.AddForce(spawn.transform.up * (7 + Vector3.Magnitude(GetComponent<Rigidbody>().velocity)), ForceMode.Impulse);
                testtt = rigidbody;
            }
            else
            {
                spawn = _gameObject;
                rigidbody = spawn.GetComponent<Rigidbody>();
                rigidbody.AddForce(velocity, ForceMode.Impulse);
            }
        }
    }

    public void Shoot_horizontal(bool flip, GameObject _gameObject = default, bool is_spawn = true)
    {
        Rigidbody rigidbody;

        if(ObjectAttack != null)
        {
            GameObject spawnItem;
            spawnItem = ObjectAttack;
            spawnItem.GetComponent<Add_item_to_character>().IsSpawn = true;
            spawnItem.transform.rotation = Quaternion.Euler(0,0,0);
            if (GetComponent<SpriteRenderer>().flipX)
            {
                spawnItem.transform.position = transform.position + (spawnItem.transform.right * -1.1f);
            }
            else
            {
                spawnItem.transform.position = transform.position + (spawnItem.transform.right * 1.1f);
            }

            GameObject spawn = new GameObject();
            if (is_spawn)
            {
                spawn = Instantiate(spawnItem);
                spawn.GetComponent<Add_item_to_character>().ai_ghost = GetComponent<Variables>().declarations.Get<AiGhost>("Ai_Ghost");
                spawn.GetComponent<Add_item_to_character>().ghost = gameObject;
                spawn.GetComponent<Add_item_to_character>().isFlip = flip;
            }
            else
            {
                spawn = _gameObject;
            }

            rigidbody = spawn.GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = false;

            if (flip)
            {
                rigidbody.AddForce(transform.right * -5, ForceMode.Impulse);
            } 
            else
            {
                rigidbody.AddForce(transform.right * 5, ForceMode.Impulse);
            }
        }
    }

    private float CulMath()
    {
        float x = Vector3.Distance(gameObject.transform.position, GameInstance.Player.transform.position);
        float v = 4.65f + (Vector3.Magnitude(GetComponent<Rigidbody>().velocity) * 2);
        float g = Physics.gravity.y;
        float gx = g * x;
        float ma = 2 * Mathf.Pow(Mathf.Sin(gx / Mathf.Pow(v, 2)), -1);
        float RTD = ma * Mathf.Rad2Deg;
        float Final = RTD - Mathf.CeilToInt(RTD / 360f) * 360f;

        /**
        Quaternion a = Quaternion.Euler(0, 0, RTD);
        Quaternion Final = Quaternion.Normalize(a);
        **/

        if (GetComponent<SpriteRenderer>().flipX)
        {
            if (Final > 0)
                Final = Final * -1;

            Final -= 75;
        }
        else
        {
            if (Final < 0)
                Final = Final * -1;
        }

        print("Angle : " + Final);

        return Final;
    }
}
