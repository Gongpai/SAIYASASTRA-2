using System;
using UnityEngine;

namespace GDD.Sinagleton
{
    public class CanDestroy_Sinagleton<T> : DontDestroy_Singleton<T> where T : CanDestroy_Sinagleton<T>
    {
        public override void OnAwake()
        {
            
        }
    }
}