using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (!_instance)
			{
				Debug.Log($"Singleton of type {typeof(T)} not contains in scene");
				return null;
			}

			return _instance;
		}
	}

	public virtual void Awake()
	{
		if (!_instance)
		{
			T[] managers = FindObjectsOfType(typeof(T)) as T[];
			if (managers.Length != 0)
			{
				if (managers.Length == 1)
				{
					_instance = managers[0];
				}
				else
				{
					Debug.LogError("You have more than one " + typeof(T).Name + " in the scene. You only need 1, it's a singleton!");
				}
			}
		}
	}
}
