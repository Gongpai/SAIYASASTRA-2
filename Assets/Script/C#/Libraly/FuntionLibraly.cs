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

    public static float Get2DLookAt(Vector2 entity,  Vector2 target)
    {
        // Get Angle in Radians
        float AngleRad = Mathf.Atan2(target.y - entity.y, target.x - entity.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;

        return AngleDeg;
    }
}
