using UnityEngine;

public class MovingWall : MonoBehaviour {
	[SerializeField]
	private Transform startTransform = null;

	[SerializeField]
	private Transform endTransform = null;

	[SerializeField]
	private float oscillateFrequency = 1;

	[SerializeField]
	private float oscillateOffset = 1;

	private float m_time = 0;

	private void Start() {
		m_time = oscillateOffset;
	}

	private void Update() {
		m_time += Time.deltaTime;

		var t = Mathf.Sin(m_time * oscillateFrequency) * 0.5f + 0.5f;

		transform.position = Vector3.Lerp(startTransform.position, endTransform.position, t);
	}
}
