using System;

namespace GDD.TouchSystem
{
    [Serializable]
    public struct TouchValue
    {
        public float duration;
        public float distance;
        public float angle;

        public TouchValue(float duration,float distance, float angle)
        {
            this.duration = duration;
            this.distance = distance;
            this.angle = angle;
        }
    }
}