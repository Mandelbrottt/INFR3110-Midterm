using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour {
	[SerializeField]
	private GameObject pauseMenu;

	[SerializeField]
	private Button resumeButton;

	[SerializeField]
	private Button restartButton;

	[SerializeField]
	private Button menuButton;

	private void Start() {
		resumeButton.onClick.AddListener(OnResumeButtonPressed);
		
		restartButton.onClick.AddListener(OnRestartButtonPressed);
		
		menuButton.onClick.AddListener(OnMenuButtonPressed);
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			SetPauseState(!pauseMenu.activeInHierarchy);
		}
	}

	public void OnResumeButtonPressed() {
		SetPauseState(false);
	}

	public void OnRestartButtonPressed() {
		SceneManager.LoadScene("PlayScene");
	}

	public void OnMenuButtonPressed() {
		SceneManager.LoadScene("StartScene");
	}

	private void SetPauseState(bool a_state) {
		pauseMenu.SetActive(a_state);

		Time.timeScale = a_state ? 0 : 1;
	}
}
