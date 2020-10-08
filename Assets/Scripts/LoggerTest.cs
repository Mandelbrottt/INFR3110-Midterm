using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace INFR3110 {
	public class LoggerTest : MonoBehaviour {
		private bool m_hasStarted = false;

		private float m_elapsedTime;

		private CheckpointLogger m_logger;
		
		private void Start() {
			m_logger = CheckpointLogger.Instance;
		}

		private void Update() {
			if (Input.GetKeyDown(KeyCode.C)) {
				var checkpoints = new Checkpoint[] {
					new Checkpoint() {
						index = 0,
						name = "Sent from C++",
						timeStamp = 0.4f,
					},
					new Checkpoint() {
						index = 1,
						name = "Something Else",
						timeStamp = 1.3f,
					},
					new Checkpoint() {
						index = 7,
						name = "Different",
						timeStamp = 2.5f,
					},
				};

				// Set the checkpoints in the DLL
				m_logger.SetCheckpoints(checkpoints, checkpoints.Length);

				for (var i = 0; i < m_logger.GetNumCheckpoints(); i++) {
					// Get the checkpoint back from DLL
					var cp = m_logger.GetCheckpoint(i);
					
					Debug.Log($"index: {cp.index}, timestamp: {cp.timeStamp}, name: {cp.name}");
				}
			}
			
			if (Input.GetKeyDown(KeyCode.Return)) {
				if (m_hasStarted)
					m_logger.EndRun();

				m_elapsedTime = 0.0f;
				m_logger.StartRun();
			}

			if (Input.GetKeyDown(KeyCode.Space)) {
				//m_logger.SaveCheckpointTime(m_elapsedTime);
				m_elapsedTime = 0;
			}

			for (var i = 0; i < 10; i++) {
				if (Input.GetKeyDown(KeyCode.Keypad0 + i)) {
					//var checkpointTime = m_logger.GetCheckpoint(i);
					//Debug.Log($"Checkpoint {i} time: {checkpointTime}s");
				}
			}

			if (Input.GetKeyDown(KeyCode.LeftShift)) {
				var totalTime = m_logger.GetTotalTime();
				Debug.Log($"Total time elapsed: {totalTime}s");
			}

			m_elapsedTime += Time.deltaTime;
		}
	}
}
