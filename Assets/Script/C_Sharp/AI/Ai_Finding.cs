using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Finding : MonoBehaviour
{
    public static Vector3 AiFindding(AiFindingMode findingMode, Vector2 ZRandomRange, Vector2 XRandomRange, Vector3 CharacterLocation, GameObject[] TargetToRandom = null)
    {
        Vector3 Location = new Vector3(0,0,0);
        switch (findingMode)
        {
            case AiFindingMode.FindingRandomLocation:
                Location = RandomLocation(ZRandomRange, XRandomRange, CharacterLocation);
                break;
            case AiFindingMode.FindingRandomTarget:
                Location = RandomTarget(TargetToRandom, CharacterLocation);
                break;
            default:
                break;
        }
        return Location;
    }

    private static Vector3 RandomLocation(Vector2 ZRandomRange, Vector2 XRandomRange, Vector3 CharacterLocation)
    {
        Vector3 Location;
        Location = new Vector3(Random.Range(XRandomRange.x, XRandomRange.y), CharacterLocation.y, Random.Range(ZRandomRange.x, ZRandomRange.y));

        return Location;
    }

    private static Vector3 RandomTarget(GameObject[] TargetToRandom, Vector3 CharacterLocation)
    {
        Vector3 Location = new Vector3(0, 0, 0);
        Vector3 TargetLocation;
        if (TargetToRandom.Length > 0)
        {
            TargetLocation = TargetToRandom[Random.Range(0, TargetToRandom.Length)].transform.position;
            Location = new Vector3(TargetLocation.x, CharacterLocation.y, TargetLocation.z);
        }

        return Location;
    }
}
