using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Ai : MonoBehaviour
{
    [SerializeField] GameObject Spawn_Ai_Ghost;
    public void On_Spawn_Ai()
    {
        Spawn_Ai_Ghost.transform.position = transform.position;
        Instantiate(Spawn_Ai_Ghost);
    }
}
