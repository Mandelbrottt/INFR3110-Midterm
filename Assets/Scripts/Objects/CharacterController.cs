using UnityEngine;

public class CharacterController : MonoBehaviour {
	private Rigidbody m_body = null;

	[SerializeField]
	private Transform groundTester = null;

	[SerializeField]
	private GameObject cameraPitchTarget = null;

	[SerializeField]
	private float groundDistance = 0.1f;

	private LayerMask m_groundLayer = 0;

	[SerializeField]
	private float moveSpeed = 10;

	[SerializeField]
	private float maxAccel = 100;
	[SerializeField]
	private float maxDecel = 100;

	[SerializeField]
	private float jumpForce = 10;

	[SerializeField]
	private float gravityScale = 3;

	[SerializeField]
	private float mouseSens = 100;

	private bool m_grounded = false;

	private float m_pitch = 0;
	private float m_yaw   = 0;

	private void Start() {
		m_body = GetComponent<Rigidbody>();

		m_groundLayer |= 1 << LayerMask.NameToLayer("Ground");
		m_groundLayer |= 1 << LayerMask.NameToLayer("Wall");
	}

	private void FixedUpdate() {
		var input = Vector3.zero;
		input.x = Input.GetAxisRaw("Horizontal");
		input.z = Input.GetAxisRaw("Vertical");

		var desiredVelocity = input.z * transform.forward + input.x * transform.right;

		var velocity = m_body.velocity;
		desiredVelocity = desiredVelocity.normalized * moveSpeed;

		var jump = Input.GetAxisRaw("Jump") > 0;

		var deltaVelocity = (desiredVelocity - velocity) / Time.deltaTime;
		deltaVelocity.y = 0;

		var limit = Vector3.Dot(deltaVelocity, velocity) > 0f ? maxAccel : maxDecel;

		var force = m_body.mass * Vector3.ClampMagnitude(deltaVelocity, limit);

		m_body.AddForce(force, ForceMode.Force);

		m_grounded = Physics.CheckSphere(
			groundTester.position, groundDistance, m_groundLayer, QueryTriggerInteraction.Ignore
		);
		var jumpAmt = m_grounded && jump ? jumpForce : 0;

		m_body.AddForce(new Vector3(0, jumpAmt, 0), ForceMode.Impulse);
		m_body.AddForce(Physics.gravity * (gravityScale - 1), ForceMode.Acceleration);

		var deltaYaw   = Input.GetAxisRaw("Mouse X") * mouseSens;
		var deltaPitch = Input.GetAxisRaw("Mouse Y") * mouseSens;

		m_yaw += deltaYaw;

		m_pitch = Mathf.Clamp(m_pitch - deltaPitch, -85f, 85f);

		transform.rotation = Quaternion.Euler(0, m_yaw, 0);

		cameraPitchTarget.transform.localRotation = Quaternion.Euler(m_pitch, 0, 0);
	}
}
