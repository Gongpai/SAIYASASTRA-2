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

    public static float Get2DLookAt(Vector2 entity, Vector2 target)
    {
        // Get Angle in Radians
        float AngleRad = Mathf.Atan2(target.y - entity.y, target.x - entity.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;

        return AngleDeg;
    }

    public static void ProgressBar_Fill(Image Pro_gressBar, float Point, float MaxPoint)
    {
        Pro_gressBar.fillAmount = Point / MaxPoint;
    }

    public static Vector2 Get_Screen_Point_Object(GameObject gameobject)
    {
        Camera maincam = Camera.main;
        Vector2 screen_point = maincam.WorldToScreenPoint(gameobject.transform.position);
        return screen_point;
    }

    public void GamePause_Component(GameObject gameObject, bool isPause)
    {
        if (!isPause)
        {
            if (gameObject.GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().enabled = true;
            }
            if (gameObject.GetComponent<Rigidbody>() != null)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        else
        {
            if (gameObject.GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().enabled = false;
            }
            if (gameObject.GetComponent<Rigidbody>() != null)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
