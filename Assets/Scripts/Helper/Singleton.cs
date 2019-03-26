using System;
using UnityEngine;

namespace StackGame
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        protected static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    InitInstance();
                }
                return _instance;
            }

            set
            {
                if (value == null)
                {
                    RemoveInstance();
                }
                else
                {
                    throw new InvalidOperationException("Create your singleton in other way.");
                }
            }
        }

        public static void InitInstance()
        {
            if (_instance == null)
            {
                _instance = new T();
                _instance.Init();
            }
        }

        protected virtual void Init()
        {
            Debug.Log($"{this} : Init.");
        }

        protected virtual void Start()
        {
            if (_instance != this as T)
            {
                // Destroy
            }
        }

        protected virtual void DeInit()
        {
            Debug.Log($"{this} : DeInit.");
        }

        public static void RemoveInstance()
        {
            if (_instance != null)
            {
                _instance.DeInit();
                _instance = null;
            }
        }
    }
}