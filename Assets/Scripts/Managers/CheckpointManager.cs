using System.Collections.Generic;
using INFR3110;
using UnityEngine;

public class CheckpointManager : MonoBehaviourSingleton<CheckpointManager> {
	public HashSet<int> hitCheckpoints = new HashSet<int>();

	[SerializeField]
	private GameObject checkpointPrefab = null;

	[SerializeField]
	private List<GameObject> checkpointsInLevel = new List<GameObject>();

	private float m_timeSinceLastCheckpoint;
	private float m_elapsedTime;

	[HideInInspector] 
	public bool PassedLastCheckpoint { get; set; } = false;

	private void Start() {

		m_elapsedTime = 0f;
		m_timeSinceLastCheckpoint = 0f;

		var numValidCheckpoints = 0;

		var cps = new GameObject[checkpointsInLevel.Count];
		
		for (int i = 0; i < checkpointsInLevel.Count; i++) {
			var checkpointInLevel = checkpointsInLevel[i];

			if (checkpointInLevel != null) {
				var cp = Instantiate(checkpointPrefab);

				cp.transform.position   = checkpointInLevel.transform.position;
				cp.transform.rotation   = checkpointInLevel.transform.rotation;
				cp.transform.localScale = checkpointInLevel.transform.localScale;

				cp.gameObject.name = checkpointInLevel.name;
				
				cps[numValidCheckpoints] = cp;

				numValidCheckpoints++;
			}
		}
		
		var checkpoints = new CheckpointStruct[numValidCheckpoints];

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

			SpawnManager.Instance.SetSpawn(a_checkpoint.transform);
			
			Debug.Log("Checkpoint hit!");

			if (hitCheckpoints.Count == CheckpointLogger.Instance.GetNumCheckpoints()) {
				CheckpointLogger.Instance.EndRun();

				PassedLastCheckpoint = true;
			}
		}
	}
}
