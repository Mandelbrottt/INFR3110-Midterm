using System.Collections.Generic;
using System.IO;
using INFR3110;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatsMenuController : MonoBehaviourSingleton<StatsMenuController> {
	public Button menuButton;

	public List<TMP_Text> bestTimes;

	public List<TMP_Text> lastTimes;

	private void Start() {
		menuButton.onClick.AddListener(OnMenuButtonClicked);

		var path = $"{Application.persistentDataPath}/ghosts/ghost.ghost";
		if (File.Exists(path)) {
			CheckpointLogger.Instance.GhostLoadFromFile(path);
		}

		{
			float gTime = CheckpointLogger.Instance.GhostGetTotalTime();
			bestTimes[0].text = $"Best Time: ";
			if (gTime == -1)
				bestTimes[0].text += "N/A";
			else
				bestTimes[0].text += $"{gTime:F2}";

			for (int i = 0; i < CheckpointLogger.Instance.GetNumCheckpoints(); i++) {
				var cp  = CheckpointLogger.Instance.GetCheckpoint(i);
				var gcp = CheckpointLogger.Instance.GhostGetCheckpoint(i);
				bestTimes[i + 1].text = $"{cp.name}: ";
				if (gcp.timeStamp == -1)
					bestTimes[i + 1].text += "N/A";
				else
					bestTimes[i + 1].text += $"{gcp.timeStamp:F2}";
			}
		}
		{
			float tTime = CheckpointLogger.Instance.GetTotalTime();
			lastTimes[0].text = $"Last Time: ";
			if (tTime == -1)
				lastTimes[0].text += "N/A";
			else
				lastTimes[0].text += $"{tTime:F2}";

			for (int i = 0; i < CheckpointLogger.Instance.GetNumCheckpoints(); i++) {
				var cp  = CheckpointLogger.Instance.GetCheckpoint(i);
				lastTimes[i + 1].text = $"{cp.name}: ";
				if (cp.timeStamp == -1)
					lastTimes[i + 1].text += "N/A";
				else
					lastTimes[i + 1].text += $"{cp.timeStamp:F2}";
			}
		}
	}

	private void OnMenuButtonClicked() {
		SceneManager.LoadScene("StartScene");
	}
}
