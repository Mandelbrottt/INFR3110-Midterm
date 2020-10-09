using INFR3110;
using UnityEngine;

public class CharacterHitboxHandler : MonoBehaviour
{
	private void OnTriggerEnter(Collider a_other) {
		var otherGameobject = a_other.gameObject;

		if (a_other.gameObject.layer == LayerMask.NameToLayer("Checkpoint")) {
			CheckpointManager.Instance.TryTriggerCheckpoint(otherGameobject);
		}
	}
}
