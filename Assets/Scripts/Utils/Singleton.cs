using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.Utils
{
    public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
    {
        static T _instance;
        public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if(_instance == null)
                    {
                        GameObject go = new GameObject($" Singleton");
                        go.AddComponent<T>();
                        _instance = go.GetComponent<T>();
                       

                    }
                }
                return _instance;
            }
        }
        private static T FindOrCreateInstance()
        {
            var instance = GameObject.FindObjectOfType<T>();
            if(instance != null)
            {
                return instance;
            }
            var name = typeof(T).Name + " Singleton";
            var containerGameObject = new GameObject(name);

            var singletonComponent = containerGameObject.AddComponent<T>();
            return singletonComponent;

        }
        protected virtual void Awake()
        {
            _instance = this as T;
            DontDestroyOnLoad(this);
        }
    }
}
