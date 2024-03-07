using UnityEngine;

namespace GDD.TouchSystem
{
    public class TouchIdentifier
    {
        public int fingerId;
        public float timeCreated;
        public Vector2 startPosition;
        public Vector3 deltaPosition;

        public TouchIdentifier(int fingerId, float timeCreated, Vector2 startPosition, Vector3 deltaPosition = default)
        {
            this.fingerId = fingerId;
            this.timeCreated = timeCreated;
            this.startPosition = startPosition;
            this.deltaPosition = deltaPosition;
        }
    }
}