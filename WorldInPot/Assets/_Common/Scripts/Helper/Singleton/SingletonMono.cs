using NaughtyAttributes;
using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T : Component
{
	[field: SerializeField, Foldout("Singleton")] public bool dontDestroyOnLoad = false;
	[SerializeField, Foldout("Singleton")] private bool killInstanceOnAwake = false;

	/// <summary>
	/// The instance.
	/// </summary>
	private static T instance;

	/// <summary>
	/// Gets the instance of the Singleton.
	/// </summary>
	/// <value>The instance.</value>
	public static T Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<T>(true);

			return instance;
		}
	}

    protected virtual void Awake()
	{
        if (instance == null)
            instance = this as T;
        else if (instance != this)
		{
			if (killInstanceOnAwake)
			{
				Destroy(instance.gameObject);
                instance = this as T;
			}
			else
			{
				Destroy(gameObject);
				return;
			}
		}

		if (dontDestroyOnLoad)
		{
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }

	protected virtual void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}
}