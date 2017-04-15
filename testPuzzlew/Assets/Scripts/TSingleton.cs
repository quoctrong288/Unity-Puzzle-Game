using UnityEngine;
public class TSingleton<T> : MonoBehaviour where T : Object
{
    [SerializeField]
    private bool m_IsNeedDestroy = false;
    private static bool m_IsNeedDestroyRef;
    private static T m_Instance;
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType<T>();
                if (!m_IsNeedDestroyRef)
                {
                    //Tell unity not to destroy this object when loading a new scene!
                    DontDestroyOnLoad(m_Instance as GameObject);
                }
            }
            return m_Instance;
        }
    }

    protected virtual void Awake()
    {
        m_IsNeedDestroyRef = m_IsNeedDestroy;
        if (m_Instance == null)
        {
            m_Instance = this as T;
            if (!m_IsNeedDestroyRef)
            {
                //If I am the first instance, make me the Singleton
                DontDestroyOnLoad(this);
            }
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != m_Instance)
                Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // reset this static var to null if it's the singleton instance
        if (m_Instance == this)
        {
            m_Instance = null;
        }
    }
}
