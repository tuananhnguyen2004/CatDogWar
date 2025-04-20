using System;
using UnityEngine;

namespace Utils
{
    // Singleton Generics
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                _instance = FindAnyObjectByType<T>();
                if (_instance == null)
                {
                    GameObject newGameObj = new();
                    newGameObj.name = typeof(T).Name;
                    _instance = newGameObj.AddComponent<T>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public static class Utils
    {
        public static float Map(this float currentValue, float oldMin, float oldMax, float newMin, float newMax)
        {
            return currentValue * ((newMax - newMin) / (oldMax - oldMin)) + newMin;
        }

        public static void Counter(ref float elapsedTime, float maxTime, Action action)
        {
            if (elapsedTime < maxTime)
            {
                elapsedTime += Time.deltaTime;
                return;
            }

            elapsedTime = 0;
            action.Invoke();
        }
    }
}

