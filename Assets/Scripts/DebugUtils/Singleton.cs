using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = FindFirstObjectByType<T>();
        }
        else
        {
            Debug.LogWarning($"Attempted to create second instance of: {typeof(T).Name}. Destroying this instance.");
            Destroy(this);
        }
    }
}
