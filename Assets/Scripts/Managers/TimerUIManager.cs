using System;
using System.Collections.Generic;
using INFR3110;
using TMPro;
using UnityEngine;

public class TimerUIManager : MonoBehaviour {

	[Header("SimpleUI"), SerializeField]
	private GameObject simpleUI = null;

	[SerializeField]
	private TMP_Text elapsedTime = null;

	[Header("GhostUI"), SerializeField]
	private GameObject ghostUI = null;

	private List<TMP_Text> m_timers = new List<TMP_Text>();

	private CheckpointStruct[] m_ghostCheckpointStructs;

	private void Update() {
		if (!simpleUI.activeInHierarchy && !ghostUI.activeInHierarchy) {
			if (!CheckpointManager.Instance.HasGhostTimes) {
				simpleUI.SetActive(true);
			} else {
				ghostUI.SetActive(true);

				elapsedTime = ghostUI.GetComponentInChildren<TMP_Text>();

				int numCheckpoints      = CheckpointLogger.Instance.GetNumCheckpoints();
				int numGhostCheckpoints = CheckpointLogger.Instance.GhostGetNumCheckpoints();

				m_ghostCheckpointStructs = new CheckpointStruct[numGhostCheckpoints];

				{
					GameObject go = Instantiate(elapsedTime.gameObject);
					TMP_Text txt = go.GetComponent<TMP_Text>();
					txt.transform.parent = ghostUI.transform;
					txt.transform.localPosition = Vector3.zero;
					txt.transform.position -= new Vector3(-5, 50, 0);
					txt.transform.localScale = Vector3.one;
					m_timers.Add(txt);
				}
				for (int i = 0; i < numGhostCheckpoints; i++) {
					m_ghostCheckpointStructs[i] = CheckpointLogger.Instance.GhostGetCheckpoint(i);

					if (i == 0) continue;

					GameObject go  = Instantiate(m_timers[m_timers.Count - 1].gameObject);
					TMP_Text   txt = go.GetComponent<TMP_Text>();
					txt.transform.parent        =  ghostUI.transform;
					txt.transform.localPosition =  Vector3.zero;
					txt.transform.position -= new Vector3(-5, 50 * (i + 1), 0);
					txt.transform.localScale = Vector3.one;
					m_timers.Add(txt);
				}
			}
		}

		if (simpleUI.activeInHierarchy) {
			elapsedTime.text = $"Elapsed Time: {CheckpointManager.Instance.ElapsedTime:F2}";
		}

		if (ghostUI.activeInHierarchy) {
			elapsedTime.text = $"Elapsed Time: {CheckpointManager.Instance.ElapsedTime:F2}";

			for (int i = 0; i < CheckpointLogger.Instance.GetNumCheckpoints(); i++) {
				var cp = CheckpointLogger.Instance.GetCheckpoint(i);

				m_timers[i].text = (Math.Abs(m_ghostCheckpointStructs[i].timeStamp - (-1f)) > 0.1f) 
									   ? $"{cp.name} ({m_ghostCheckpointStructs[i].timeStamp:F2})" 
									   : $"{cp.name} (N/A)";

				if (i <= CheckpointManager.Instance.CurrentCheckpoint) {
					var curTime = (i < CheckpointManager.Instance.CurrentCheckpoint)
									  ? cp.timeStamp
									  : CheckpointManager.Instance.TimeSinceLastCheckpoint;

					var ghostTime = m_ghostCheckpointStructs[i].timeStamp;

					m_timers[i].text += $": {curTime:F2}";

					if (curTime <= ghostTime) {
						m_timers[i].text += $" (+{ghostTime - curTime:F2})";
						m_timers[i].color = Color.green;
					} else {
						m_timers[i].text += $" (-{curTime - ghostTime:F2})";
						m_timers[i].color = Color.red;
					}
				} else {
					m_timers[i].color = Color.white;
				}

			}
		}
	}
}
