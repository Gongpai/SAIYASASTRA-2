using System;
using UnityEngine;

namespace GDD.Sinagleton
{
    public class DontDestroy_Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        public void Awake()
        {
            OnAwake();
        }

        public virtual void OnAwake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}