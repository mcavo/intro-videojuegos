using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform player;

	private float xOffset = 0.0f;
	private float zOffset = 10.0f;
	
	// Update make after the rest of the updates done
	void LateUpdate () {
		transform.position = new Vector3(player.position.x + xOffset, transform.position.y, player.position.z - zOffset);
	}
}
