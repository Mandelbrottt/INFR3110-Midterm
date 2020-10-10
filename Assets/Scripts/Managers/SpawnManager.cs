using System.Runtime.InteropServices;
using INFR3110;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviourSingleton<SpawnManager> {
	[SerializeField]
	private CharacterController character = null;

	private Vector3    m_spawnPosition;
	private Quaternion m_spawnRotation;

	[SerializeField]
	private float deathPlaneHeight = -5f;

	private void Start() {
		var transformRef = character.transform;
		m_spawnPosition = transformRef.position;
		m_spawnRotation = transformRef.rotation;
	}

	private void Update() {
		if (character.transform.position.y < deathPlaneHeight) {

			if (!CheckpointManager.Instance.PassedLastCheckpoint)
				Respawn();
			else if (character.transform.position.y < deathPlaneHeight * 5) {
				SceneManager.LoadScene("EndScene");
			}

		}
	}

	public void SetSpawn(Transform a_transform) {
		m_spawnPosition = a_transform.position;
		m_spawnRotation = a_transform.rotation;
	}

	public void Respawn() {
		var transformRef = character.transform;
		transformRef.position = m_spawnPosition;
		transformRef.rotation = m_spawnRotation;
	}
}
