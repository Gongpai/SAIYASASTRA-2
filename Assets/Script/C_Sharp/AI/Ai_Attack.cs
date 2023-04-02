using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ai_Attack : MonoBehaviour
{
    [SerializeField] private GameObject ObjectAttack;
    [SerializeField] GameObject Ghost_Effects;
    [SerializeField] List<Transform> XPoint;
    [SerializeField] List<Transform> YPoint;

    Animator Ghoat_Effects_Animator;
    private Rigidbody testtt;

    void Start()
    {
        if(Ghost_Effects != null && Ghost_Effects.GetComponent<Animator>() != null)
            Ghoat_Effects_Animator = Ghost_Effects.GetComponent<Animator>();
    }

    void Update()
    {
        //if (testtt != null)
            //print("Speed : " + Vector3.Magnitude(testtt.velocity));
    }

    [System.Obsolete]
    public void Attack(AiGhost selectAiGhost)
    {
        switch (selectAiGhost)
        {
            //สัมภเวสี / ผีตายโหง
            case AiGhost.Hungry_ghost:
                Shoot_projectile();
                break;
            //เจ้าที่
            case AiGhost.Home_ghost:
                Random_Postition_Attack();
                print("CanAttackHomeGhost");
                //Shoot_horizontal(GetComponent<SpriteRenderer>().flipX);
                break;
            //ผียาม + พนักงาน 
            case AiGhost.Guard_ghost:
                PlayAnimCombat(true);
                break;
            //กุมารทอง
            case AiGhost.Kid_ghost:
                break;
            //ผีหญิงหุ่นรองเสื้อ
            case AiGhost.Mannequin_ghost:
                PlayAnimCombat(true);
                break;
            //กระสือ
            case AiGhost.Soi_Ju_ghost:
                Shoot_horizontal(GetComponent<SpriteRenderer>().flipX, 1.5f);
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
            spawnItem.transform.rotation = Rot;
            spawnItem.transform.position = transform.position + (spawnItem.transform.up * 1.1f);
            print("Rota : " + Rot);

            GameObject spawn;
            if (is_spawn)
            {
                spawn = Instantiate(spawnItem);
                spawn.GetComponent<Item_Attack_System>().ai_ghost = GetComponent<Variables>().declarations.Get<AiGhost>("Ai_Ghost");
                spawn.GetComponent<Item_Attack_System>().ghost = gameObject;
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

    [System.Obsolete]
    public void Random_Postition_Attack()
    {
        Ghost_Effects.SetActive(true);
        Ghoat_Effects_Animator.SetBool("IsPlay", true);
        for (int i = 0; i < 10; i++)
        {
            float random_X = Random.Range(XPoint[0].position.x, XPoint[1].position.x);
            float random_Z = Random.Range(YPoint[0].position.z, YPoint[1].position.z);

            Vector2 randomPos = new Vector2(random_X, random_Z);
            print("Pose Spawn" + randomPos);

            GameObject spawn;
            Rigidbody rigidbody;

            spawn = Instantiate(ObjectAttack);

            if (i == 0)
            {
                spawn.GetComponent<AudioSource>().Play();
            }

            if(i == 1)
            {
                spawn.GetComponent<Item_Script>().IsCanPlaySound = true;
            }
            else
            {
                spawn.GetComponent<Item_Script>().IsCanPlaySound = false;
            }

            spawn.transform.position = new Vector3(randomPos.x, XPoint[0].position.y, randomPos.y);
            rigidbody = spawn.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            spawn.GetComponent<Item_Attack_System>().ai_ghost = GetComponent<Variables>().declarations.Get<AiGhost>("Ai_Ghost");
            spawn.GetComponent<Item_Attack_System>().ghost = gameObject;
        }
    }

    public void Shoot_horizontal(bool flip, float scale = 1.0f, GameObject _gameObject = default, bool is_spawn = true)
    {
        Rigidbody rigidbody;

        if(ObjectAttack != null)
        {
            GameObject spawnItem;
            spawnItem = ObjectAttack;
            spawnItem.transform.rotation = Quaternion.Euler(0,0,0);
            if (GetComponent<SpriteRenderer>().flipX)
            {
                spawnItem.transform.position = transform.position + (spawnItem.transform.right * -1.1f);
            }
            else
            {
                spawnItem.transform.position = transform.position + (spawnItem.transform.right * 1.1f);
            }

            GameObject spawn;
            if (is_spawn)
            {
                spawn = Instantiate(spawnItem);
                spawn.GetComponent<Item_Attack_System>().ai_ghost = GetComponent<Variables>().declarations.Get<AiGhost>("Ai_Ghost");
                spawn.GetComponent<Item_Attack_System>().ghost = gameObject;
                spawn.GetComponent<Item_Attack_System>().isFlip = flip;
                spawn.GetComponent<Item_Attack_System>().Scale_Item = scale;
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
