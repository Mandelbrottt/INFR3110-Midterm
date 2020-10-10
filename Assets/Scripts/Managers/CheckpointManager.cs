using System.Collections.Generic;
using System.IO;
using INFR3110;
using UnityEngine;

public class CheckpointManager : MonoBehaviourSingleton<CheckpointManager> {
	public HashSet<int> hitCheckpoints = new HashSet<int>();

	[SerializeField]
	private GameObject checkpointPrefab = null;

	[SerializeField]
	private List<GameObject> checkpointsInLevel = new List<GameObject>();

	[HideInInspector] 
	public float TimeSinceLastCheckpoint { get; private set; }

	[HideInInspector]
	public float ElapsedTime { get; private set; }
	
	[HideInInspector]
	public int CurrentCheckpoint => hitCheckpoints.Count;

	[HideInInspector] 
	public bool HasGhostTimes { get; private set; } = false;

	[HideInInspector] 
	public bool PassedLastCheckpoint { get; set; } = false;
	private void Start() {

		ElapsedTime = 0f;
		TimeSinceLastCheckpoint = 0f;

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

		CheckpointLogger.Instance.ResetRun();
		
		var path = $"{Application.persistentDataPath}/ghosts/ghost.ghost";
		if (File.Exists(path)) {
			CheckpointLogger.Instance.GhostLoadFromFile(path);
			HasGhostTimes = true;
		}
	}

	private void Update() {
		if (!PassedLastCheckpoint) {
			TimeSinceLastCheckpoint += Time.deltaTime;
			ElapsedTime             += Time.deltaTime;
		}
	}

	public void TryTriggerCheckpoint(GameObject a_checkpoint) {
		int instanceId = a_checkpoint.GetInstanceID();

		if (!hitCheckpoints.Contains(instanceId)) {
			CheckpointLogger.Instance.SaveCheckpointTime(TimeSinceLastCheckpoint);

			hitCheckpoints.Add(instanceId);
			
			TimeSinceLastCheckpoint = 0f;

			SpawnManager.Instance.SetSpawn(a_checkpoint.transform);
			
			if (!PassedLastCheckpoint 
			    && hitCheckpoints.Count == CheckpointLogger.Instance.GetNumCheckpoints()) {
				PassedLastCheckpoint = true;

				float totalTime      = CheckpointLogger.Instance.GetTotalTime();
				float ghostTotalTime = CheckpointLogger.Instance.GhostGetTotalTime();

				if (!HasGhostTimes || totalTime < ghostTotalTime) {
					var path = $"{Application.persistentDataPath}/ghosts/ghost.ghost";
					CheckpointLogger.Instance.GhostSaveToFile(path);
				}
			}
		}
	}
}
