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
			if (Input.GetKeyDown(KeyCode.Return)) {
				if (m_hasStarted)
					m_logger.EndRun();

				m_elapsedTime = 0.0f;
				m_logger.StartRun();
			}

			if (Input.GetKeyDown(KeyCode.Space)) {
				m_logger.SaveCheckpointTime(m_elapsedTime);
				m_elapsedTime = 0;
			}

			for (var i = 0; i < 10; i++) {
				if (Input.GetKeyDown(KeyCode.Keypad0 + i)) {
					var checkpointTime = m_logger.GetCheckpointTime(i);
					Debug.Log($"Checkpoint {i} time: {checkpointTime}s");
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
