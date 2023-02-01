using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Attack : MonoBehaviour
{
    [SerializeField] private GameObject ObjectAttack;

    private Rigidbody testtt;

    void Update()
    {
        if (testtt != null)
            print("Speed : " + Vector3.Magnitude(testtt.velocity));
    }

    public void Attack(AiGhost selectAiGhost)
    {
        switch (selectAiGhost)
        {
            case AiGhost.Guard_ghost:
                Shoot();
                break;
            default:
                break;
        }
    }

    private void Shoot()
    {
        Rigidbody rigidbody;
        Quaternion Rot = Quaternion.Euler(0, 0, FuntionLibraly.Get2DLookAt(gameObject.transform.position, GameInstance.Player.GetComponent<Player_Movement>().HeadPoint.transform.position) - CulMath() + 55);
        

        if (ObjectAttack != null)
        {
            GameObject spawnItem;
            spawnItem = ObjectAttack;
            spawnItem.GetComponent<Add_item_to_character>().IsSpawn = true;
            spawnItem.transform.rotation = Rot;
            spawnItem.transform.position = transform.position + (spawnItem.transform.up * 1.2f);
            print("Rota : " + Rot);

            GameObject spawn = Instantiate(spawnItem);

            rigidbody = spawn.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(spawn.transform.up * (7 + Vector3.Magnitude(GetComponent<Rigidbody>().velocity)), ForceMode.Impulse);
            testtt = rigidbody;
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
