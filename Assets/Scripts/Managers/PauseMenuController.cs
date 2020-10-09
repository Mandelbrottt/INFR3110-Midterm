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

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible   = false;
	}

	private void OnDestroy() {
		Time.timeScale = 1.0f;
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
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible   = true;

		SceneManager.LoadScene("StartScene");
	}

	private void SetPauseState(bool a_state) {
		pauseMenu.SetActive(a_state);

		Time.timeScale = a_state ? 0f : 1f;

		Cursor.lockState = a_state ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = !a_state;
	}
}
