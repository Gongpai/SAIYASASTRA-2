using System;
using UnityEngine;

namespace GDD.TouchSystem
{
    [Serializable]
    public struct TouchValue
    {
        public float duration;
        public float distance;
        public float angle;
        public Touch touch;

        public TouchValue(float duration,float distance, float angle, Touch touch)
        {
            this.duration = duration;
            this.distance = distance;
            this.angle = angle;
            this.touch = touch;
        }
    }
}