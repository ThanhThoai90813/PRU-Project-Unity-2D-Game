using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Component
{
    private static T instance;

    public static T Instance => instance;

    [SerializeField] protected bool dontDestroyOverload;

    void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

            if (dontDestroyOverload)
            {
                DontDestroyOnLoad(gameObject);
            }
                
            CustomAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
        
    protected virtual void CustomAwake(){}
}