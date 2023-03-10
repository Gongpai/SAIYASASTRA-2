using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Make_DontDestroyOnLoad : MonoBehaviour
{
    public static GameObject make_dontdead;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        make_dontdead = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void Destroy_GameInstance()
    {
        Destroy(make_dontdead);
    }
}
