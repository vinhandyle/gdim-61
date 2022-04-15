using UnityEngine;

/// <summary>
/// An object that will persist between scenes.
/// </summary>
public class Singleton<Object> : MonoBehaviour where Object : Singleton<Object>
{
    public static Object Instance { private set; get; }

    public static bool isInitialized
    { 
        get { return Instance != null; }
    }

    protected virtual void Awake()
    {
        Instance = (Object)this;
    }

    protected virtual void OnDestroy()
    {
        if (Instance.Equals(this))
            Instance = null;
    }
}
