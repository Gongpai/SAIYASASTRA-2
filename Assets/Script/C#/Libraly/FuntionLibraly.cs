using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuntionLibraly : MonoBehaviour
{
    public static void DestroyWidget(GameObject Widget, GameObject Canvas, float timedelay)
    {
        Canvas.GetComponent<Animator>().SetBool("IsPlayReverse", true);
        print(Canvas.GetComponent<Animator>().GetBool("IsPlayReverse"));
        Destroy(Widget, timedelay);
    }
}
