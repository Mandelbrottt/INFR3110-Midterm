using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
	public void OnStartButtonPressed() {
		SceneManager.LoadScene("PlayScene");
	}

	public void OnStatsButtonPressed() {
		SceneManager.LoadScene("EndScene");
	}
}
