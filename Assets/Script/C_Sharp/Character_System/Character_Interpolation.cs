using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Interpolation : MonoBehaviour
{
    [SerializeField] Vector3 EndPoint;
    [SerializeField] float Speed = 1;
    [SerializeField] Animator animator;

    Vector3 StartPoint;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartPoint = transform.position;
        animator.SetFloat("Speed", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        GameInstance.Player = gameObject;
        if (time < 1)
        {
            time += Time.deltaTime * Speed;
            transform.position = Vector3.Lerp(StartPoint, EndPoint, time);
        }
    }

    private void OnDestroy()
    {
        GameInstance.Reset_Gameinstance();
        Make_DontDestroyOnLoad.Destroy_GameInstance();

    }
}
