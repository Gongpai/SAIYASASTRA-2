using System;
using Unity.Mathematics;
using UnityEngine;

namespace GDD.Util
{
    public static class VectorUtil
    {
        public static Vector3 GetVectorDistance(Vector3 a, Vector3 b, float distance)
        {
            Vector3 result = a - b;
            result = Vector3.Normalize(result);
            result *= distance;
            result += b;
            return result;
        }
    }

    [Serializable]
    public struct floatMinMax
    {
        public float min;
        public float max;

        public floatMinMax(float _min, float _max)
        {
            min = _min;
            max = _max;
        }
    }

    public struct float2D
    {
        public float x;
        public float y;
        
        public float2D(float _x, float _y)
        {
            x = _x;
            y = _y;
        }
    }
    
    public struct float3D
    {
        public float x;
        public float y;
        public float z;
        
        public float3D(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
    }
}