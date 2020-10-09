using System.Collections.Generic;
using INFR3110;
using UnityEngine;

public class CheckpointManager : MonoBehaviourSingleton<CheckpointManager> {
	public HashSet<int> hitCheckpoints = new HashSet<int>();

	[SerializeField]
	private GameObject checkpointPrefab = null;

	[SerializeField]
	private List<Transform> checkpointPositions = new List<Transform>();

	private float m_timeSinceLastCheckpoint;
	private float m_elapsedTime;

	private void Start() {

		m_elapsedTime = 0f;
		m_timeSinceLastCheckpoint = 0f;

		var cps = new GameObject[checkpointPositions.Count];
		
		for (int i = 0; i < checkpointPositions.Count; i++) {
			var cp = Instantiate(checkpointPrefab);
			
			cp.transform.position   = checkpointPositions[i].position;
			cp.transform.rotation   = checkpointPositions[i].rotation;
			cp.transform.localScale = checkpointPositions[i].localScale;

			cp.gameObject.name = $"Checkpoint {i}";

			cps[i] = cp;
		}
		
		var checkpoints = new CheckpointStruct[checkpointPositions.Count];

		for (var i = 0; i < checkpoints.Length; i++) {
			checkpoints[i].name = cps[i].name;
			checkpoints[i].timeStamp = 0.0f;
		}

		CheckpointLogger.Instance.SetCheckpoints(checkpoints, checkpoints.Length);

		CheckpointLogger.Instance.StartRun();
	}

	private void Update() {
		m_timeSinceLastCheckpoint += Time.deltaTime;
		m_elapsedTime += Time.deltaTime;
	}

	public void TryTriggerCheckpoint(GameObject a_checkpoint) {
		int instanceId = a_checkpoint.GetInstanceID();

		if (!hitCheckpoints.Contains(instanceId)) {
			CheckpointLogger.Instance.SaveCheckpointTime(m_timeSinceLastCheckpoint);

			hitCheckpoints.Add(instanceId);
			
			m_timeSinceLastCheckpoint = 0f;

			if (hitCheckpoints.Count == CheckpointLogger.Instance.GetNumCheckpoints()) {
				CheckpointLogger.Instance.EndRun();
			}
		}
	}
}
