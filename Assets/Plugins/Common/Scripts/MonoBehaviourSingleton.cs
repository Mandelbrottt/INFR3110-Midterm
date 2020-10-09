using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour {
	protected static T s_instance;

	public static T Instance {
		get {
			if (s_instance == null) {
				var instances = FindObjectsOfType<T>();
				
				Debug.Assert(instances.Length != 0, $"An instance of {typeof(T)} is needed in the scene!");
				Debug.Assert(instances.Length == 1, $"There should only be one instance of {typeof(T)} in the scene!");

				s_instance = instances[0];
			}

			return s_instance;
		}
	}
}
